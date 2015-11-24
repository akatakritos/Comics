using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Comics.Core.Downloaders
{
    public interface IDilbertWebClient
    {
        ComicDownloadResult GetComicHtml(DateTime publishedDate);
    }

    public class DilbertWebClient : IDilbertWebClient
    {
        public ComicDownloadResult GetComicHtml(DateTime publishedDate)
        {
            var client = new HttpClient();
            var permalink = new Uri($"http://dilbert.com/strip/{publishedDate:yyyy-MM-dd}");
            using (var response = client.GetAsync(permalink).Result)
            {
                var content = response.Content.ReadAsStringAsync().Result;

                return new ComicDownloadResult(content, ToComicNumber(publishedDate), permalink);
            }
        }

        public static int ToComicNumber(DateTime publishedDate)
        {
            return publishedDate.Year * 10000 +
                   publishedDate.Month * 100 +
                   publishedDate.Day;
        }
    }
}