using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;

using Comics.Core.Downloaders;
using Comics.Core.Import;
using Comics.Core.Persistence;

namespace Comics.Web
{
    public static class ComicTypes
    {
        /// <summary>
        /// Autofac will call this each time a class depends on ComicConfigRegistry
        /// </summary>
        /// <param name="componentRegistry"></param>
        /// <returns></returns>
        public static ComicConfigRegistry RegisterComics(IComponentContext componentRegistry)
        {
            var registry = new ComicConfigRegistry();

            registry.Add(
                new ComicConfig(ComicType.Dilbert, componentRegistry.Resolve<DilbertDownloader>()));
            registry.Add(
                new ComicConfig(ComicType.Explosm, componentRegistry.Resolve<ExplosmDownloader>()));
            registry.Add(
                new ComicConfig(ComicType.Pearls, componentRegistry.Resolve<PearlsDownloader>()));

            return registry;
        }
    }
}