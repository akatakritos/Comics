﻿using System;
using System.Collections.Generic;
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
            var permalink = new Uri($"http://www.gocomics.com/pearlsbeforeswine/{publishedDate:yyyy/MM/dd}");
            using (var handler = new HttpClientHandler() { AllowAutoRedirect = false })
            using (var client = new HttpClient(handler))
            using (var response = client.GetAsync(permalink).Result)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new ComicNotFoundException(permalink);

                var content = response.Content.ReadAsStringAsync().Result;

                if (!content.Contains(permalink.ToString()))
                    throw new ComicNotFoundException(permalink);

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