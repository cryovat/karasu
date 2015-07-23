using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Karasu.Model;
using Karasu.Repositories;

namespace Karasu.Instrumentation
{
    public class SongRunner
    {
        private readonly ISongRepository _songRepository;
        private readonly ISettingsRepository _settingsRepository;

        private int _started;
        private bool _running = true;

        private Process _process;

        public QueueItem CurrentSong { get; private set; }

        public SongRunner(ISongRepository songRepository, ISettingsRepository settingsRepository)
        {
            _songRepository = songRepository;
            _settingsRepository = settingsRepository;
        }

        public bool RestartCurrent()
        {
            var process = _process;
            if (process == null) return false;

            try
            {
                process.StandardInput.WriteLine("seek 0 2");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool SkipCurrent()
        {
            var process = _process;
            if (process == null) return false;

            try
            {
                process.StandardInput.WriteLine("quit");

                Thread.Sleep(200);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Start()
        {
            if (Interlocked.CompareExchange(ref _started, 1, 0) == 1)
            {
                throw new InvalidOperationException("Runner already started.");
            }

            ThreadPool.QueueUserWorkItem(async v =>
            {
                while (_running)
                {
                    var current = _songRepository.PopQueue();

                    if (current == null)
                    {
                        CurrentSong = null;
                        await Task.Delay(100);
                        continue;
                    }

                    CurrentSong = current;

                    var path = _settingsRepository.MplayerPath;

                    if (!File.Exists(path)) continue;

                    var pssi = new ProcessStartInfo(_settingsRepository.MplayerPath)
                    {
                        Arguments = $"-slave -quiet \"{current.Song.Path}\" -fs",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardInput = true
                    };

                    _process = Process.Start(pssi);

                    if (_process == null) continue;

                    _process.WaitForExit();

                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }

            });


        }
    }
}
