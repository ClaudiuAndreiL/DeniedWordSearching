using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeniedWordSearching.fourth
{
    public class Node
    {
        public Dictionary<HashSet<string>, Node> Children { get; set; } = new();
        public string? DeniedWord { get; set; } = null;
    }

}
