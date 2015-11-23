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
        public void DownloadsWhileThereIsAValidNextLink()
        {
            var mocker = new AutoMoqer();
            // depends on 4125 fixture not having a next link
            var result4124 = new ComicDownloadResult(Fixture.Load("explosm-4124"), 4124, new Uri("http://explosm.net/comics/4124/"));
            var result4125 = new ComicDownloadResult(Fixture.Load("explosm-4125"), 4125, new Uri("http://explosm.net/comics/4125/"));

            mocker.GetMock<IExplosmWebClient>()
                .Setup(m => m.GetComicHtml(4124))
                .Returns(result4124);
            mocker.GetMock<IExplosmWebClient>()
                .Setup(m => m.GetComicHtml(4125))
                .Returns(result4125);

            var downloader = mocker.Create<ExplosmDownloader>();

            var comics = downloader.GetNewComics(4124);

            Check.That(comics).ContainsExactly(result4125);

        }
    }
}
