using System;
using System.Collections.Generic;
using System.Linq;

namespace Comics.Core.Downloaders
{
    public struct ComicDownloadResult
    {
        public int StatusCode { get; }
        public string Content { get; }
        public bool NotFound => StatusCode == 404;

        public ComicDownloadResult(int statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }

    }
}