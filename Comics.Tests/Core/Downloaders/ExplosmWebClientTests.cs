using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Downloaders;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Downloaders
{
    [Trait("Category", "Integration")]
    public class ExplosmWebClientTests
    {
        [Fact]
        public void CanDownloadAComic()
        {
            var client = new ExplosmWebClient();

            var result = client.GetComicHtml(4125);

            Check.That(result.StatusCode).IsEqualTo(200);
            Check.That(result.Content).IsNotEmpty();
        }

        [Fact]
        public void HandlesNotFound()
        {
            var client = new ExplosmWebClient();

            var result = client.GetComicHtml(9999999);

            Check.That(result.StatusCode).IsEqualTo(404);
            Check.That(result.NotFound).IsTrue();
        }

    }
}
