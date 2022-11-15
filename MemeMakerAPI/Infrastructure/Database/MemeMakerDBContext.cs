using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Domain.Entities;

namespace Infrastructure.Database
{
    public partial class MemeMakerDBContext : DbContext
    {
        public MemeMakerDBContext()
        {
        }

        public MemeMakerDBContext(DbContextOptions<MemeMakerDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApiKey> ApiKey { get; set; } = null!;
        public virtual DbSet<GeneratedMeme> GeneratedMeme { get; set; } = null!;
        public virtual DbSet<Template> Template { get; set; } = null!;
        public virtual DbSet<TemplateUsage> TemplateUsage { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiKey>(entity =>
            {
                entity.Property(e => e.active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.api_key)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.create_date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.expire_date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(dateadd(year,(1),getdate()))");
            });

            modelBuilder.Entity<GeneratedMeme>(entity =>
            {
                entity.Property(e => e.create_date).HasColumnType("datetime");

                entity.Property(e => e.path)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.template)
                    .WithMany(p => p.GeneratedMeme)
                    .HasForeignKey(d => d.template_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GeneratedMeme_Template");
            });

            modelBuilder.Entity<Template>(entity =>
            {
                entity.Property(e => e.create_date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.path)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TemplateUsage>(entity =>
            {
                entity.Property(e => e.date).HasColumnType("datetime");

                entity.HasOne(d => d.template)
                    .WithMany(p => p.TemplateUsage)
                    .HasForeignKey(d => d.template_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateUsage_Template");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
