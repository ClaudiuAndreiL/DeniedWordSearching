namespace DeniedWordSearching.sevents
{
    public class GraphNode2
    {
        public string? DeniedSender;
        public DeniedMatchTypeEnum MatchTypeEnum;
        public Dictionary<string, GraphNode2> Nodes = new();
    }
}
