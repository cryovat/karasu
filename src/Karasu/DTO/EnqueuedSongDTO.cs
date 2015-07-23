using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karasu.DTO
{
    public class EnqueuedSongDTO
    {
        public bool Secret { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }

        public PlayerDTO Player { get; set; }
    }


}
