using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Comics.Core.Parsers;
using Comics.Core.Persistence;

namespace Comics.Core.Downloaders
{
    public class DilbertDownloader : IComicDownloader
    {
        private readonly IDilbertWebClient _client;

        public DilbertDownloader(IDilbertWebClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Mock property
        /// </summary>
        public DateTime Today { get; set; } = DateTime.Today;

        public readonly DateTime DefaultStartDate = new DateTime(2015, 11, 20);

        public IEnumerable<Comic> GetNewComicsSince(Comic lastDownloaded)
        {
            var lastComicDate = lastDownloaded?.PublishedDate ?? DefaultStartDate;
            Trace.WriteLine($"Downloading new comics since {lastComicDate}", nameof(DilbertDownloader));

            for (var current = lastComicDate.AddDays(1).Date; current <= Today; current = current.AddDays(1).Date)
            {
                ComicDownloadResult result;
                try
                {
                    result = _client.GetComicHtml(current);
                }
                catch (ComicNotFoundException e)
                {
                    Trace.WriteLine(e.Message, nameof(DilbertDownloader));
                    yield break;
                }

                var parseResult = DilbertParser.Parse(result.Content);
                var comic = new Comic()
                {
                    ComicType = ComicType.Dilbert,
                    PublishedDate = current,
                    ComicNumber = result.ComicNumber,
                    ImageSrc = parseResult.ImageUri.ToString(),
                    Permalink = result.Permalink.ToString(),
                };

                yield return comic;

                Trace.WriteLine("Finished downloading comics", nameof(DilbertDownloader));

            }
        }
    }
}
