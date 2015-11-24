using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AutoMoq;

using Comics.Core.Downloaders;
using Comics.Core.Persistence;
using Comics.Web.Controllers;

using Moq;

using NFluent;

using Xunit;

namespace Comics.Tests.Web.Controllers
{
    public class DilbertControllerTests
    {
        [Fact]
        public void ItAlwaysChecksForNewComics()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLatestComics(ComicType.Dilbert, 10))
                .Returns(Enumerable.Empty<Comic>().ToList());

            mocker.Create<DilbertController>().Feed();

            mocker.GetMock<IDilbertImporter>()
                .Verify(m => m.ImportNewComics(null), Times.Once);
        }

        [Fact]
        public void ItSendsBackAnRssFeed()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLatestComics(ComicType.Dilbert, 10))
                .Returns(new[]
                {
                    new Comic()
                    {
                        ComicNumber = 1,
                        ComicType = ComicType.Dilbert,
                        ImageSrc = "http://example.com/image.png",
                        Permalink = "http://example.com",
                        ComicId = 1,
                        PublishedDate = DateTime.Today
                    }
                });

            var result = mocker.Create<DilbertController>().Feed();

            var s = new MemoryStream();
            result.FileStream.CopyTo(s);
            var xml = Encoding.UTF8.GetString(s.ToArray());

            Check.That(result.ContentType).IsEqualTo("text/xml");
            Check.That(xml).Contains("http://example.com/image.png");
        }

        [Fact]
        public void ItSearchesForDilbertComics()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLatestComics(ComicType.Dilbert, It.IsAny<int>()))
                .Returns(Enumerable.Empty<Comic>().ToList())
                .Verifiable();

            mocker.Create<DilbertController>().Feed();

            mocker.GetMock<IComicsRepository>().Verify();
        }

    }
}
