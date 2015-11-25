using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Comics.Core.Persistence;
using Comics.Web.Models;

namespace Comics.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IComicsRepository _comics;
        public HomeController(IComicsRepository comics)
        {
            _comics = comics;
        }

        // GET: Home
        public ActionResult Index()
        {
            var model = new HomePageViewModel()
            {
                TodaysDilbert = _comics.GetLastImportedComic(ComicType.Dilbert),
                TodaysExplosm = _comics.GetLastImportedComic(ComicType.Explosm),
            };
            return View(model);
        }
    }
}