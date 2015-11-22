using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Comics.Tests
{
    public static class Fixture
    {
        public static string Load(string fixtureName)
        {
            return File.ReadAllText(FilenameFor(fixtureName));
        }

        private static string FilenameFor(string fixtureName)
        {
            return Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\..\Fixtures\",
                fixtureName + ".txt");
        }
    }
}
