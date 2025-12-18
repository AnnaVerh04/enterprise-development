using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.Infrastructure.Persistence;

/// <summary>
/// Контекст базы данных для работы с MongoDB через EF Core
/// </summary>
public class RealEstateDbContext : DbContext
{
    /// <summary>
    /// Коллекция контрагентов
    /// </summary>
    public DbSet<Counterparty> Counterparties { get; set; } = null!;

    /// <summary>
    /// Коллекция объектов недвижимости
    /// </summary>
    public DbSet<RealEstateProperty> Properties { get; set; } = null!;

    /// <summary>
    /// Коллекция заявок
    /// </summary>
    public DbSet<Request> Requests { get; set; } = null!;

    /// <summary>
    /// Конструктор контекста
    /// </summary>
    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Конфигурация моделей для MongoDB
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Counterparty>(entity =>
        {
            entity.ToCollection("counterparties");
            entity.HasKey(c => c.Id);
        });

        modelBuilder.Entity<RealEstateProperty>(entity =>
        {
            entity.ToCollection("properties");
            entity.HasKey(p => p.Id);
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.ToCollection("requests");
            entity.HasKey(r => r.Id);
        });
    }
}
