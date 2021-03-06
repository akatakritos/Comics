﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Comics.Core.Parsers;
using Comics.Core.Persistence;

namespace Comics.Core.Downloaders
{
    public class PearlsDownloader : IComicDownloader
    {
        private readonly IPearlsWebClient _client;

        //for unit test
        public DateTime Today { get; set; } = DateTime.Today;

        public readonly DateTime DefaultStartDate = new DateTime(2015, 12, 1);

        public PearlsDownloader(IPearlsWebClient client)
        {
            _client = client;
        }

        public IEnumerable<Comic> GetNewComicsSince(Comic lastDownloaded)
        {
            var lastComicDate = lastDownloaded?.PublishedDate ?? DefaultStartDate;
            Trace.WriteLine($"Getting new comics since {lastComicDate}", nameof(PearlsDownloader));

            for (var current = lastComicDate.AddDays(1).Date; current <= Today; current = current.AddDays(1).Date)
            {
                ComicDownloadResult result;
                try
                {
                    result = _client.GetComicHtml(current);
                }
                catch (ComicNotFoundException e)
                {
                    Trace.WriteLine(e.Message, nameof(PearlsDownloader));
                    yield break;
                }

                var parseResult = PearlsParser.Parse(result.Content);
                var comic = new Comic()
                {
                    ComicType = ComicType.Pearls,
                    PublishedDate = current,
                    ComicNumber = result.ComicNumber,
                    ImageSrc = parseResult.ImageUri.ToString(),
                    Permalink = result.Permalink.ToString(),
                };

                yield return comic;

                Trace.WriteLine("Finished downloading comics", nameof(PearlsDownloader));

            }
        }
    }
}
