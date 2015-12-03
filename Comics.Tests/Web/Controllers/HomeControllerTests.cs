using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AutoMoq;

using Comics.Core.Downloaders;
using Comics.Core.Persistence;
using Comics.Web.Controllers;
using Comics.Web.Models;

using NFluent;

using Xunit;

namespace Comics.Tests.Web.Controllers
{
    public class HomeControllerTests
    {
        private readonly AutoMoqer _mocker;
        private readonly ComicConfigRegistry _registry;

        private readonly Comic _latestDilbert;
        private readonly Comic _latestExplosm;

        public HomeControllerTests()
        {
            _mocker = new AutoMoqer();

            _registry = new ComicConfigRegistry();
            _mocker.SetInstance(_registry);

            _latestDilbert = RegisterComic(ComicType.Dilbert);
            _latestExplosm = RegisterComic(ComicType.Explosm);
        }

        private Comic RegisterComic(ComicType type)
        {
            var comic = new Comic() { ComicType = type };
            _mocker.GetMock<IComicsRepository>()
                 .Setup(m => m.GetLastImportedComic(type))
                 .Returns(comic);

            _registry.Add(new ComicConfig(type, null));

            return comic;
        }

        [Fact]
        public void Home_SetsTheMostRecentDilbertComic()
        {
            var result = _mocker.Create<HomeController>().Index();
            var model = (HomePageViewModel)((ViewResult)result).Model;

            var dilbert = model.LatestComics.First(c => c.ComicType == ComicType.Dilbert);

            Check.That(dilbert).IsEqualTo(_latestDilbert);
        }

        [Fact]
        public void Index_SetsTheMostRecentExplosmComic()
        {
            var result = _mocker.Create<HomeController>().Index();
            var model = (HomePageViewModel)((ViewResult)result).Model;

            var dilbert = model.LatestComics.First(c => c.ComicType == ComicType.Explosm);

            Check.That(dilbert).IsEqualTo(_latestExplosm);
        }
    }
}
