using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeniedWordSearching.fifth
{
    public class Node
    {
        public string? DeniedSender { get; set; } = null;
        public Dictionary<HashSet<string>, Node> Children { get; } = new();
    }
}
