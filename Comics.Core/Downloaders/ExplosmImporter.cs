using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Parsers;
using Comics.Core.Persistence;

namespace Comics.Core.Downloaders
{
    public interface IExplosmImporter
    {
        void ImportNewComics(int defaultStart = 4125);
    }

    public class ExplosmImporter : IExplosmImporter
    {
        private readonly IComicsRepository _comicsRepository;
        private readonly IExplosmWebClient _explosmClient;

        public ExplosmImporter(IComicsRepository comicsRepository, IExplosmWebClient explosmClient)
        {
            _comicsRepository = comicsRepository;
            _explosmClient = explosmClient;
        }

        public void ImportNewComics(int defaultStart = 4125)
        {
            var lastComic = _comicsRepository.GetLastImportedComic(ComicType.Explosm);

            var lastComicNumber = lastComic?.ComicNumber ?? defaultStart;

            var downloader = new ExplosmDownloader(_explosmClient);
            var results = downloader.GetNewComics(lastComicNumber);

            foreach (var result in results)
            {
                var parseResult = ExplosmParser.Parse(result.Content);
                var comic = new Comic()
                {
                    ComicType = ComicType.Explosm,
                    ComicNumber = result.ComicNumber,
                    ImageSrc = parseResult.ImageUri.ToString(),
                    PublishedDate = parseResult.PublishedDate,
                    Permalink = result.Permalink.ToString(),
                };

                _comicsRepository.InsertComic(comic);
            }
        }
    }
}
