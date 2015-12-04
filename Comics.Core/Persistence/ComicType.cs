using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Comics.Core.Persistence
{
    public enum ComicType
    {
        Unknown = 0,
        [Display(Name="Cyanide & Happiness")]
        Explosm = 1,
        Dilbert = 2,
    }
}