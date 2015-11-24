using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Comics.Core.Persistence
{
    public class ComicsContext : DbContext
    {
        public ComicsContext() : base()
        {
        }

        public ComicsContext(string connection) : base(connection)
        {
        }

        public DbSet<Comic> Comics { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("comics");
            base.OnModelCreating(modelBuilder);
        }
    }
}
