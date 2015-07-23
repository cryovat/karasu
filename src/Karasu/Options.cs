using System.Collections.Generic;

using CommandLine;

namespace Karasu
{
    public class Options
    {
        [Option("mplayer-path", HelpText = "Absolute path to Mplayer executable")]
        public string MplayerExecutablePath { get; set; }

        [Option("app-path", HelpText = "Absolute path to folder containing web app")]
        public string WebAppPath { get; set; }

        [Option("library-paths", HelpText = "One or more absolute paths to folders containing songs")]
        public IList<string> LibraryPath { get; set; }
    }
}
