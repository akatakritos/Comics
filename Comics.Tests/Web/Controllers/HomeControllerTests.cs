using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AutoMoq;

using Comics.Core.Persistence;
using Comics.Web.Controllers;
using Comics.Web.Models;

using NFluent;

using Xunit;

namespace Comics.Tests.Web.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Home_SetsTheMostRecentDilbertComic()
        {
            var mocker = new AutoMoqer();
            var mockComic = new Comic();
            mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLastImportedComic(ComicType.Dilbert))
                .Returns(mockComic);

            var result = mocker.Create<HomeController>().Index();
            var model = (HomePageViewModel)((ViewResult)result).Model;

            Check.That(model.TodaysDilbert).IsEqualTo(mockComic);
        }

        [Fact]
        public void Index_SetsTheMostRecentExplosmComic()
        {
            var mocker = new AutoMoqer();
            var mockComic = new Comic();
            mocker.GetMock<IComicsRepository>()
                .Setup(m => m.GetLastImportedComic(ComicType.Explosm))
                .Returns(mockComic);

            var result = mocker.Create<HomeController>().Index();
            var model = (HomePageViewModel)((ViewResult)result).Model;

            Check.That(model.TodaysExplosm).IsEqualTo(mockComic);
        }
    }
}
