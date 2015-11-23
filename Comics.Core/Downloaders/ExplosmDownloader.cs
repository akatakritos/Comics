using System;
using System.Collections.Generic;
using System.Linq;

namespace Comics.Core.Downloaders
{
    public interface IExplosmWebClient
    {
        ComicDownloadResult GetComicHtml(int comicNumber);
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExplosmDownloader
    {
        private readonly IExplosmWebClient _client;

        public ExplosmDownloader(IExplosmWebClient client)
        {
            _client = client;
        }

        public IEnumerable<ComicDownloadResult> GetNewComics(int lastComic)
        {
            ComicDownloadResult result;

            do
            {
                lastComic++;
                result = _client.GetComicHtml(lastComic);

                if (!result.NotFound)
                    yield return result;

            } while (!result.NotFound);
        }
    }
}
