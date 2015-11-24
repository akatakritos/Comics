using System;
using System.Collections.Generic;
using System.Linq;

namespace Comics.Core.Downloaders
{
    public interface IComicImporter
    {
        void ImportNewComics();
    }

    public class ComicImporter : IComicImporter
    {
        private readonly IDilbertImporter _dilbert;
        private readonly IExplosmImporter _explosm;

        public ComicImporter(IDilbertImporter dilbert,
                             IExplosmImporter explosm)
        {
            _dilbert = dilbert;
            _explosm = explosm;
        }

        public void ImportNewComics()
        {
            _dilbert.ImportNewComics();
            _explosm.ImportNewComics();
        }
    }
}
