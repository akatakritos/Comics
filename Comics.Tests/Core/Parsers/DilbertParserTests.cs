using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Parsers;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Parsers
{
    public class DilbertParserTests
    {
        [Fact]
        public void Parse_FindsTheComicSrc()
        {
            var html = Fixture.Load("dilbert-2015-11-23");

            var result = DilbertParser.Parse(html);

            Check.That(result.ImageUri).IsEqualTo(new Uri("http://assets.amuniversal.com/041159a06560013319a6005056a9545d"));
        }

        [Fact]
        public void Parse_FindsThePublishedDate()
        {
            var html = Fixture.Load("dilbert-2015-11-23");

            var result = DilbertParser.Parse(html);

            Check.That(result.PublishedDate).IsEqualTo(new DateTime(2015, 11, 23));
        }

        [Fact]
        public void Parse_FailsGracefully()
        {
            const string html = "<html/>";

            var result = DilbertParser.Parse(html);

            Check.That(result.Succeeded).IsFalse();
        }

        [Fact]
        public void Parse_ThrowsForNullInput()
        {
            Check.ThatCode(() => DilbertParser.Parse(null))
                .Throws<ArgumentNullException>();
        }

        [Fact]
        public void Parse_ThrowsForEmptyInput()
        {
            Check.ThatCode(() => DilbertParser.Parse(""))
                .Throws<ArgumentException>();
        }
    }
}
