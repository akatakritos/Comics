using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Comics.Core.Persistence;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Persistence
{
    [Trait("Category", "Integration")]
    public class ComicsRepositoryTests
    {
        [Fact]
        public async void CanInsertAComic()
        {
            var database = IntegrationDatabase.CreateBlankDatabase();
            using (var context = database.CreateContext())
            {
                var repo = new ComicsRepository(context);
                repo.InsertComic(new Comic()
                {
                    ComicType = ComicType.Explosm,
                    ComicNumber = 1,
                    ImageSrc = "http://www.example.com/example.png",
                    PublishedDate = new DateTime(2015, 5, 17)
                });

                var inserted = await context.Comics.SingleAsync();

                Check.That(inserted.ComicType).IsEqualTo(ComicType.Explosm);
                Check.That(inserted.ComicNumber).IsEqualTo(1);
                Check.That(inserted.ImageSrc).IsEqualTo("http://www.example.com/example.png");
                Check.That(inserted.PublishedDate).IsEqualTo(new DateTime(2015, 5, 17));
                Check.That(inserted.ComicId).IsGreaterThan(0);
            }
        }

        [Fact]
        public async void GetLastComicGetsNewestOfType()
        {
            var database = IntegrationDatabase.CreateBlankDatabase();
            using (var context = database.CreateContext())
            {
                context.Comics.Add(new Comic() { ComicType = ComicType.Unknown, PublishedDate = DateTime.Today });
                context.Comics.Add(new Comic { ComicType = ComicType.Explosm, ComicNumber = 5, ImageSrc = "http://example.com/test.png", PublishedDate = new DateTime(2015, 11, 12) });
                context.Comics.Add(new Comic { ComicType = ComicType.Explosm, ComicNumber = 6, ImageSrc = "http://example.com/test.png", PublishedDate = new DateTime(2015, 11, 13) });
                await context.SaveChangesAsync();

                var repo = new ComicsRepository(context);
                var lastComic = repo.GetLastImportedComic(ComicType.Explosm);

                Check.That(lastComic.ComicType).IsEqualTo(ComicType.Explosm);
                Check.That(lastComic.ComicNumber).IsEqualTo(6);
            }
        }

        [Fact]
        public async void GetLatest_OnlyGrabsTheSpecifiedNumber()
        {
            var database = IntegrationDatabase.CreateBlankDatabase();
            using (var context = database.CreateContext())
            {
                DateTime startDate = new DateTime(2015, 5, 1);
                for (int i = 1; i <= 15; i++)
                {
                    var comic = new Comic()
                    {
                        ComicNumber = i,
                        ComicType = ComicType.Explosm,
                        ImageSrc = $"http://www.example.com/images/{i}.png",
                        PublishedDate = startDate.AddDays(i).Date
                    };

                    context.Comics.Add(comic);
                }

                await context.SaveChangesAsync();

                var repo = new ComicsRepository(context);
                var recent = repo.GetLatestComics(ComicType.Explosm);

                Check.That(recent).HasSize(10);
            }
        }

        [Fact]
        public async void GetLatest_ComicsAreInReverseChronologicalOrder()
        {
            var database = IntegrationDatabase.CreateBlankDatabase();
            using (var context = database.CreateContext())
            {
                DateTime startDate = new DateTime(2015, 5, 1);
                for (int i = 0; i < 15; i++)
                {
                    var comic = new Comic()
                    {
                        ComicNumber = i+1,
                        ComicType = ComicType.Explosm,
                        ImageSrc = $"http://www.example.com/images/{i}.png",
                        PublishedDate = startDate.AddDays(i).Date
                    };

                    context.Comics.Add(comic);
                }

                await context.SaveChangesAsync();

                var repo = new ComicsRepository(context);
                var recent = repo.GetLatestComics(ComicType.Explosm);

                Check.That(recent.First().PublishedDate).IsEqualTo(new DateTime(2015, 5, 15));
                Check.That(recent.Last().PublishedDate).IsEqualTo(new DateTime(2015, 5, 6));
            }
        }
    }
}
