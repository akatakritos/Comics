using System;
using System.Collections.Generic;
using System.Linq;

using AutoMoq;

using Comics.Core.Downloaders;
using Comics.Core.Persistence;

using Moq;

using Xunit;

namespace Comics.Tests.Core.Downloaders
{
    public class ExplosmImporterTests
    {
        private readonly AutoMoqer _mocker;
        public ExplosmImporterTests()
        {
            var mocker = new AutoMoqer();

            mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLastImportedComic(ComicType.Explosm))
                .Returns(new Comic() { ComicNumber = 4124 });
            mocker.GetMock<IExplosmWebClient>()
                .Setup(m => m.GetComicHtml(4125))
                .Returns(new ComicDownloadResult(200, Fixture.Load("explosm-4125"), 4125, new Uri("http://www.google.com/")));

            mocker.GetMock<IExplosmWebClient>()
                .Setup(m => m.GetComicHtml(4126))
                .Returns(new ComicDownloadResult(404, "", 4126, new Uri("http://www.google.com/")));

            _mocker = mocker;
        }

        [Fact]
        public void GetsEachNewComicAndInsertsIt()
        {
            _mocker.Create<ExplosmImporter>().ImportNewComics();

            _mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(
                    It.Is<Comic>(c => c.ComicNumber == 4125)), Times.Once);
        }

        [Fact]
        public void InsertedComicContainsLink()
        {
            _mocker.Create<ExplosmImporter>().ImportNewComics();

            _mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(
                    It.Is<Comic>(c => c.ImageSrc == "http://files.explosm.net/comics/Kris/knowingis.png")), Times.Once);
        }

        [Fact]
        public void InsertedComicContainsPublishedDate()
        {
            _mocker.Create<ExplosmImporter>().ImportNewComics();

            _mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(
                    It.Is<Comic>(c => c.PublishedDate == new DateTime(2015,11,22))), Times.Once);
        }

        [Fact]
        public void InsertedComicContainsRightEnumValue()
        {
            _mocker.Create<ExplosmImporter>().ImportNewComics();

            _mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(
                    It.Is<Comic>(c => c.ComicType == ComicType.Explosm)), Times.Once);
        }

        [Fact]
        public void InsertedComicContainsThePermalink()
        {
            _mocker.Create<ExplosmImporter>().ImportNewComics();

            _mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(
                    It.Is<Comic>(c => c.Permalink == "http://www.google.com/")), Times.Once);
        }


    }
}
