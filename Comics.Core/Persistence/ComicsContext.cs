using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Comics.Core.Persistence
{
    public interface IComicsRepository
    {
        Comic GetLastImportedComic(ComicType type);
        void InsertComic(Comic comic);
    }

    public class ComicsContext : DbContext
    {
        public ComicsContext() : base()
        {
        }

        public ComicsContext(string connection) : base(connection)
        {
        }

        public DbSet<Comic> Comics { get; set; }
    }
}
