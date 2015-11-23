using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Comics.Core.Downloaders
{
    public class ExplosmWebClient : IExplosmWebClient
    {
        public ComicDownloadResult GetComicHtml(int comicNumber)
        {
            var client = new HttpClient();

            var permalink = new Uri($"http://explosm.net/comics/{comicNumber}/");
            var response = client.GetAsync(permalink).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new ComicDownloadResult((int)response.StatusCode, content, comicNumber, permalink);
        }
    }
}
