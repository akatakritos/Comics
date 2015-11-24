using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Parsers;
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
            var lastComicDate = lastComic?.PublishedDate ?? (defaultStart ?? new DateTime(2015, 11, 20));

            var downloader = new DilbertDownloader(_client);
            if (Today.HasValue)
                downloader.Today = Today.Value;

            var results = downloader.GetNewComics(lastComicDate.Date);
            foreach (var result in results)
            {
                var parseResult = DilbertParser.Parse(result.Content);
                var comic = new Comic()
                {
                    PublishedDate = parseResult.PublishedDate,
                    Permalink = result.Permalink.ToString(),
                    ComicType = ComicType.Dilbert,
                    ImageSrc = parseResult.ImageUri.ToString(),
                    ComicNumber = result.ComicNumber,
                };

                _repository.InsertComic(comic);
            }
        }
    }
}
