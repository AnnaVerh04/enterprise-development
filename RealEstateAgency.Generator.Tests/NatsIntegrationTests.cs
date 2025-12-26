using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NATS.Client.Core;
using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Generator.Generators;
using RealEstateAgency.Generator.Services;
using Testcontainers.Nats;

namespace RealEstateAgency.Generator.Tests;

/// <summary>
/// Интеграционные тесты NATS с использованием Testcontainers
/// </summary>
[Collection("NatsIntegration")]
public class NatsIntegrationTests : IAsyncLifetime
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly NatsContainer _natsContainer;
    private NatsPublisher? _publisher;
    private readonly Mock<ILogger<NatsPublisher>> _loggerMock;

    public NatsIntegrationTests()
    {
        _natsContainer = new NatsBuilder()
            .WithImage("nats:latest")
            .Build();

        _loggerMock = new Mock<ILogger<NatsPublisher>>();
    }

    public async Task InitializeAsync()
    {
        await _natsContainer.StartAsync();
        _publisher = new NatsPublisher(_natsContainer.GetConnectionString(), _loggerMock.Object);
    }

    public async Task DisposeAsync()
    {
        if (_publisher != null)
        {
            await _publisher.DisposeAsync();
        }
        await _natsContainer.DisposeAsync();
    }

    [Fact]
    public async Task ConnectAsync_ShouldConnectToNats()
    {
        await _publisher!.ConnectAsync();

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Успешное подключение")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task PublishAsync_ShouldPublishCounterparty()
    {
        var counterparty = ContractGenerator.GenerateCounterparty();
        var receivedData = new TaskCompletionSource<CreateCounterpartyDto>();

        await _publisher!.ConnectAsync();

        var options = new NatsOpts { Url = _natsContainer.GetConnectionString() };
        await using var subscriber = new NatsConnection(options);
        await subscriber.ConnectAsync();

        var testTopic = "test.counterparty.topic";

        var subscription = Task.Run(async () =>
        {
            await foreach (var msg in subscriber.SubscribeAsync<string>(testTopic))
            {
                if (!string.IsNullOrEmpty(msg.Data))
                {
                    var dto = JsonSerializer.Deserialize<CreateCounterpartyDto>(msg.Data, _jsonOptions);
                    if (dto != null)
                    {
                        receivedData.SetResult(dto);
                        break;
                    }
                }
            }
        });

        await Task.Delay(100);

        await _publisher.PublishAsync(testTopic, counterparty);

        var received = await receivedData.Task.WaitAsync(TimeSpan.FromSeconds(5));
        received.FullName.Should().Be(counterparty.FullName);
        received.PassportNumber.Should().Be(counterparty.PassportNumber);
        received.PhoneNumber.Should().Be(counterparty.PhoneNumber);
    }

    [Fact]
    public async Task PublishAsync_ShouldPublishProperty()
    {
        var property = ContractGenerator.GenerateProperty();
        var receivedData = new TaskCompletionSource<CreateRealEstatePropertyDto>();

        await _publisher!.ConnectAsync();

        var options = new NatsOpts { Url = _natsContainer.GetConnectionString() };
        await using var subscriber = new NatsConnection(options);
        await subscriber.ConnectAsync();

        var subscription = Task.Run(async () =>
        {
            await foreach (var msg in subscriber.SubscribeAsync<string>("test.property"))
            {
                if (!string.IsNullOrEmpty(msg.Data))
                {
                    var dto = JsonSerializer.Deserialize<CreateRealEstatePropertyDto>(msg.Data, _jsonOptions);
                    if (dto != null)
                    {
                        receivedData.SetResult(dto);
                        break;
                    }
                }
            }
        });

        await Task.Delay(100);

        await _publisher.PublishAsync("test.property", property);

        var received = await receivedData.Task.WaitAsync(TimeSpan.FromSeconds(5));
        received.Address.Should().Be(property.Address);
        received.CadastralNumber.Should().Be(property.CadastralNumber);
        received.Type.Should().Be(property.Type);
    }

    [Fact]
    public async Task PublishStreamAsync_ShouldPublishMultipleMessages()
    {
        var messages = new List<CreateCounterpartyDto>
        {
            ContractGenerator.GenerateCounterparty(),
            ContractGenerator.GenerateCounterparty(),
            ContractGenerator.GenerateCounterparty()
        };

        var receivedMessages = new List<CreateCounterpartyDto>();
        var allReceived = new TaskCompletionSource<bool>();

        await _publisher!.ConnectAsync();

        var options = new NatsOpts { Url = _natsContainer.GetConnectionString() };
        await using var subscriber = new NatsConnection(options);
        await subscriber.ConnectAsync();

        var subscription = Task.Run(async () =>
        {
            await foreach (var msg in subscriber.SubscribeAsync<string>("test.stream"))
            {
                if (!string.IsNullOrEmpty(msg.Data))
                {
                    var dto = JsonSerializer.Deserialize<CreateCounterpartyDto>(msg.Data, _jsonOptions);
                    if (dto != null)
                    {
                        receivedMessages.Add(dto);
                        if (receivedMessages.Count >= 3)
                        {
                            allReceived.SetResult(true);
                            break;
                        }
                    }
                }
            }
        });

        await Task.Delay(100);

        await _publisher.PublishStreamAsync("test.stream", ToAsyncEnumerable(messages));

        await allReceived.Task.WaitAsync(TimeSpan.FromSeconds(10));
        receivedMessages.Should().HaveCount(3);
    }

    private static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            await Task.Yield();
            yield return item;
        }
    }
}

/// <summary>
/// Тесты NatsPublisher с мокированием (без реального NATS)
/// </summary>
public class NatsPublisherUnitTests
{
    [Fact]
    public async Task ConnectAsync_WithInvalidUrl_ShouldRetryAndEventuallyFail()
    {
        var loggerMock = new Mock<ILogger<NatsPublisher>>();
        var publisher = new NatsPublisher("nats://invalid-host:9999", loggerMock.Object);

        var act = async () =>
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await publisher.ConnectAsync(cts.Token);
        };

        await act.Should().ThrowAsync<OperationCanceledException>();

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Попытка подключения")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);

        await publisher.DisposeAsync();
    }

    [Fact]
    public async Task DisposeAsync_ShouldNotThrow()
    {
        var loggerMock = new Mock<ILogger<NatsPublisher>>();
        var publisher = new NatsPublisher("nats://localhost:4222", loggerMock.Object);

        var act = async () => await publisher.DisposeAsync();
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task DisposeAsync_CalledTwice_ShouldNotThrow()
    {
        var loggerMock = new Mock<ILogger<NatsPublisher>>();
        var publisher = new NatsPublisher("nats://localhost:4222", loggerMock.Object);

        await publisher.DisposeAsync();
        var act = async () => await publisher.DisposeAsync();
        await act.Should().NotThrowAsync();
    }
}