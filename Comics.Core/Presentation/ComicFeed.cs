using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Comics.Core.Persistence;

namespace Comics.Core.Presentation
{
    public class ComicFeed
    {
        public string Name { get; }
        public Uri FeedUri { get; }

        public string Description { get; set; }

        public ComicFeed(string name, Uri feedUri)
        {
            Name = name;
            FeedUri = feedUri;
            Description = name;
        }

        public string Render(IEnumerable<Comic> comics)
        {
            var feed = new SyndicationFeed(Name, Description, FeedUri);

            feed.Items = comics.Select(MapComicToItem);

            var sw = new StringWriter();
            using (var writer = new XmlTextWriter(sw))
            {
                new Rss20FeedFormatter(feed).WriteTo(writer);
                return sw.ToString();
            }
        }

        private SyndicationItem MapComicToItem(Comic c)
        {
            var content = $"<img src='{c.ImageSrc}' />";
            return new SyndicationItem(
                c.PublishedDate.ToString("yyyy-MM-dd"),
                content,
                new Uri(c.Permalink));
        }
    }
}
