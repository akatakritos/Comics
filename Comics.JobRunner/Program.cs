using System;
using System.Collections.Generic;
using System.Linq;

using Comics.Core.Downloaders;
using Comics.Core.Persistence;

namespace Comics.JobRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ComicsContext())
            {
                var repository = new ComicsRepository(context);
                var client = new ExplosmWebClient();

                var importer = new ExplosmImporter(repository, client);
                importer.ImportNewComics(4120);
            }
        }
    }
}
