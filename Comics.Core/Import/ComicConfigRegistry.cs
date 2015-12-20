using System.Collections.Generic;

namespace Comics.Core.Import
{
    public class ComicConfigRegistry
    {
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