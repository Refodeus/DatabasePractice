using System;
using System.Collections.Generic;
using DatabasePractice.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabasePractice.DbContexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public IQueryable<Client> get_avg_check()
    {
        return Clients.FromSqlRaw("SELECT * FROM get_avgv_check()");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=db;Port=5432;Database=project;Username=common_user;Password=15a16k;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Clients_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Orders_pkey");

            entity.HasOne(o => o.КлиентNavigation).WithMany()
            .HasForeignKey(o => o.Клиент).HasConstraintName("Клиент_fk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.ДатаИВремя).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Статус).HasDefaultValueSql("'Не обработан'::text");
        });

        modelBuilder.Entity<GetAvgCheck>(entity =>
        {
            entity.HasNoKey();
        });
        modelBuilder.Entity<GetPurchasesOfBirthday>(entity =>
        {
            entity.HasNoKey();
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
