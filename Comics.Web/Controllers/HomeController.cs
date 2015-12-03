using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Comics.Core.Downloaders;
using Comics.Core.Persistence;
using Comics.Web.Models;

namespace Comics.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IComicsRepository _comics;
        private readonly ComicConfigRegistry _registry;

        public HomeController(IComicsRepository comics, ComicConfigRegistry registry)
        {
            _comics = comics;
            _registry = registry;
        }

        // GET: Home
        public ActionResult Index()
        {
            var latestComics = _registry.Entries
                .Select(e => _comics.GetLastImportedComic(e.ComicType))
                .ToArray();

            var model = new HomePageViewModel { LatestComics = latestComics };
            return View(model);
        }
    }
}