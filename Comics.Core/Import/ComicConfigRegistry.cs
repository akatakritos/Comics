using System.Collections.Generic;

using Comics.Core.Downloaders;

namespace Comics.Core.Import
{
    public class ComicConfigRegistry
    {
        public static ComicConfigRegistry Registry { get; }

        static ComicConfigRegistry()
        {
            Registry = new ComicConfigRegistry();
        }

        public ComicConfigRegistry()
        {
            _entries = new List<ComicConfig>();
        }

        public void Add(ComicConfig config)
        {
            _entries.Add(config);
        }

        private readonly IList<ComicConfig> _entries;
        public IEnumerable<ComicConfig> Entries => _entries;

    }
}