using System;
using System.Collections.Generic;
using System.Linq;

namespace Comics.Core.Parsers
{
    public class ComicParseResult
    {
        public Uri ImageUri { get; }
        public DateTime PublishedDate { get; }

        public bool Succeeded { get; }
        public string FailureMessage { get; }

        private ComicParseResult(Uri uri, DateTime publishedDate)
        {
            Succeeded = true;
            ImageUri = uri;
            PublishedDate = publishedDate;
        }

        private ComicParseResult(string failureMesage)
        {
            Succeeded = false;
            FailureMessage = failureMesage;
        }

        public static ComicParseResult Fail(string failureMessage)
        {
            return new ComicParseResult(failureMessage);
        }

        public static ComicParseResult Succeed(Uri imageUri, DateTime publishedDate)
        {
            return new ComicParseResult(imageUri, publishedDate);
        }

        public void AssertValid()
        {
            if (!Succeeded)
            {
                throw new Exception(FailureMessage);
            }
        }
    }
}