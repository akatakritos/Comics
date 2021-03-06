﻿using System;
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
    public class DilbertWebClientTests
    {
        public DilbertWebClientTests()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

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

            Check.That(result.Permalink).IsEqualTo(new Uri("https://dilbert.com/strip/2015-11-23"));
        }

        [Fact]
        public void HandlesRedirectAsNotFound()
        {
            var client = new DilbertWebClient();

            Check.ThatCode(() => client.GetComicHtml(new DateTime(2020, 1, 1)))
                .Throws<ComicNotFoundException>();
        }

        [Fact]
        public void DownloadedHtmlIsParseable()
        {
            var client = new DilbertWebClient();
            var result = client.GetComicHtml(new DateTime(2015, 11, 23));
            var parseResult = DilbertParser.Parse(result.Content);
            Check.That(parseResult.ImageUri).IsNotNull();
        }

    }
}
