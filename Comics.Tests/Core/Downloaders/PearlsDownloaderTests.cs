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
    public class PearlsDownloaderTests
    {
        private readonly AutoMoqer _mocker;
        private readonly DateTime Today = new DateTime(2017, 01, 13);
        private const string TodayFixture = "pearls-2017-01-13";
        private const int TodayComicNumber = 20170113;
        private const string TodayUrl = "http://www.gocomics.com/pearlsbeforeswine/2017/01/13";
        public PearlsDownloaderTests()
        {
            var mocker = new AutoMoqer();
            var result20151204 = new ComicDownloadResult(
                Fixture.Load(TodayFixture),
                TodayComicNumber,
                new Uri(TodayUrl));

            mocker.GetMock<IPearlsWebClient>()
                .Setup(m => m.GetComicHtml(Today))
                .Returns(result20151204);

            _mocker = mocker;
        }

        [Fact]
        public void LoadsNewComicsUpToToday()
        {
            var downloader = _mocker.Create<PearlsDownloader>();
            downloader.Today = Today;

            var lastComic = new Comic() { PublishedDate = Today.AddDays(-1) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.PublishedDate).IsEqualTo(Today);
        }

        [Fact]
        public void ItSkipsFailedComics() //like it if hasnt been posted yet
        {
            var mocker = new AutoMoqer();
            var todayResult = new ComicDownloadResult(
                Fixture.Load(TodayFixture),
                TodayComicNumber,
                new Uri(TodayUrl)); ;

            var tomorrow = Today.AddDays(1);

            mocker.GetMock<IPearlsWebClient>()
                .Setup(m => m.GetComicHtml(Today))
                .Returns(todayResult);

            mocker.GetMock<IPearlsWebClient>()
                .Setup(m => m.GetComicHtml(tomorrow))
                .Throws(new ComicNotFoundException(new Uri("http://www.gocomics.com/pearlsbeforeswine/" + Today.AddDays(1).ToString("yyyy/MM/dd"))));

            var downloader = mocker.Create<PearlsDownloader>();
            downloader.Today = tomorrow;

            var lastComic = new Comic() { PublishedDate = Today.AddDays(-1) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.PublishedDate).IsEqualTo(Today);
        }

        [Fact]
        public void ItSetsTheComicTypeToPearls()
        {
            var downloader = _mocker.Create<PearlsDownloader>();
            downloader.Today = Today;

            var lastComic = new Comic() { PublishedDate = Today.AddDays(-1) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ComicType).IsEqualTo(ComicType.Pearls);
        }

        [Fact]
        public void ItSetsTheComicNumberBasedOnTheDate()
        {
            var downloader = _mocker.Create<PearlsDownloader>();
            downloader.Today = Today;

            var lastComic = new Comic() { PublishedDate = Today.AddDays(-1) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ComicNumber).IsEqualTo(TodayComicNumber);
        }

        [Fact]
        public void ItSetsTheImageSrc()
        {
            var downloader = _mocker.Create<PearlsDownloader>();
            downloader.Today = Today;

            var lastComic = new Comic() { PublishedDate = Today.AddDays(-1) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ImageSrc).IsEqualTo("http://assets.amuniversal.com/3a70d0d0b3fa013428f8005056a9545d");
        }

        [Fact]
        public void ItSetsThePermalink()
        {
            var downloader = _mocker.Create<PearlsDownloader>();
            downloader.Today = Today;

            var lastComic = new Comic() { PublishedDate = Today.AddDays(-1) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.Permalink).IsEqualTo(TodayUrl);
        }

        [Fact]
        public void ItSetsThePublishedDate()
        {
            var downloader = _mocker.Create<PearlsDownloader>();
            downloader.Today = Today;

            var lastComic = new Comic() { PublishedDate = Today.AddDays(-1) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.PublishedDate).IsEqualTo(Today);
        }
    }
}
