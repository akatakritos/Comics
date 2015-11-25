using System;
using System.Collections.Generic;
using System.Linq;

namespace Comics.Core.Downloaders
{


    public class DilbertDownloader
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

        public IEnumerable<ComicDownloadResult> GetNewComics(DateTime lastComicDate)
        {
            for (var current = lastComicDate.AddDays(1).Date; current <= Today; current = current.AddDays(1).Date)
            {
                ComicDownloadResult result;
                try
                {
                    result = _client.GetComicHtml(current);
                }
                catch (ComicNotFoundException)
                {
                    yield break;
                }

                yield return result;

            }
        }
    }
}
