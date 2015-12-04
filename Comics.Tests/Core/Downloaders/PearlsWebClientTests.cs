using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Comics.Core.Downloaders;
using Comics.Core.Parsers;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Downloaders
{
    [Trait("Category", "Integration")]
    public class PearlsWebClientTests
    {
        [Fact]
        public void CanDownloadComicHtml()
        {
            var client = new PearlsWebClient();

            var result = client.GetComicHtml(new DateTime(2015, 12, 4));

            Check.That(result.Content).IsNotEmpty();
        }

        [Fact]
        public void SetsComicNumberToDate()
        {
            var client = new PearlsWebClient();

            var result = client.GetComicHtml(new DateTime(2015, 12, 4));

            Check.That(result.ComicNumber).IsEqualTo(20151204);
        }

        [Fact]
        public void SetsPermalink()
        {
            var client = new PearlsWebClient();

            var result = client.GetComicHtml(new DateTime(2015, 12, 4));

            Check.That(result.Permalink).IsEqualTo(new Uri("http://www.gocomics.com/pearlsbeforeswine/2015/12/04"));
        }

        [Fact]
        public void HandlesWrongDatesAsNotFound()
        {
            var client = new PearlsWebClient();

            Check.ThatCode(() => client.GetComicHtml(new DateTime(2020, 1, 1)))
                .Throws<ComicNotFoundException>();
        }

        [Fact]
        public void DownloadedHtmlIsParseable()
        {
            var client = new PearlsWebClient();
            var result = client.GetComicHtml(new DateTime(2015, 12, 4));
            var parseResult = PearlsParser.Parse(result.Content);
            Check.That(parseResult.ImageUri).IsNotNull();
        }
    }
}
