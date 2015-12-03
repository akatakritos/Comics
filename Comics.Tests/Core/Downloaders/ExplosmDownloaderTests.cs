using System;
using System.Collections.Generic;
using System.Linq;

using AutoMoq;

using Comics.Core.Downloaders;
using Comics.Core.Persistence;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Downloaders
{

    public class ExplosmDownloaderTests
    {
        private readonly AutoMoqer _mocker;

        public ExplosmDownloaderTests()
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

            _mocker = mocker;
        }

        [Fact]
        public void DownloadsWhileThereIsAValidNextLink()
        {
            var downloader = _mocker.Create<ExplosmDownloader>();

            var lastComic = new Comic { ComicNumber = 4124 };
            var comics = downloader.GetNewComicsSince(lastComic);

            Check.That(comics.Single().ComicNumber).IsEqualTo(4125);
        }

        [Fact]
        public void ItSetsTheComicTypeToExplosm()
        {
            var downloader = _mocker.Create<ExplosmDownloader>();

            var lastComic = new Comic { ComicNumber = 4124 };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ComicType).IsEqualTo(ComicType.Explosm);
        }

        [Fact]
        public void ItSetsTheComicNumberToTheDownloadedNumber()
        {
            var downloader = _mocker.Create<ExplosmDownloader>();

            var lastComic = new Comic { ComicNumber = 4124 };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ComicNumber).IsEqualTo(4125);
        }

        [Fact]
        public void ItSetsTheImageSrc()
        {
            var downloader = _mocker.Create<ExplosmDownloader>();

            var lastComic = new Comic { ComicNumber = 4124 };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ImageSrc).IsEqualTo("http://files.explosm.net/comics/Kris/knowingis.png");
        }

        [Fact]
        public void ItSetsThePermalink()
        {
            var downloader = _mocker.Create<ExplosmDownloader>();

            var lastComic = new Comic { ComicNumber = 4124 };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.Permalink).IsEqualTo("http://explosm.net/comics/4125/");
        }

        [Fact]
        public void ItSetsThePublishedDate()
        {
            var downloader = _mocker.Create<ExplosmDownloader>();

            var lastComic = new Comic { ComicNumber = 4124 };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.PublishedDate).IsEqualTo(new DateTime(2015, 11, 22));
        }
    }
}
