using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Comics.Core.Parsers;
using Comics.Core.Persistence;

using HtmlAgilityPack;

namespace Comics.Core.Downloaders
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExplosmDownloader : IComicDownloader
    {
        private readonly IExplosmWebClient _client;

        public ExplosmDownloader(IExplosmWebClient client)
        {
            _client = client;
        }

        public const int DefaultStartComic = 4124;
        public IEnumerable<Comic> GetNewComicsSince(Comic lastDownloaded)
        {
            var lastComic = lastDownloaded?.ComicNumber ?? DefaultStartComic;
            var result = _client.GetComicHtml(lastComic);
            var next = ParseNextComicNumber(result.Content);

            while(next.HasValue)
            {
                lastComic = next.Value;

                result = _client.GetComicHtml(lastComic);
                var parseResult = ExplosmParser.Parse(result.Content);

                var comic = new Comic()
                {
                    ComicType = ComicType.Explosm,
                    ComicNumber = lastComic,
                    ImageSrc = parseResult.ImageUri.ToString(),
                    Permalink = result.Permalink.ToString(),
                    PublishedDate = parseResult.PublishedDate
                };
                yield return comic;

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
