using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Persistence;

namespace Comics.Core.Downloaders
{
    public interface IDilbertImporter
    {
        void ImportNewComics(DateTime? defaultStart = null);
    }

    public class DilbertImporter : IDilbertImporter
    {
        private readonly IComicsRepository _repository;
        private readonly IDilbertWebClient _client;

        public DateTime? Today { get; set; } = null;

        public DilbertImporter(IComicsRepository repository, IDilbertWebClient client)
        {
            _repository = repository;
            _client = client;
        }

        public void ImportNewComics(DateTime? defaultStart = null)
        {
            var lastComic = _repository.GetLastImportedComic(ComicType.Dilbert);

            var downloader = new DilbertDownloader(_client);
            if (Today.HasValue)
                downloader.Today = Today.Value;

            var results = downloader.GetNewComicsSince(lastComic);
            foreach (var comic in results)
            {

                _repository.InsertComic(comic);
            }
        }
    }
}
