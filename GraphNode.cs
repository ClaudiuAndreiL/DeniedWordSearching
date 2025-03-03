namespace DeniedWordSearching
{
    public class GraphNode
    {
        public string? DeniedSender { get; set; }
        public HashSet<string> CharacterSubstitutions { get; set; } = new();
        public Dictionary<HashSet<string>, GraphNode> Children { get; set; } = new();
    }
}
