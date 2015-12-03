using Comics.Core.Persistence;

namespace Comics.Core.Import
{
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
