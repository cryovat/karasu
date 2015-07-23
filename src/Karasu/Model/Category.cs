using System.Collections.Generic;

namespace Karasu.Model
{
    public class Category
    {
        public string Name { get; set; }
        public ICollection<Song> Songs { get; }

        public Category()
        {
            Songs = new List<Song>();
        }
    }
}
