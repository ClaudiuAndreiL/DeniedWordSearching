namespace DeniedWordSearching.fourth
{
    public class Node
    {
        public Dictionary<HashSet<string>, Node> Children { get; set; } = new();
        public string? DeniedWord { get; set; } = null;
    }

}
