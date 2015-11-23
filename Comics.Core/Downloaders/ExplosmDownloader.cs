using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Comics.Core.Parsers;

namespace Comics.Core.Downloaders
{
    public interface IExplosmWebClient
    {
        ComicDownloadResult GetComicHtml(int comicNumber);
    }

    public struct ComicDownloadResult
    {
        public int StatusCode { get; }
        public string Content { get; }
        public bool NotFound => StatusCode == 404;

        public ComicDownloadResult(int statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }

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
