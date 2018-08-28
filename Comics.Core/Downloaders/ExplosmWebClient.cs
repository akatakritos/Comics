using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Comics.Core.Downloaders
{
    public interface IExplosmWebClient
    {
        ComicDownloadResult GetComicHtml(int comicNumber);
    }

    [Serializable]
    public class ComicNotFoundException : Exception
    {
        public ComicNotFoundException(Uri failedUri) :
            base($"Comic not found at {failedUri}")
        {
        }
    }

    public class ExplosmWebClient : IExplosmWebClient
    {
        public ComicDownloadResult GetComicHtml(int comicNumber)
        {
            var client = new HttpClient();

            var permalink = new Uri($"http://explosm.net/comics/{comicNumber}/");

            Trace.WriteLine($"Downloading {permalink}", nameof(ExplosmWebClient));
            using (var response = client.GetAsync(permalink).Result)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Trace.WriteLine($"Received 404", nameof(ExplosmWebClient));
                    throw new ComicNotFoundException(permalink);
                }

                var content = response.Content.ReadAsStringAsync().Result;

                return new ComicDownloadResult(content, comicNumber, permalink);
            }
        }
    }
}
