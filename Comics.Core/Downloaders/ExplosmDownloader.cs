using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using HtmlAgilityPack;

namespace Comics.Core.Downloaders
{


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
            var result = _client.GetComicHtml(lastComic);
            var next = ParseNextComicNumber(result.Content);

            while(next.HasValue)
            {
                lastComic = next.Value;
                result = _client.GetComicHtml(lastComic);
                yield return result;

                next = ParseNextComicNumber(result.Content);
            }
        }

        private static int? ParseNextComicNumber(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var nextLink = doc.QuerySelector("a.next-comic");

            var nextUrl = nextLink?.GetAttributeValue("href", null);
            if (nextUrl == null)
                return null;

            var match = Regex.Match(nextUrl, @"/(\d+)/?$");
            return int.Parse(match.Groups[1].Value);

        }
    }
}
