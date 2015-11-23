using System;
using System.Collections.Generic;
using System.Linq;

using AutoMoq;

using Comics.Core.Downloaders;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Downloaders
{

    public class ExplosmDownloaderTests
    {
        [Fact]
        public void DownloaderGoesUntilIt404s()
        {
            var mocker = new AutoMoqer();
            var result1000 = new ComicDownloadResult(200, "<html />", 1000);
            var result1001 = new ComicDownloadResult(200, "<html />", 1001);

            mocker.GetMock<IExplosmWebClient>()
                .Setup(m => m.GetComicHtml(1000))
                .Returns(result1000);
            mocker.GetMock<IExplosmWebClient>()
                .Setup(m => m.GetComicHtml(1001))
                .Returns(result1001);
            mocker.GetMock<IExplosmWebClient>()
                .Setup(m => m.GetComicHtml(1002))
                .Returns(new ComicDownloadResult(404, "", 1002));

            var downloader = mocker.Create<ExplosmDownloader>();

            var comics = downloader.GetNewComics(999);

            Check.That(comics).ContainsExactly(result1000, result1001);

        }
    }
}
