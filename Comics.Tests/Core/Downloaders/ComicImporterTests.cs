using System;
using System.Collections.Generic;
using System.Linq;

using AutoMoq;

using Comics.Core.Downloaders;

using Moq;

using Xunit;

namespace Comics.Tests.Core.Downloaders
{
    public class ComicImporterTests
    {
        [Fact]
        public void ImportsDilbertComics()
        {
            var mocker = new AutoMoqer();

            mocker.Create<ComicImporter>().ImportNewComics();

            mocker.GetMock<IDilbertImporter>()
                .Verify(m => m.ImportNewComics(null), Times.Once);
        }

        [Fact]
        public void ImportsExplosmComics()
        {
            var mocker = new AutoMoqer();

            mocker.Create<ComicImporter>().ImportNewComics();

            mocker.GetMock<IExplosmImporter>()
                .Verify(m => m.ImportNewComics(4125), Times.Once);
        }

    }
}
