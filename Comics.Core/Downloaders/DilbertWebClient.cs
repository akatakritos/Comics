using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var permalink = new Uri($"https://dilbert.com/strip/{publishedDate:yyyy-MM-dd}");
            Trace.WriteLine($"Downloading {permalink}", nameof(DilbertWebClient));

            using (var handler = new HttpClientHandler() { AllowAutoRedirect = false })
            using (var client = new HttpClient(handler))
            using (var response = client.GetAsync(permalink).Result)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Trace.WriteLine($"Non-OK Response Code: {response.StatusCode}", nameof(DilbertWebClient));
                    throw new ComicNotFoundException(permalink);
                }

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