using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Parsers;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Parsers
{
    public class ExplosmParser_ParsePermalinkTests
    {
        [Fact]
        public void ItCanFindThePermalink()
        {
            var html = Fixture.Load("explosm-latest");

            var permalink = ExplosmParser.ParsePermalink(html);

            Check.That(permalink).IsEqualTo("http://explosm.net/comics/4125/");
        }
    }
}
