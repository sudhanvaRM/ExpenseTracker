using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Server.Models.Entities
{
    public partial class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<Split> Splits { get; set; }
        public virtual DbSet<SplitParticipant> SplitParticipants { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserBalance> UserBalances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasKey(e => e.DebitId).HasName("expense_pkey");

                entity.ToTable("expense");

                entity.Property(e => e.DebitId).HasColumnName("debit_id");
                entity.Property(e => e.Amount)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount");
                entity.Property(e => e.Category)
                    .HasMaxLength(20)
                    .HasColumnName("category");
                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .HasColumnName("comment");
                entity.Property(e => e.Timestamp)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("timestamp");
                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Income>(entity =>
            {
                entity.HasKey(e => e.CreditId).HasName("income_pkey");

                entity.ToTable("income");

                entity.Property(e => e.CreditId).HasColumnName("credit_id");
                entity.Property(e => e.Amount)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount");
                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .HasColumnName("comment");
                entity.Property(e => e.Timestamp)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("timestamp");
                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User).WithMany(p => p.Incomes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("users_pkey");

                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Email)
                    .HasMaxLength(320)
                    .HasColumnName("email");
                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                // Updated navigation property name to 'Splits'
                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Split>(entity =>
            {
                entity.ToTable("splits");

                entity.HasKey(e => e.SplitId)
                      .HasName("split_pkey");

                entity.Property(e => e.SplitId)
                      .HasColumnName("split_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.TransactionId)
                      .HasColumnName("transaction_id")
                      .IsRequired();

                entity.Property(e => e.UserId)
                      .HasColumnName("user_id")
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Relationships
                entity.HasOne(e => e.Expense)
                      .WithMany()
                      .HasForeignKey(e => e.TransactionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.User)
                      .WithMany(u => u.Splits)  // Changed to 'Splits' to match the navigation property
                      .HasForeignKey(s => s.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

           modelBuilder.Entity<SplitParticipant>(entity =>
          {
            entity.ToTable("split_participants");
            entity.HasKey(sp => new { sp.SplitId, sp.ParticipantId }); // Composite Primary Key

            // Add default value for PaidStatus
            entity.Property(sp => sp.PaidStatus)
                .HasColumnName("paid_status")
                .HasDefaultValue(false); // Set default value to false

            entity.Property(e => e.ParticipantId)
                .HasColumnName("participant_id")
                .IsRequired();

            entity.Property(e => e.TransactionID)
                .HasColumnName("transaction_id")
                .IsRequired();

            entity.Property(e => e.SplitId)
                .HasColumnName("split_id")
                .IsRequired();

            entity.Property(e => e.Amount)
                .HasColumnName("amount")
                .IsRequired();

    // Relationship with Split
            entity.HasOne(sp => sp.Split) // Many-to-One relationship with Split
                .WithMany() // A Split can have many participants
                .HasForeignKey(sp => sp.SplitId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete on Split deletion

            // Relationship with Participant/User
            entity.HasOne(sp => sp.Participant) // Many-to-One relationship with User
                .WithMany() // A User can participate in many splits
                .HasForeignKey(sp => sp.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete on User deletion

            // New: Relationship with Expense
            entity.HasOne(sp => sp.Expense) // Many-to-One relationship with Expense
                .WithMany(e => e.SplitParticipants) // An Expense can have many participants
                .HasForeignKey(sp => sp.TransactionID) // Foreign key linking to Expense
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete on Expense deletion
        });


            modelBuilder.Entity<UserBalance>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("user_balance_pkey");

                entity.ToTable("user_balance");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");

                entity.Property(e => e.Balance)
                    .HasPrecision(10, 2)
                    .HasColumnName("balance");

                entity.HasOne(d => d.User).WithOne(p => p.UserBalance)
                    .HasForeignKey<UserBalance>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_users");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
