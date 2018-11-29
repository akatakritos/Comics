using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Comics.Core.Downloaders;
using Comics.Core.Parsers;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Downloaders
{
    [Trait("Category", "Integration")]
    public class ExplosmWebClientTests
    {
        public ExplosmWebClientTests()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        [Fact]
        public void CanDownloadAComic()
        {
            var client = new ExplosmWebClient();

            var result = client.GetComicHtml(4125);

            Check.That(result.Content).IsNotEmpty();
        }

        [Fact]
        public void HandlesNotFound()
        {
            var client = new ExplosmWebClient();

            Check.ThatCode(() => client.GetComicHtml(999999)).Throws<ComicNotFoundException>();
        }

        [Fact]
        public void DownloadedHtmlIsParseable()
        {
            var client = new ExplosmWebClient();
            var result = client.GetComicHtml(4125);
            var parseResult = ExplosmParser.Parse(result.Content);

            Check.That(parseResult.ImageUri).IsNotNull();
        }

    }
}
