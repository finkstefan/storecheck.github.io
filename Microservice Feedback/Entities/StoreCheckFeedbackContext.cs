using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Microservice_Feedback.Entities
{
    public partial class StoreCheckFeedbackContext : DbContext
    { 

        public StoreCheckFeedbackContext(DbContextOptions<StoreCheckFeedbackContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<FeedbackCategory> FeedbackCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:Feedback");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AI");

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.FeedbackId).ValueGeneratedNever();

                entity.HasOne(d => d.FeedbackCategory)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.FeedbackCategoryId)
                    .HasConstraintName("FK_Feedback_FeedbackCategory");
            });

            modelBuilder.Entity<FeedbackCategory>(entity =>
            {
                entity.Property(e => e.FeedbackCategoryId).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
