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
    public class DilbertImporterTests
    {
        private readonly AutoMoqer _mocker;

        public DilbertImporterTests()
        {
            _mocker = new AutoMoqer();
            _mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLastImportedComic(ComicType.Dilbert))
                .Returns(new Comic() { PublishedDate = new DateTime(2015, 11, 22) });
            _mocker.GetMock<IDilbertWebClient>()
                .Setup(m => m.GetComicHtml(new DateTime(2015, 11, 23)))
                .Returns(new ComicDownloadResult(Fixture.Load("dilbert-2015-11-23"), 20151123, new Uri("http://dilbert.com/strips/2015-11-23")));

        }
        [Fact]
        public void GetsEachNewComicAndInsertsIt()
        {
            var downloader = _mocker.Create<DilbertImporter>();
            downloader.Today = new DateTime(2015, 11, 23);

            downloader.ImportNewComics();

            _mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(
                    It.Is<Comic>( c => c.PublishedDate == new DateTime(2015, 11, 23))), Times.Once);
        }

        [Fact]
        public void ImportNewComics_InsertsComicWithLink()
        {
            var downloader = _mocker.Create<DilbertImporter>();
            downloader.Today = new DateTime(2015, 11, 23);

            downloader.ImportNewComics();

            _mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(
                    It.Is<Comic>(c => c.Permalink == "http://dilbert.com/strips/2015-11-23")), Times.Once);
        }

        [Fact]
        public void ImportNewComics_InsertsComicWithRightEnumValue()
        {
            var downloader = _mocker.Create<DilbertImporter>();
            downloader.Today = new DateTime(2015, 11, 23);

            downloader.ImportNewComics();

            _mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(
                    It.Is<Comic>(c => c.ComicType == ComicType.Dilbert)), Times.Once);
        }

        [Fact]
        public void ImportNewComics_InsertsComicWithImageSrc()
        {
            var downloader = _mocker.Create<DilbertImporter>();
            downloader.Today = new DateTime(2015, 11, 23);

            downloader.ImportNewComics();

            _mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(
                    It.Is<Comic>(c => c.ImageSrc == "http://assets.amuniversal.com/041159a06560013319a6005056a9545d")), Times.Once);
        }

        [Fact]
        public void ImportNewComics_InsertsComicWithComicNumber()
        {
            var downloader = _mocker.Create<DilbertImporter>();
            downloader.Today = new DateTime(2015, 11, 23);

            downloader.ImportNewComics();

            _mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(
                    It.Is<Comic>(c => c.ComicNumber == 20151123)), Times.Once);
        }
    }
}
