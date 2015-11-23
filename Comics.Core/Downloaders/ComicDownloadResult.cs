using System;
using System.Collections.Generic;
using System.Linq;

namespace Comics.Core.Downloaders
{
    public struct ComicDownloadResult
    {
        public int StatusCode { get; }
        public string Content { get; }
        public int ComicNumber { get; }
        public bool NotFound => StatusCode == 404;

        public ComicDownloadResult(int statusCode, string content, int comicNumber)
        {
            ComicNumber = comicNumber;
            StatusCode = statusCode;
            Content = content;
        }

    }
}