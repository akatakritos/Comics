using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using HtmlAgilityPack;

namespace Comics.Core.Parsers
{
    public static class PearlsParser
    {
        public static ComicParseResult Parse(string html)
        {
            if (html == null) throw new ArgumentNullException(nameof(html));
            if (html == "") throw new ArgumentException($"{nameof(html)} should contain some text", nameof(html));

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var img = doc.QuerySelectorAll("picture.item-comic-image > img")
                .FirstOrDefault();
            if (img == null)
                return ComicParseResult.Fail("Could not find strip url");

            var src = img.GetAttributeValue("src", null);
            if (src == null)
                return ComicParseResult.Fail("img element did not have src attribute");

            var meta = doc.QuerySelector("meta[name='twitter:url']");
            if (meta == null)
                return ComicParseResult.Fail("Could not load twitter:url attribtue");

            var content = meta.GetAttributeValue("content", null);
            if (content == null)
                return ComicParseResult.Fail("meta tag did not have content attribute");

            var dateString = content.Replace("https://www.gocomics.com/pearlsbeforeswine/", "");

            var date = DateTime.ParseExact(dateString, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var uri = new Uri(src);

            return ComicParseResult.Succeed(uri, date);


        }
    }
}
