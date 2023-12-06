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

        //modelBuilder.Entity<UserEntity>(entity =>
        //{
        //    entity.ToTable("user", "public");

        //    entity.Property(e => e.UserId)
        //        .HasColumnName("user_id")
        //        .HasDefaultValueSql("public.uuid_generate_v4()");

        //    entity.Property(e => e.CreatedOn)
        //        .HasColumnType("timestamp without time zone")
        //        .HasColumnName("created_on");

        //    entity.Property(e => e.FirstName).HasColumnName("first_name");

        //    entity.Property(e => e.LastName).HasColumnName("last_name");

        //    entity.Property(e => e.Login).HasColumnName("login");

        //    entity.Property(e => e.UpdatedOn)
        //        .HasColumnType("timestamp without time zone")
        //        .HasColumnName("updated_on");
        //});
    }
}
