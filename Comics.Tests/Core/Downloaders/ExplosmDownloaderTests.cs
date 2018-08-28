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

        const int SecondMostRecentNumber = 5021;
        const int MostRecentNumber = 5022;

        public ExplosmDownloaderTests()
        {

            var mocker = new AutoMoqer();
            // depends on explosm-latest fixture not having a next link
            var secondMostRecentResult = new ComicDownloadResult(Fixture.Load($"explosm-latest-minus-one"), SecondMostRecentNumber, new Uri($"http://explosm.net/comics/{SecondMostRecentNumber}/"));
            var mostRecentResult = new ComicDownloadResult(Fixture.Load("explosm-latest"), MostRecentNumber, new Uri($"http://explosm.net/comics/{MostRecentNumber}/"));

            mocker.GetMock<IExplosmWebClient>()
                .Setup(m => m.GetComicHtml(SecondMostRecentNumber))
                .Returns(secondMostRecentResult);
            mocker.GetMock<IExplosmWebClient>()
                .Setup(m => m.GetComicHtml(MostRecentNumber))
                .Returns(mostRecentResult);

            _mocker = mocker;
        }

        [Fact]
        public void DownloadsWhileThereIsAValidNextLink()
        {
            var downloader = _mocker.Create<ExplosmDownloader>();

            var lastComic = new Comic { ComicNumber = SecondMostRecentNumber };
            var comics = downloader.GetNewComicsSince(lastComic);

            Check.That(comics.Single().ComicNumber).IsEqualTo(MostRecentNumber);
        }

        [Fact]
        public void ItSetsTheComicTypeToExplosm()
        {
            var downloader = _mocker.Create<ExplosmDownloader>();

            var lastComic = new Comic { ComicNumber = SecondMostRecentNumber };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ComicType).IsEqualTo(ComicType.Explosm);
        }

        [Fact]
        public void ItSetsTheComicNumberToTheDownloadedNumber()
        {
            var downloader = _mocker.Create<ExplosmDownloader>();

            var lastComic = new Comic { ComicNumber = SecondMostRecentNumber };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ComicNumber).IsEqualTo(MostRecentNumber);
        }

        [Fact]
        public void ItSetsTheImageSrc()
        {
            var downloader = _mocker.Create<ExplosmDownloader>();

            var lastComic = new Comic { ComicNumber = SecondMostRecentNumber };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ImageSrc).IsEqualTo("http://files.explosm.net/comics/Rob/thatgirl3.png?t=5CE637");
        }

        [Fact]
        public void ItSetsThePermalink()
        {
            var downloader = _mocker.Create<ExplosmDownloader>();

            var lastComic = new Comic { ComicNumber = SecondMostRecentNumber };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.Permalink).IsEqualTo($"http://explosm.net/comics/{MostRecentNumber}/");
        }

        [Fact]
        public void ItSetsThePublishedDate()
        {
            var downloader = _mocker.Create<ExplosmDownloader>();

            var lastComic = new Comic { ComicNumber = SecondMostRecentNumber };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.PublishedDate).IsEqualTo(new DateTime(2018, 8, 27));
        }
    }
}
