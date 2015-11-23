using System;
using System.Collections.Generic;
using System.Linq;

namespace Comics.Core.Persistence
{
    public interface IComicsRepository
    {
        Comic GetLastImportedComic(ComicType type);
        void InsertComic(Comic comic);

        IReadOnlyCollection<Comic> GetLatestComics(ComicType type, int count = 10);
    }

    public class ComicsRepository : IComicsRepository
    {
        private readonly ComicsContext _context;

        public ComicsRepository(ComicsContext context)
        {
            _context = context;
        }

        public Comic GetLastImportedComic(ComicType type)
        {
            return _context.Comics
                .Where(c => c.ComicType == type)
                .OrderByDescending(c => c.PublishedDate)
                .FirstOrDefault();
        }

        public void InsertComic(Comic comic)
        {
            _context.Comics.Add(comic);
            _context.SaveChanges();
        }

        public IReadOnlyCollection<Comic> GetLatestComics(ComicType type, int count = 10)
        {
            return _context.Comics
                .Where(c => c.ComicType == type)
                .OrderByDescending(c => c.PublishedDate)
                .Take(count)
                .ToList();
        }
    }
}