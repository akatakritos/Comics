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
    public  class DilbertDownloaderTests
    {
        private readonly AutoMoqer _mocker;
        public DilbertDownloaderTests()
        {
            var mocker = new AutoMoqer();
            var result20151123 = new ComicDownloadResult(
                Fixture.Load("dilbert-2015-11-23"),
                20151123,
                new Uri("http://dilbert.com/strip/2015-11-23"));

            mocker.GetMock<IDilbertWebClient>()
                .Setup(m => m.GetComicHtml(new DateTime(2015, 11, 23)))
                .Returns(result20151123);

            _mocker = mocker;
        }

        [Fact]
        public void LoadsNewComicsUpToToday()
        {
            var downloader = _mocker.Create<DilbertDownloader>();
            downloader.Today = new DateTime(2015, 11, 23); // mock Today

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 11, 22) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.PublishedDate).IsEqualTo(new DateTime(2015, 11, 23));
        }

        [Fact]
        public void ItSkipsFailedComics() //like it if hasnt been posted yet
        {
            var mocker = new AutoMoqer();
            var result20151123 = new ComicDownloadResult(
                Fixture.Load("dilbert-2015-11-23"),
                20151123,
                new Uri("http://dilbert.com/strip/2015-11-23"));

            mocker.GetMock<IDilbertWebClient>()
                .Setup(m => m.GetComicHtml(new DateTime(2015, 11, 23)))
                .Returns(result20151123);
            mocker.GetMock<IDilbertWebClient>()
                .Setup(m => m.GetComicHtml(new DateTime(2015, 11, 24)))
                .Throws(new ComicNotFoundException(new Uri("http://dilbert.com/strip/2015-11-24")));

            var downloader = mocker.Create<DilbertDownloader>();
            downloader.Today = new DateTime(2015,11,24);

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 11, 22) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.PublishedDate).IsEqualTo(new DateTime(2015, 11, 23));
        }

        [Fact]
        public void ItSetsTheComicTypeToDilbert()
        {
            var downloader = _mocker.Create<DilbertDownloader>();
            downloader.Today = new DateTime(2015, 11, 23);

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 11, 22) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ComicType).IsEqualTo(ComicType.Dilbert);
        }

        [Fact]
        public void ItSetsTheComicNumberBasedOnTheDate()
        {
            var downloader = _mocker.Create<DilbertDownloader>();
            downloader.Today = new DateTime(2015, 11, 23);

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 11, 22) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ComicNumber).IsEqualTo(20151123);
        }

        [Fact]
        public void ItSetsTheImageSrc()
        {
            var downloader = _mocker.Create<DilbertDownloader>();
            downloader.Today = new DateTime(2015, 11, 23);

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 11, 22) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.ImageSrc).IsEqualTo("http://assets.amuniversal.com/041159a06560013319a6005056a9545d");
        }

        [Fact]
        public void ItSetsThePermalink()
        {
            var downloader = _mocker.Create<DilbertDownloader>();
            downloader.Today = new DateTime(2015, 11, 23);

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 11, 22) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.Permalink).IsEqualTo("http://dilbert.com/strip/2015-11-23");
        }

        [Fact]
        public void ItSetsThePublishedDate()
        {
            var downloader = _mocker.Create<DilbertDownloader>();
            downloader.Today = new DateTime(2015, 11, 23);

            var lastComic = new Comic() { PublishedDate = new DateTime(2015, 11, 22) };
            var comic = downloader.GetNewComicsSince(lastComic).Single();

            Check.That(comic.PublishedDate).IsEqualTo(new DateTime(2015, 11, 23));
        }
    }
}
