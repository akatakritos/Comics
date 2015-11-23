using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Comics.Core.Persistence
{
    public class Comic
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComicId { get; set; }

        public ComicType ComicType { get; set; }

        public int ComicNumber { get; set; }

        public string ImageSrc { get; set; }

        public DateTime PublishedDate { get; set; }

        public override string ToString()
        {
            return $"{ComicType} ({ComicNumber}) published {PublishedDate}";
        }
    }
}