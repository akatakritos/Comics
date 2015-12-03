using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Comics.Core.Downloaders;
using Comics.Core.Persistence;

namespace Comics.Web
{
    public static class ComicTypes
    {
        public static void RegisterComics()
        {
            var context = DependencyResolver.Current;
            ComicConfigRegistry.Registry.Add(
                new ComicConfig(ComicType.Dilbert, context.GetService<DilbertDownloader>()));
            ComicConfigRegistry.Registry.Add(
                new ComicConfig(ComicType.Explosm, context.GetService<ExplosmDownloader>()));
        }
    }
}