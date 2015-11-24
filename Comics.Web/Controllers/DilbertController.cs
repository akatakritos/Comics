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
    public class DilbertController : Controller
    {
        private readonly IComicsRepository _repository;

        public DilbertController(IComicsRepository repository)
        {
            _repository = repository;
        }

        public FileStreamResult Feed()
        {
            var comics = _repository.GetLatestComics(ComicType.Dilbert);

            var feed = new ComicFeed("Dilbert Comics", new Uri("http://example.com"));
            var xml = feed.Render(comics);
            return new FileStreamResult(new MemoryStream(Encoding.UTF8.GetBytes(xml)), "text/xml");
        }
    }
}