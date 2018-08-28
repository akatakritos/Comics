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
        const string LatestFixture = "pearls-2018-08-02";

        [Fact]
        public void Parse_FindsTheComicSrc()
        {
            var html = Fixture.Load(LatestFixture);

            var result = PearlsParser.Parse(html);

            Check.That(result.ImageUri).IsEqualTo(new Uri("https://assets.amuniversal.com/ec1188a0718901364736005056a9545d"));
        }

        [Fact]
        public void Parse_FindsThePublishedDate()
        {
            var html = Fixture.Load(LatestFixture);

            var result = PearlsParser.Parse(html);

            Check.That(result.PublishedDate).IsEqualTo(new DateTime(2018, 8, 2));
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
