using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeniedWordSearching
{
    public class GraphNode
    {
        public string? DeniedSender { get; set; }
        public HashSet<string> CharacterSubstitutions { get; set; } = new();
        public Dictionary<HashSet<string>, GraphNode> Children { get; set; } = new();
    }
}
