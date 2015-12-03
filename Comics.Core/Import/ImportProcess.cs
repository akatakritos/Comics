using System.Collections.Generic;

using Comics.Core.Persistence;

namespace Comics.Core.Import
{
    public interface IImportProcess
    {
        void Run();
        IReadOnlyList<Comic> ImportedComics { get; }
    }

    public class ImportProcess : IImportProcess
    {
        private readonly IComicsRepository _comics;
        private readonly ComicConfigRegistry _registry;
        private readonly List<Comic> _importedComics;

        public ImportProcess(IComicsRepository comics, ComicConfigRegistry registry)
        {
            _comics = comics;
            _registry = registry;
            _importedComics = new List<Comic>();
        }

        public void Run()
        {
            _importedComics.Clear();

            foreach (var comicConfig in _registry.Entries)
            {
                var lastComic = _comics.GetLastImportedComic(comicConfig.ComicType);

                var newComics = comicConfig.Downloader.GetNewComicsSince(lastComic);

                foreach (var comic in newComics)
                {
                    _comics.InsertComic(comic);
                    _importedComics.Add(comic);
                }
            }
        }

        public IReadOnlyList<Comic> ImportedComics => _importedComics;

    }
}
