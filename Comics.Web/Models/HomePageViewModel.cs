using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Persistence;

namespace Comics.Web.Models
{
    public class HomePageViewModel
    {
        public Comic TodaysDilbert { get; set; }
        public Comic TodaysExplosm { get; set; }
    }
}