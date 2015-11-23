﻿using System;
using System.Globalization;

using HtmlAgilityPack;

namespace Comics.Core.Parsers
{
    public static class ExplosmParser
    {
        public static ComicParseResult Parse(string html)
        {
            if (html == null)
                throw new ArgumentNullException(nameof(html));

            if (string.IsNullOrEmpty(html))
                throw new ArgumentException("html is empty", nameof(html));

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var image = doc.GetElementbyId("main-comic");
            if (image == null)
            {
                return ComicParseResult.Fail("Could not find comic img element");
            }


            var dateNode = doc.QuerySelector(".past-week-comic-title");
            if (dateNode == null)
            {
                return ComicParseResult.Fail("Could not find date elemenet");
            }

            DateTime date;
            if (!DateTime.TryParseExact(dateNode.InnerText, "yyyy.MM.dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return ComicParseResult.Fail($"Could not parse date: '{dateNode.InnerText}'");
            }


            var src = new Uri(image.Attributes["src"].Value);

            return ComicParseResult.Succeed(AdjustSchemeToHttp(src), date);

        }

        private static Uri AdjustSchemeToHttp(Uri src)
        {
            var builder = new UriBuilder(src);
            builder.Scheme = "http";
            return builder.Uri;
        }

        public static string ParsePermalink(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var textbox = doc.GetElementbyId("permalink");
            var urlOfCurrent = textbox.GetAttributeValue("value", null);

            return urlOfCurrent;
        }
    }
}
