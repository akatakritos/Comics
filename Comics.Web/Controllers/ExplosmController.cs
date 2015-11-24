using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Comics.Core.Persistence;
using Comics.Core.Presentation;

namespace Comics.Web.Controllers
{
    public class ExplosmController : Controller
    {
        private readonly IComicsRepository _comicsRepository;

        public ExplosmController(IComicsRepository comicsRepository)
        {
            _comicsRepository = comicsRepository;
        }

        // GET: Explosm
        public FileStreamResult Feed()
        {
            var comics = _comicsRepository.GetLatestComics(ComicType.Explosm);

            var feed = new ComicFeed("Cyanide & Happiness Comics", new Uri("http://example.com"));
            var xml = feed.Render(comics);
            return new FileStreamResult(new MemoryStream(Encoding.UTF8.GetBytes(xml)), "text/xml");
        }
    }
}