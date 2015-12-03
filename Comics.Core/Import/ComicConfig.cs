using Comics.Core.Downloaders;
using Comics.Core.Persistence;

namespace Comics.Core.Import
{
    public class ComicConfig
    {
        public ComicType ComicType { get; }
        public IComicDownloader Downloader { get; }

        public ComicConfig(ComicType comicType, IComicDownloader downloader)
        {
            ComicType = comicType;
            Downloader = downloader;
        }
    }
}