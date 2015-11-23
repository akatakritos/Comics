using System;
using System.Collections.Generic;
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
            using (var response = client.GetAsync(permalink).Result)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new ComicNotFoundException(permalink);

                var content = response.Content.ReadAsStringAsync().Result;

                return new ComicDownloadResult(content, comicNumber, permalink);
            }
        }
    }
}
