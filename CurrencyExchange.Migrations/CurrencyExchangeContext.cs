using CurrencyExchange.Contracts.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Migrations;

/// <summary>
/// add-migration Initial -StartupProject CurrencyExchange.WebApi.Host -Project CurrencyExchange.Migrations
/// update-database Initial -StartupProject CurrencyExchange.WebApi.Host -Project CurrencyExchange.Migrations
/// </summary>
public partial class CurrencyExchangeContext : DbContext
{
    public CurrencyExchangeContext(DbContextOptions<CurrencyExchangeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<CurrencyEntity> Currencies { get; set; }
    public virtual DbSet<UserAccountEntity> UserAccounts { get; set; }
    public virtual DbSet<TransactionEntity> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();
        modelBuilder.HasPostgresExtension("uuid-ossp");
    }
}
