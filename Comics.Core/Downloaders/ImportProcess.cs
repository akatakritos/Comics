using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Persistence;

namespace Comics.Core.Downloaders
{
    public class ComicConfigRegistry
    {
        public static ComicConfigRegistry Registry { get; }

        static ComicConfigRegistry()
        {
            Registry = new ComicConfigRegistry();
        }

        public ComicConfigRegistry()
        {
            _entries = new List<ComicConfig>();
        }

        public void Add(ComicConfig config)
        {
            _entries.Add(config);
        }

        private readonly IList<ComicConfig> _entries;
        public IEnumerable<ComicConfig> Entries => _entries;

    }

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

    public interface IImportProcess
    {
        void Run();
    }

    public class ImportProcess : IImportProcess
    {
        private readonly IComicsRepository _comics;
        private readonly ComicConfigRegistry _registry;

        public ImportProcess(IComicsRepository comics, ComicConfigRegistry registry)
        {
            _comics = comics;
            _registry = registry;
        }

        public void Run()
        {
            foreach (var comicConfig in _registry.Entries)
            {
                var lastComic = _comics.GetLastImportedComic(comicConfig.ComicType);

                var newComics = comicConfig.Downloader.GetNewComicsSince(lastComic);

                foreach (var comic in newComics)
                {
                    _comics.InsertComic(comic);
                }
            }
        }
    }
}
