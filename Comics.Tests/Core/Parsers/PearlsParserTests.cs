using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Parsers;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Parsers
{

    public class PearlsParserTests
    {
        [Fact]
        public void Parse_FindsTheComicSrc()
        {
            var html = Fixture.Load("pearls-2015-12-04");

            var result = PearlsParser.Parse(html);

            Check.That(result.ImageUri).IsEqualTo(new Uri("http://assets.amuniversal.com/5a7e112071f701331e71005056a9545d"));
        }

        [Fact]
        public void Parse_FindsThePublishedDate()
        {
            var html = Fixture.Load("pearls-2015-12-04");

            var result = PearlsParser.Parse(html);

            Check.That(result.PublishedDate).IsEqualTo(new DateTime(2015, 12, 4));
        }

        [Fact]
        public void Parse_FailsGracefully()
        {
            const string html = "<html/>";

            var result = PearlsParser.Parse(html);

            Check.That(result.Succeeded).IsFalse();
        }

        [Fact]
        public void Parse_ThrowsForNullInput()
        {
            Check.ThatCode(() => PearlsParser.Parse(null))
                .Throws<ArgumentNullException>();
        }

        [Fact]
        public void Parse_ThrowsForEmptyInput()
        {
            Check.ThatCode(() => PearlsParser.Parse(""))
                .Throws<ArgumentException>();
        }
    }
}
