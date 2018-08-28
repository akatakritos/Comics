using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Parsers;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Parsers
{
    public class ExplosmParser_ParseTests
    {
        [Fact]
        public void FindsTheComicUrl()
        {
            var html = Fixture.Load("explosm-4125");

            var result = ExplosmParser.Parse(html);

            result.AssertValid();
            Check.That(result.ImageUri).IsEqualTo(new Uri("http://files.explosm.net/comics/Kris/knowingis.png"));
        }

        [Fact]
        public void FindsTheComicDate()
        {
            var html = Fixture.Load("explosm-4125");

            var result = ExplosmParser.Parse(html);

            result.AssertValid();
            Check.That(result.PublishedDate).IsEqualTo(new DateTime(2015, 11, 22));
        }

        [Fact]
        public void ParseErrors()
        {
            const string html = "<html></html>";

            var result = ExplosmParser.Parse(html);

            Check.That(result.Succeeded).IsFalse();
            Check.That(result.FailureMessage).IsNotEmpty();
        }

        [Fact]
        public void NullInputThrows()
        {
            Check.ThatCode(() => ExplosmParser.Parse(null)).Throws<ArgumentNullException>();
        }

        [Fact]
        public void EmptyInputThrows()
        {
            Check.ThatCode(() => ExplosmParser.Parse("")).Throws<ArgumentException>();
        }

        [Fact]
        public void QueryStringsAreNotEncoded()
        {
            var html = Fixture.Load("explosm-latest");
            var result = ExplosmParser.Parse(html);
            
            result.AssertValid();
            Check.That(result.ImageUri).IsEqualTo(new Uri("http://files.explosm.net/comics/Rob/thatgirl3.png?t=5CE637"));
        }
    }
}
