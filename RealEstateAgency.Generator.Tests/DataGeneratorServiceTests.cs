using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NATS.Client.Core;
using RealEstateAgency.Contracts.Dto;
using RealEstateAgency.Generator.Services;
using System.Text.Json;
using Testcontainers.Nats;

namespace RealEstateAgency.Generator.Tests;

/// <summary>
/// Интеграционные тесты сервиса генерации данных
/// </summary>
[Collection("NatsIntegration")]
public class DataGeneratorServiceTests : IAsyncLifetime
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly NatsContainer _natsContainer;
    private NatsPublisher? _publisher;
    private readonly Mock<ILogger<NatsPublisher>> _publisherLoggerMock;
    private readonly Mock<ILogger<DataGeneratorService>> _serviceLoggerMock;

    public DataGeneratorServiceTests()
    {
        _natsContainer = new NatsBuilder()
            .WithImage("nats:latest")
            .Build();

        _publisherLoggerMock = new Mock<ILogger<NatsPublisher>>();
        _serviceLoggerMock = new Mock<ILogger<DataGeneratorService>>();
    }

    public async Task InitializeAsync()
    {
        await _natsContainer.StartAsync();
        _publisher = new NatsPublisher(_natsContainer.GetConnectionString(), _publisherLoggerMock.Object);
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
    public async Task ExecuteAsync_ShouldPublishCounterpartiesToCorrectTopic()
    {
        var receivedCounterparties = new List<CreateCounterpartyDto>();
        var allReceived = new TaskCompletionSource<bool>();

        var options = new NatsOpts { Url = _natsContainer.GetConnectionString() };
        await using var subscriber = new NatsConnection(options);
        await subscriber.ConnectAsync();

        var subscription = Task.Run(async () =>
        {
            await foreach (var msg in subscriber.SubscribeAsync<string>(DataGeneratorService.CounterpartyTopic))
            {
                if (!string.IsNullOrEmpty(msg.Data))
                {
                    var dto = JsonSerializer.Deserialize<CreateCounterpartyDto>(msg.Data, _jsonOptions);
                    if (dto != null)
                    {
                        receivedCounterparties.Add(dto);
                        if (receivedCounterparties.Count >= 3)
                        {
                            allReceived.TrySetResult(true);
                        }
                    }
                }
            }
        });

        await Task.Delay(100);

        var service = new DataGeneratorService(
            _publisher!,
            _serviceLoggerMock.Object,
            batchSize: 3,
            delayBetweenBatchesMs: 100);

        using var cts = new CancellationTokenSource();

        var serviceTask = service.StartAsync(cts.Token);

        await allReceived.Task.WaitAsync(TimeSpan.FromSeconds(30));

        cts.Cancel();
        await service.StopAsync(CancellationToken.None);

        receivedCounterparties.Should().HaveCountGreaterThanOrEqualTo(3);
        receivedCounterparties.Should().AllSatisfy(c =>
        {
            c.FullName.Should().NotBeNullOrEmpty();
            c.PassportNumber.Should().NotBeNullOrEmpty();
            c.PhoneNumber.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task ExecuteAsync_ShouldPublishPropertiesToCorrectTopic()
    {
        var receivedProperties = new List<CreateRealEstatePropertyDto>();
        var allReceived = new TaskCompletionSource<bool>();

        var options = new NatsOpts { Url = _natsContainer.GetConnectionString() };
        await using var subscriber = new NatsConnection(options);
        await subscriber.ConnectAsync();

        var subscription = Task.Run(async () =>
        {
            await foreach (var msg in subscriber.SubscribeAsync<string>(DataGeneratorService.PropertyTopic))
            {
                if (!string.IsNullOrEmpty(msg.Data))
                {
                    var dto = JsonSerializer.Deserialize<CreateRealEstatePropertyDto>(msg.Data, _jsonOptions);
                    if (dto != null)
                    {
                        receivedProperties.Add(dto);
                        if (receivedProperties.Count >= 3)
                        {
                            allReceived.TrySetResult(true);
                        }
                    }
                }
            }
        });

        await Task.Delay(100);

        var service = new DataGeneratorService(
            _publisher!,
            _serviceLoggerMock.Object,
            batchSize: 3,
            delayBetweenBatchesMs: 100);

        using var cts = new CancellationTokenSource();

        var serviceTask = service.StartAsync(cts.Token);

        await allReceived.Task.WaitAsync(TimeSpan.FromSeconds(30));

        cts.Cancel();
        await service.StopAsync(CancellationToken.None);

        receivedProperties.Should().HaveCountGreaterThanOrEqualTo(3);
        receivedProperties.Should().AllSatisfy(p =>
        {
            p.Address.Should().NotBeNullOrEmpty();
            p.CadastralNumber.Should().NotBeNullOrEmpty();
            p.TotalArea.Should().BeGreaterThan(0);
        });
    }

    [Fact]
    public async Task ExecuteAsync_ShouldPublishBothCounterpartiesAndProperties()
    {
        var counterpartyCount = 0;
        var propertyCount = 0;
        var allReceived = new TaskCompletionSource<bool>();

        var options = new NatsOpts { Url = _natsContainer.GetConnectionString() };
        await using var subscriber = new NatsConnection(options);
        await subscriber.ConnectAsync();

        var counterpartySub = Task.Run(async () =>
        {
            await foreach (var msg in subscriber.SubscribeAsync<string>(DataGeneratorService.CounterpartyTopic))
            {
                if (!string.IsNullOrEmpty(msg.Data))
                {
                    Interlocked.Increment(ref counterpartyCount);
                    CheckCompletion();
                }
            }
        });

        var propertySub = Task.Run(async () =>
        {
            await foreach (var msg in subscriber.SubscribeAsync<string>(DataGeneratorService.PropertyTopic))
            {
                if (!string.IsNullOrEmpty(msg.Data))
                {
                    Interlocked.Increment(ref propertyCount);
                    CheckCompletion();
                }
            }
        });

        void CheckCompletion()
        {
            if (counterpartyCount >= 5 && propertyCount >= 5)
            {
                allReceived.TrySetResult(true);
            }
        }

        await Task.Delay(100);

        var service = new DataGeneratorService(
            _publisher!,
            _serviceLoggerMock.Object,
            batchSize: 5,
            delayBetweenBatchesMs: 100);

        using var cts = new CancellationTokenSource();

        var serviceTask = service.StartAsync(cts.Token);

        await allReceived.Task.WaitAsync(TimeSpan.FromSeconds(30));

        cts.Cancel();
        await service.StopAsync(CancellationToken.None);

        counterpartyCount.Should().BeGreaterThanOrEqualTo(5);
        propertyCount.Should().BeGreaterThanOrEqualTo(5);
    }

    [Fact]
    public async Task StopAsync_ShouldStopGracefully()
    {
        var service = new DataGeneratorService(
            _publisher!,
            _serviceLoggerMock.Object,
            batchSize: 10,
            delayBetweenBatchesMs: 1000);

        using var cts = new CancellationTokenSource();

        var serviceTask = service.StartAsync(cts.Token);

        await Task.Delay(500);

        cts.Cancel();

        var act = async () => await service.StopAsync(CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public void Constructor_ShouldAcceptCustomParameters()
    {
        var service = new DataGeneratorService(
            _publisher!,
            _serviceLoggerMock.Object,
            batchSize: 100,
            delayBetweenBatchesMs: 10000);

        service.Should().NotBeNull();
    }
}

/// <summary>
/// Тесты проверки топиков
/// </summary>
public class DataGeneratorServiceTopicsTests
{
    [Fact]
    public void CounterpartyTopic_ShouldHaveCorrectValue()
    {
        DataGeneratorService.CounterpartyTopic.Should().Be("realestate.counterparty.created");
    }

    [Fact]
    public void PropertyTopic_ShouldHaveCorrectValue()
    {
        DataGeneratorService.PropertyTopic.Should().Be("realestate.property.created");
    }

    [Fact]
    public void RequestTopic_ShouldHaveCorrectValue()
    {
        DataGeneratorService.RequestTopic.Should().Be("realestate.request.created");
    }
}