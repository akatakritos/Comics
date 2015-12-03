using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Persistence;

namespace Comics.Core.Downloaders
{
    public interface IComicDownloader
    {
        IEnumerable<Comic> GetNewComicsSince(Comic lastDownloaded);
    }
}