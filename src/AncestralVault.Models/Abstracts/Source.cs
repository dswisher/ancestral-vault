
using System.Collections.Generic;

namespace AncestralVault.Models.Abstracts
{
    public class Source
    {
        public Citation Citation { get; set; }
        public IList<Event> Events { get; set; }
    }
}

