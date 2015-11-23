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

            var response = client.GetAsync(new Uri($"http://explosm.net/comics/{comicNumber}/")).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new ComicDownloadResult((int)response.StatusCode, content, comicNumber);
        }
    }
}
