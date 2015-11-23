using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Comics.Core.Downloaders;
using Comics.Core.Persistence;
using Comics.Core.Presentation;

namespace Comics.Web.Controllers
{
    public class ExplosmController : Controller
    {
        private readonly IComicsRepository _comicsRepository;
        private readonly IExplosmImporter _importer;

        public ExplosmController(IComicsRepository comicsRepository, IExplosmImporter importer)
        {
            _comicsRepository = comicsRepository;
            _importer = importer;
        }

        // GET: Explosm
        public FileStreamResult Feed()
        {
            _importer.ImportNewComics();

            var comics = _comicsRepository.GetLatestComics(ComicType.Explosm);

            var feed = new ComicFeed("Explosm Comics", new Uri("http://example.com"));
            var xml = feed.Render(comics);
            return new FileStreamResult(new MemoryStream(Encoding.UTF8.GetBytes(xml)), "text/xml");
        }
    }
}