using System;
using System.Collections.Generic;
using System.Linq;

using AutoMoq;

using Comics.Core.Downloaders;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Downloaders
{
    public  class DilbertDownloaderTests
    {
        [Fact]
        public void Something()
        {
            var mocker = new AutoMoqer();
            var result20151123 = new ComicDownloadResult(
                Fixture.Load("dilbert-2015-11-23"),
                20151123,
                new Uri("http://dilbert.com/strip/2015-11-23"));

            mocker.GetMock<IDilbertWebClient>()
                .Setup(m => m.GetComicHtml(new DateTime(2015, 11, 23)))
                .Returns(result20151123);

            var downloader = mocker.Create<DilbertDownloader>();
            downloader.Today = new DateTime(2015, 11, 23); // mock Today

            var newComics = downloader.GetNewComics(new DateTime(2015, 11, 22));

            Check.That(newComics).ContainsExactly(result20151123);
        }
    }
}
