using AutoMoq;

using Comics.Core.Downloaders;
using Comics.Core.Import;
using Comics.Core.Persistence;

using Moq;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Import
{
    public class ImportProcessTests
    {


        [Fact]
        public void UsesRegistryToGetNewComics()
        {
            var mocker = new AutoMoqer();

            var lastExplosmComic = new Comic();
            var newExplosmComic1 = new Comic();
            var newExplosmComic2 = new Comic();

            var explosm = new Mock<IComicDownloader>();
            explosm.Setup(m => m.GetNewComicsSince(lastExplosmComic))
                .Returns(new[] { newExplosmComic1, newExplosmComic2 });

            mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLastImportedComic(ComicType.Explosm))
                .Returns(lastExplosmComic)
                .Verifiable();

            var registry = new ComicConfigRegistry();
            registry.Add(new ComicConfig(ComicType.Explosm, explosm.Object));

            // ReSharper disable once RedundantTypeArgumentsOfMethod
            mocker.SetInstance<ComicConfigRegistry>(registry);

            mocker.Create<ImportProcess>()
                .Run();

            mocker.GetMock<IComicsRepository>().VerifyAll();
            mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(newExplosmComic1), Times.Once);
            mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(newExplosmComic2), Times.Once);
        }

        [Fact]
        public void ReportsComicsImported()
        {
            var mocker = new AutoMoqer();

            var lastExplosmComic = new Comic();
            var newExplosmComic1 = new Comic();
            var newExplosmComic2 = new Comic();

            var explosm = new Mock<IComicDownloader>();
            explosm.Setup(m => m.GetNewComicsSince(lastExplosmComic))
                .Returns(new[] { newExplosmComic1, newExplosmComic2 });

            mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLastImportedComic(ComicType.Explosm))
                .Returns(lastExplosmComic)
                .Verifiable();

            var registry = new ComicConfigRegistry();
            registry.Add(new ComicConfig(ComicType.Explosm, explosm.Object));

            // ReSharper disable once RedundantTypeArgumentsOfMethod
            mocker.SetInstance<ComicConfigRegistry>(registry);

            var process = mocker.Create<ImportProcess>();
            process.Run();

            Check.That(process.ImportedComics.Count).IsEqualTo(2);
        }

        [Fact]
        public void UsesRegistryToGetNewComicsFromMultipleSources()
        {
            var mocker = new AutoMoqer();

            var lastExplosmComic = new Comic();
            var newExplosmComic1 = new Comic();
            var newExplosmComic2 = new Comic();

            var lastDilbert = new Comic();
            var newDilbert1 = new Comic();
            var newDilbert2 = new Comic();

            var explosm = new Mock<IComicDownloader>();
            explosm.Setup(m => m.GetNewComicsSince(lastExplosmComic))
                .Returns(new[] { newExplosmComic1, newExplosmComic2 });

            var dilbert = new Mock<IComicDownloader>();
            dilbert.Setup(m => m.GetNewComicsSince(lastDilbert))
                .Returns(new[] { newDilbert1, newDilbert2 });

            mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLastImportedComic(ComicType.Explosm))
                .Returns(lastExplosmComic)
                .Verifiable();

            mocker.GetMock<IComicsRepository>()
               .Setup(m => m.GetLastImportedComic(ComicType.Dilbert))
               .Returns(lastDilbert)
               .Verifiable();

            var registry = new ComicConfigRegistry();
            registry.Add(new ComicConfig(ComicType.Explosm, explosm.Object));
            registry.Add(new ComicConfig(ComicType.Dilbert, dilbert.Object));

            // ReSharper disable once RedundantTypeArgumentsOfMethod
            mocker.SetInstance<ComicConfigRegistry>(registry);

            mocker.Create<ImportProcess>()
                .Run();

            mocker.GetMock<IComicsRepository>().VerifyAll();
            mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(newExplosmComic1), Times.Once);
            mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(newExplosmComic2), Times.Once);
            mocker.GetMock<IComicsRepository>()
              .Verify(m => m.InsertComic(newDilbert1), Times.Once);
            mocker.GetMock<IComicsRepository>()
                .Verify(m => m.InsertComic(newDilbert2), Times.Once);
        }
    }
}
