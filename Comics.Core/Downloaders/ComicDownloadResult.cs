using System;
using System.Collections.Generic;
using System.Linq;

namespace Comics.Core.Downloaders
{
    public struct ComicDownloadResult
    {
        public Uri Permalink { get; }
        public int StatusCode { get; }
        public string Content { get; }
        public int ComicNumber { get; }
        public bool NotFound => StatusCode == 404;

        public ComicDownloadResult(int statusCode, string content, int comicNumber, Uri permalink)
        {
            ComicNumber = comicNumber;
            Permalink = permalink;
            StatusCode = statusCode;
            Content = content;
        }

    }
}