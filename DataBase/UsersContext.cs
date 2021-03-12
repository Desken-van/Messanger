using Messanger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messanger.DataBase
{
    public partial class UsersContext : DbContext
    {
        public DbSet<Account> Logins { get; set; }
        public DbSet<SMS> Sms { get; set; }
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("logins");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SMS>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PK__sms__333823AA8699F1B4");

                entity.ToTable("sms");

                entity.Property(e => e.Sms)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SMS");
            });
        }
    }
}