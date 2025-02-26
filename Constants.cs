namespace DeniedWordSearching
{
    public static class Constants
    {
        public static List<char> SkippableChars = new() { ' ', '-', '_', '+', '=', '.' };

        public static Dictionary<char, string[]> CharacterReplacements = new()
        {   //inf0, 1nnf0
            { 'a', new[] { "4", "@" } },
            { 'b', new[] { "8" } },
            { 'c', new[] { "(", "{" } },
            { 'd', new[] { "|)" } },
            { 'e', new[] { "3" } },
            { 'f', new[] { "ph" } },
            { 'g', new[] { "6", "9" } },
            { 'h', new[] { "#" } },
            { 'i', new[] { "1", "!", "|" } },
            { 'j', new[] { "_|" } },
            { 'k', new[] { "|<" } },
            { 'l', new[] { "1", "|" } },
            { 'm', new[] { "rn", "nn" } },
            { 'n', new[] { "nn" } },
            { 'o', new[] { "0" } },
            { 'p', new[] { "|o" } },
            { 'q', new[] { "0_" } },
            { 'r', new[] { "|2" } },
            { 's', new[] { "5", "$" } },
            { 't', new[] { "7", "+" } },
            { 'u', new[] { "v" } },
            { 'v', new[] { "\\/" } },
            { 'w', new[] { "vv", "uu", "\\/\\/" } },
            { 'x', new[] { "><" } },
            { 'y', new[] { "'/" } },
            { 'z', new[] { "2" } }
        };
    }
}
