using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comics.Core.Persistence
{
    public enum ComicType
    {
        Explosm = 1,
    }

    public class Comic
    {
        public ComicType ComicType { get; set; }
        public int ComicNumber { get; set; }
        public string ImageSrc { get; set; }
        public DateTime PublishedDate { get; set; }

        public override string ToString()
        {
            return $"{ComicType} ({ComicNumber}) published {PublishedDate}";
        }
    }


    public interface IComicsRepository
    {
        Comic GetLastImportedComic(ComicType type);
        void InsertComic(Comic comic);
    }

    public class ComicsContext
    {
    }
}
