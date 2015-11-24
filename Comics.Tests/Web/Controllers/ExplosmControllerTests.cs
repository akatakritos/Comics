using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AutoMoq;

using Comics.Core.Persistence;
using Comics.Web.Controllers;

using NFluent;

using Xunit;

namespace Comics.Tests.Web.Controllers
{
    public class ExplosmControllerTests
    {
        [Fact]
        public void ItSendsBackAnRssFeed()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLatestComics(ComicType.Explosm, 10))
                .Returns(new[]
                {
                    new Comic()
                    {
                        ComicNumber = 1,
                        ComicType = ComicType.Explosm,
                        ImageSrc = "http://example.com/image.png",
                        Permalink = "http://example.com",
                        ComicId = 1,
                        PublishedDate = DateTime.Today
                    }
                });

            var result = mocker.Create<ExplosmController>().Feed();

            var s = new MemoryStream();
            result.FileStream.CopyTo(s);
            var xml = Encoding.UTF8.GetString(s.ToArray());

            Check.That(result.ContentType).IsEqualTo("text/xml");
            Check.That(xml).Contains("http://example.com/image.png");
        }
    }
}
