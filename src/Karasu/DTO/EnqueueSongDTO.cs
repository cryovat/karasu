using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karasu.DTO
{
    public class EnqueueSongDTO
    {
        public int SongId { get; set; }
        public bool Secret { get; set; }

        public PlayerDTO Player { get; set; }
    }
}
