using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Parsers;
using Comics.Core.Persistence;

namespace Comics.Core.Downloaders
{
    public class ExplosmImporter
    {
        private readonly IComicsRepository _comicsRepository;
        private readonly IExplosmWebClient _explosmClient;

        public ExplosmImporter(IComicsRepository comicsRepository, IExplosmWebClient explosmClient)
        {
            _comicsRepository = comicsRepository;
            _explosmClient = explosmClient;
        }

        public void ImportNewComics()
        {
            var lastComic = _comicsRepository.GetLastImportedComic(ComicType.Explosm);

            var downloader = new ExplosmDownloader(_explosmClient);
            var results = downloader.GetNewComics(lastComic.ComicNumber);

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
