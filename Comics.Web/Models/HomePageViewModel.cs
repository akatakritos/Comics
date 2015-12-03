using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Persistence;

namespace Comics.Web.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<Comic> LatestComics { get; set; }
    }
}