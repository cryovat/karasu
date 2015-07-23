using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karasu.Model
{
    public class Song
    {
        public int Id { get; set; }

        public string Artist { get; set; }
        public string Title { get; set; }

        public string Path { get; set; }
    }
}
