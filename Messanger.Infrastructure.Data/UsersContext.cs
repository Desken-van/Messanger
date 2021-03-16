using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Interfaces.Streaming;
using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Messanger.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Infrastructure.Data
{
    public class UsersContext : DbContext
    {
        public DbSet<AccountEntity> Logins { get; set; }
        public DbSet<MessageEntity> Sms { get; set; }
        public UsersContext(DbContextOptions<UsersContext> options)
                : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AccountEntity>(entity =>
            {

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
            modelBuilder.Entity<MessageEntity>(entity =>
            {

                entity.Property(e => e.Sms)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}
