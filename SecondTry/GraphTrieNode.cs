namespace DeniedWordSearching.SecondTry
{
    public class GraphTrieNode
    {
        public Dictionary<HashSet<string>, GraphTrieNode> Children { get; } = new();
        public string? BannedWord { get; set; }  // Stores the denied word at the end node
        public HashSet<string> CharacterSubstitutions { get; } = new();
        public List<GraphTrieNode> BacktrackNodes { get; } = new(); // For restarting on match failures
    }
}
