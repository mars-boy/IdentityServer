using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenValidation.EntityFramework
{
    public class AgilysysContext : DbContext
    {
        public AgilysysContext(DbContextOptions options) : base(options) { }

        public DbSet<ClientToken> ClientTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<ClientToken>()
                .Property(c => c.CreatedTs)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
