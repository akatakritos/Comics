using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Downloaders;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Downloaders
{
    [Trait("Category", "Integration")]
    public class DilbertWebClientTests
    {
        [Fact]
        public void CanDownloadComicHtml()
        {
            var client = new DilbertWebClient();

            var result = client.GetComicHtml(new DateTime(2015, 11, 23));

            Check.That(result.Content).IsNotEmpty();
        }

        [Fact]
        public void SetsComicNumberToDate()
        {
            var client = new DilbertWebClient();

            var result = client.GetComicHtml(new DateTime(2015, 11, 23));

            Check.That(result.ComicNumber).IsEqualTo(20151123);
        }

        [Fact]
        public void SetsPermalink()
        {
            var client = new DilbertWebClient();

            var result = client.GetComicHtml(new DateTime(2015, 11, 23));

            Check.That(result.Permalink).IsEqualTo(new Uri("http://dilbert.com/strip/2015-11-23"));
        }


    }
}
