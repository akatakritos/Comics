using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AutoMoq;

using Comics.Core.Persistence;
using Comics.Web.Controllers;

using Moq;

using NFluent;

using Xunit;

namespace Comics.Tests.Web.Controllers
{
    public class FeedControllerTests
    {
        [Theory]
        [InlineData(ComicType.Dilbert)]
        [InlineData(ComicType.Explosm)]
        public void ItSearchesForTheReferencedComic(ComicType type)
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLatestComics(type, It.IsAny<int>()))
                .Returns(Enumerable.Empty<Comic>().ToList())
                .Verifiable();

            mocker.Create<FeedController>().Feed(type);

            mocker.GetMock<IComicsRepository>().Verify();
        }

        [Theory]
        [InlineData(ComicType.Dilbert)]
        [InlineData(ComicType.Explosm)]
        public void ItSendsBackSomeRss(ComicType type)
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLatestComics(type, 10))
                .Returns(new[]
                {
                    new Comic()
                    {
                        ComicNumber = 1,
                        ComicType = type,
                        ImageSrc = "http://example.com/image.png",
                        Permalink = "http://example.com",
                        ComicId = 1,
                        PublishedDate = DateTime.Today
                    }
                });

            var result = mocker.Create<FeedController>().Feed(type);

            var s = new MemoryStream();
            result.FileStream.CopyTo(s);
            var xml = Encoding.UTF8.GetString(s.ToArray());

            Check.That(result.ContentType).IsEqualTo("text/xml");
            Check.That(xml).Contains("http://example.com/image.png");
        }
    }
}
