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
        const string LatestFixture = "pearls-2017-01-13";

        [Fact]
        public void Parse_FindsTheComicSrc()
        {
            var html = Fixture.Load(LatestFixture);

            var result = PearlsParser.Parse(html);

            Check.That(result.ImageUri).IsEqualTo(new Uri("http://assets.amuniversal.com/3a70d0d0b3fa013428f8005056a9545d"));
        }

        [Fact]
        public void Parse_FindsThePublishedDate()
        {
            var html = Fixture.Load(LatestFixture);

            var result = PearlsParser.Parse(html);

            Check.That(result.PublishedDate).IsEqualTo(new DateTime(2017, 01, 13));
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
