using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karasu.Model
{
    public class QueueItem
    {
        public bool Secret { get; set; }

        public Song Song { get; set; }
        public Player Player { get; set; }
    }
}
