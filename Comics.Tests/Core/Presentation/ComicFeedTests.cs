using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

using Comics.Core.Persistence;
using Comics.Core.Presentation;

using HtmlAgilityPack;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Presentation
{
    public class ComicFeedTests
    {
        [Fact]
        public void ItContainsATitle()
        {
            var feed = new ComicFeed("Explosm Comic Feed", new Uri("http://example.com"));

            var xml = feed.Render(Enumerable.Empty<Comic>());

            var rss = ReadRssXml(xml);
            Check.That(rss.Title.Text).IsEqualTo("Explosm Comic Feed");
        }

        [Fact]
        public void CanSetDescription()
        {
            var feed = new ComicFeed("Explosm Comic Feed", new Uri("http://example.com"));
            feed.Description = "Feed Description";

            var xml = feed.Render(Enumerable.Empty<Comic>());

            var rss = ReadRssXml(xml);
            Check.That(rss.Description.Text).IsEqualTo("Feed Description");
        }

        [Fact]
        public void PassedInItemsTurnIntoFeedItems()
        {
            var feed = new ComicFeed("Foobar", new Uri("http://abc.def"));

            var xml = feed.Render(Enumerable.Repeat(new Comic()
            {
                Permalink = "http://example.com/"
            }, 5));

            var rss = ReadRssXml(xml);
            Check.That(rss.Items).HasSize(5);
        }

        [Fact]
        public void ItemHasComicsDateAsName()
        {
            var feed = new ComicFeed("Foobar", new Uri("http://abc.def"));

            var xml = feed.Render(new[]
            {
                new Comic()
                {
                    PublishedDate = new DateTime(2015, 5, 16),
                    Permalink = "http://example.com/"
                }
            });

            var rss = ReadRssXml(xml);
            Check.That(rss.Items.Single().Title.Text).IsEqualTo("2015-05-16");
        }

        [Fact]
        public void ItemLinksToPermalink()
        {
            var feed = new ComicFeed("Foobar", new Uri("http://abc.def"));

            var xml = feed.Render(new[]
            {
                new Comic() { Permalink = "http://www.google.com/" }
            });

            var rss = ReadRssXml(xml);
            Check.That(rss.Items.Single().Links.First().Uri.ToString()).IsEqualTo("http://www.google.com/");
        }

        [Fact]
        public void ContentHasImageTagWithSource()
        {
            var feed = new ComicFeed("Foobar", new Uri("http://abc.def"));

            var xml = feed.Render(new[]
            {
                new Comic()
                {
                    Permalink = "http://www.google.com/",
                    ImageSrc = "http://example.com/image.png"
                }
            });

            var rss = ReadRssXml(xml);
            var doc = new HtmlDocument();
            doc.LoadHtml(rss.Items.First().Summary.Text);
            var img = doc.QuerySelector("img");

            Check.That(img.GetAttributeValue("src", null)).IsEqualTo("http://example.com/image.png");
        }

        private SyndicationFeed ReadRssXml(string xml)
        {
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
            {
                return SyndicationFeed.Load(reader);
            }
        }
    }
}
