using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Karasu.Model;

namespace Karasu.Repositories
{
    public interface ISongRepository
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<Song> SearchSongs(string category, string query, int skip, int take);
        IQueryable<QueueItem> ListQueue();

        QueueItem EnqueueSong(string playerName, string playerHex, int songId, bool secret);
        bool HasQueuedSongs();
        QueueItem PopQueue();
        void Reindex(params string[] paths);
    }

    public class SongRepository : ISongRepository
    {
        private readonly object _sync = new object();

        private List<Category> _categories = new List<Category>();
        private Queue<QueueItem> _queue = new Queue<QueueItem>();

        public IEnumerable<Category> GetCategories()
        {
            return _categories;
        }

        public IEnumerable<Song> SearchSongs(string category, string query, int skip, int take)
        {
            var songs = string.IsNullOrWhiteSpace(category)
                ? _categories.SelectMany(c => c.Songs)
                : _categories.Where(c => c.Name.Equals(category, StringComparison.OrdinalIgnoreCase)).SelectMany(c => c.Songs);

            songs = songs.OrderBy(s => s.Artist).ThenBy(s => s.Title);

            if (string.IsNullOrWhiteSpace(query))
            {
                return songs.Skip(skip).Take(take);
            }

            var parts = query.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            return
                songs.Where(
                    s =>
                        parts.All(
                            p =>
                                s.Artist.IndexOf(p, StringComparison.OrdinalIgnoreCase) > -1 ||
                                s.Title.IndexOf(p, StringComparison.OrdinalIgnoreCase) > -1))
                                .Skip(skip)
                                .Take(take);
        }

        public IQueryable<QueueItem> ListQueue()
        {
            return _queue.AsQueryable();
        }

        public QueueItem EnqueueSong(string playerName, string playerHexColor, int songId, bool secret)
        {
            lock (_sync)
            {
                var song = _categories.SelectMany(c => c.Songs).FirstOrDefault(s => s.Id == songId);

                if (song == null) return null;

                var item = new QueueItem
                {
                    Player = new Player
                    {
                        Name = playerName,
                        HexColor = playerHexColor
                    },
                    Song = song,
                    Secret = secret,
                };

                _queue.Enqueue(item);

                return item;
            }
        }

        public bool HasQueuedSongs()
        {
            return _queue.Count > 0;
        }

        public QueueItem PopQueue()
        {
            lock (_sync)
            {
                return _queue.Count > 0 ? _queue.Dequeue() : null;
            }
        }

        public void Reindex(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
            {
                lock (_sync)
                {
                    _categories = new List<Category>();
                    _queue = new Queue<QueueItem>();
                    return;
                }
            }

            var categories = new Dictionary<string, Category>(StringComparer.OrdinalIgnoreCase);
            var totalCount = 0;

            foreach (var path in paths)
            {
                if (!Directory.Exists(path)) continue;

                var libraryDirName = Path.GetFileName(Path.GetDirectoryName(path));

                foreach (var file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    var name = Path.GetFileNameWithoutExtension(file) ?? "Unknown Song";
                    var parts = name.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                    var categoryName = "Uncategorized";
                    Category category;

                    var songDirName = Path.GetFileName(Path.GetDirectoryName(file));

                    if (songDirName != null && !string.Equals(libraryDirName, songDirName, StringComparison.OrdinalIgnoreCase))
                    {
                        categoryName = songDirName;
                    }

                    if (!categories.TryGetValue(categoryName, out category))
                    {
                        category = (categories[categoryName] = new Category { Name = categoryName });
                    }

                    category.Songs.Add(new Song
                        {
                            Id = ++totalCount,
                            Artist = parts.Length == 2 ? parts[0] : "Unknown Artist",
                            Title = parts.Length == 2 ? parts[1] : name,
                            Path = file
                        });
                }
            }

            lock (_sync)
            {
                _categories = categories.OrderBy(c => c.Key, StringComparer.OrdinalIgnoreCase).Select(c => c.Value).ToList();
                _queue = new Queue<QueueItem>();
            }
        }
    }
}
