using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Comics.Core.Extensions;
using Comics.Core.Persistence;
using Comics.Core.Presentation;

namespace Comics.Web.Controllers
{
    public class FeedController : Controller
    {
        private readonly IComicsRepository _comics;

        public FeedController(IComicsRepository comics)
        {
            _comics = comics;
        }

        // GET: Feed
        public FileStreamResult Feed(ComicType type)
        {
            var comics = _comics.GetLatestComics(type);

            var feed = new ComicFeed($"{type.ToDisplayName()} Comics", new Uri("http://example.com"));
            var xml = feed.Render(comics);
            return new FileStreamResult(new MemoryStream(Encoding.UTF8.GetBytes(xml)), "text/xml");
        }
    }
}