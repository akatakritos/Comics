using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Comics.Core.Downloaders
{
    public interface IPearlsWebClient
    {
        ComicDownloadResult GetComicHtml(DateTime publishedDate);
    }

    public class PearlsWebClient : IPearlsWebClient
    {
        public ComicDownloadResult GetComicHtml(DateTime publishedDate)
        {
            var permalink = new Uri($"https://www.gocomics.com/pearlsbeforeswine/{publishedDate:yyyy/MM/dd}");
            Trace.WriteLine($"Downloading {permalink}", nameof(PearlsWebClient));

            using (var handler = new HttpClientHandler() { AllowAutoRedirect = false })
            using (var client = new HttpClient(handler))
            using (var response = client.GetAsync(permalink).Result)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Trace.WriteLine($"Non OK StatusCode {response.StatusCode}", nameof(PearlsWebClient));
                    throw new ComicNotFoundException(permalink);
                }

                var content = response.Content.ReadAsStringAsync().Result;

                if (!content.Contains(permalink.ToString()))
                {
                    Trace.WriteLine($"Couldn't find permalink meaning ucomics just returned the latest instead of 404 or redirect.", nameof(PearlsWebClient));
                    throw new ComicNotFoundException(permalink);
                }

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