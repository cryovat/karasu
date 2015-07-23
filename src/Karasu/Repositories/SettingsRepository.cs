using System;
using System.Collections.Generic;
using System.IO;

namespace Karasu.Repositories
{
    public interface ISettingsRepository
    {
        string MplayerPath { get; }
        bool AddPath(string path);
        IEnumerable<string> ListPaths();
    }

    public class SettingsRepository : ISettingsRepository
    {
        private readonly HashSet<string> _paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public string MplayerPath { get; }

        public SettingsRepository(string mplayerPath, IEnumerable<string> libraryPaths)
        {
            MplayerPath = mplayerPath;

            foreach (var path in libraryPaths)
            {
                AddPath(path);
            }
        }

        public bool AddPath(string path)
        {
            if (!Directory.Exists(path) || _paths.Contains(path)) return false;

            _paths.Add(path);
            return true;
        }

        public IEnumerable<string> ListPaths()
        {
            return _paths;
        }
    }
}
