using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

namespace Comics.Core.Parsers
{
    public static class DilbertParser
    {
        public static ComicParseResult Parse(string html)
        {
            if (html == null) throw new ArgumentNullException(nameof(html));
            if (html == "") throw new ArgumentException("html must have some contents", nameof(html));

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var img = doc.QuerySelector(".img-comic");
            var src = img?.GetAttributeValue("src", null);

            var meta = doc.QuerySelector("meta[property='article:publish_date']");
            var date = meta?.GetAttributeValue("content", null);

            if (src == null)
                return ComicParseResult.Fail("Could not find image src");
            if (date == null)
                return ComicParseResult.Fail("Could not find publish date");

            if (src.StartsWith("//"))
                src = "https:" + src;

            return ComicParseResult.Succeed(new Uri(src), DateTime.Parse(date));
        }
    }
}
