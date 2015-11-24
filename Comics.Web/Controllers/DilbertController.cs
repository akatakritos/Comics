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
    public class DilbertController : Controller
    {
        private readonly IDilbertImporter _importer;
        private readonly IComicsRepository _repository;

        public DilbertController(IDilbertImporter importer, IComicsRepository repository)
        {
            _importer = importer;
            _repository = repository;
        }

        public FileStreamResult Feed()
        {
            _importer.ImportNewComics();

            var comics = _repository.GetLatestComics(ComicType.Explosm);

            var feed = new ComicFeed("Dilbert Comics", new Uri("http://example.com"));
            var xml = feed.Render(comics);
            return new FileStreamResult(new MemoryStream(Encoding.UTF8.GetBytes(xml)), "text/xml");
        }
    }
}