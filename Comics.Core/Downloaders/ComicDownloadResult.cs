using System;
using System.Collections.Generic;
using System.Linq;

namespace Comics.Core.Downloaders
{
    public struct ComicDownloadResult
    {
        public Uri Permalink { get; }
        public string Content { get; }
        public int ComicNumber { get; }

        public ComicDownloadResult(string content, int comicNumber, Uri permalink)
        {
            ComicNumber = comicNumber;
            Permalink = permalink;
            Content = content;
        }

        public override string ToString()
        {
            return $"ComicNumber: {ComicNumber}, Permalink: {Permalink}";
        }
    }
}