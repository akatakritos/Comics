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
        public PearlsDownloaderTests()
        {
            var mocker = new AutoMoqer();
            var result20151204 = new ComicDownloadResult(
                Fixture.Load("pearls-2015-12-04"),
                20151204,
                new Uri("http://www.gocomics.com/pearlsbeforeswine/2015/12/04"));

            mocker.GetMock<IPearlsWebClient>()
                .Setup(m => m.GetComicHtml(new DateTime(2015, 12, 04)))
                .Returns(result20151204);

            _mocker = mocker;
        }

        [Fact]
        public void LoadsNewComicsUpToToday()
        {
            var downloader = _mocker.Create<PearlsDownloader>();
            downloader.Today = new DateTime(2015, 12, 4); // mock Today

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 12, 3) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.PublishedDate).IsEqualTo(new DateTime(2015, 12, 4));
        }

        [Fact]
        public void ItSkipsFailedComics() //like it if hasnt been posted yet
        {
            var mocker = new AutoMoqer();
            var result20151204 = new ComicDownloadResult(
                Fixture.Load("pearls-2015-12-04"),
                20151204,
                new Uri("http://www.gocomics.com/pearlsbeforeswine/2015/12/04")); ;

            mocker.GetMock<IPearlsWebClient>()
                .Setup(m => m.GetComicHtml(new DateTime(2015, 12, 4)))
                .Returns(result20151204);
            mocker.GetMock<IPearlsWebClient>()
                .Setup(m => m.GetComicHtml(new DateTime(2015, 12, 5)))
                .Throws(new ComicNotFoundException(new Uri("http://www.gocomics.com/pearlsbeforeswine/2015/12/05")));

            var downloader = mocker.Create<PearlsDownloader>();
            downloader.Today = new DateTime(2015, 12, 5);

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 12, 3) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.PublishedDate).IsEqualTo(new DateTime(2015, 12, 4));
        }

        [Fact]
        public void ItSetsTheComicTypeToPearls()
        {
            var downloader = _mocker.Create<PearlsDownloader>();
            downloader.Today = new DateTime(2015, 12, 4); // mock Today

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 12, 3) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ComicType).IsEqualTo(ComicType.Pearls);
        }

        [Fact]
        public void ItSetsTheComicNumberBasedOnTheDate()
        {
            var downloader = _mocker.Create<PearlsDownloader>();
            downloader.Today = new DateTime(2015, 12, 4); // mock Today

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 12, 3) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ComicNumber).IsEqualTo(20151204);
        }

        [Fact]
        public void ItSetsTheImageSrc()
        {
            var downloader = _mocker.Create<PearlsDownloader>();
            downloader.Today = new DateTime(2015, 12, 4); // mock Today

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 12, 3) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ImageSrc).IsEqualTo("http://assets.amuniversal.com/5a7e112071f701331e71005056a9545d");
        }

        [Fact]
        public void ItSetsThePermalink()
        {
            var downloader = _mocker.Create<PearlsDownloader>();
            downloader.Today = new DateTime(2015, 12, 4); // mock Today

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 12, 3) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.Permalink).IsEqualTo("http://www.gocomics.com/pearlsbeforeswine/2015/12/04");
        }

        [Fact]
        public void ItSetsThePublishedDate()
        {
            var downloader = _mocker.Create<PearlsDownloader>();
            downloader.Today = new DateTime(2015, 12, 4); // mock Today

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 12, 3) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.PublishedDate).IsEqualTo(new DateTime(2015, 12, 4));
        }
    }
}
