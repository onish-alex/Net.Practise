using System.Collections.Generic;

namespace Linq2
{
    public class Film : ArtObject
    {
        public int Length { get; set; }

        public IEnumerable<Actor> Actors { get; set; }
    }
}
