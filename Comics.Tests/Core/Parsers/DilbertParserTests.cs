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
            var html = Fixture.Load("dilbert-2018-11-29");

            var result = DilbertParser.Parse(html);

            Check.That(result.ImageUri).IsEqualTo(new Uri("https://assets.amuniversal.com/ec914e90c8c601366722005056a9545d"));
        }

        [Fact]
        public void Parse_FindsThePublishedDate()
        {
            var html = Fixture.Load("dilbert-2018-11-29");

            var result = DilbertParser.Parse(html);

            Check.That(result.PublishedDate).IsEqualTo(new DateTime(2018, 11, 29));
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
