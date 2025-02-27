namespace DeniedWordSearching
{
    public static class Constants
    {
        public static class V2
        {
            public static HashSet<string> EmptyLookingCharacters = new HashSet<string>
            {
                " ", "_", "-", ".", ",", "=", "+", "~", "`", "'", "\"", "|", "/", "\\"
            };

            public static List<HashSet<string>> SubstitutionSets = new List<HashSet<string>>
            {
                new() { "a", "4", "@" },
                new() { "b", "8", "ß" },
                new() { "c", "(", "¢", "©" },
                new() { "d", "ð" },
                new() { "e", "3", "€", "ë", "è", "é", "ê", "æ" },
                new() { "f", "ƒ" },
                new() { "g", "6", "9" },
                new() { "h", "#" },
                new() { "l", "i", "1", "|", "!", "£", "¡", "í", "ì" }, // <- MERGED
                new() { "j", "ʝ" },
                new() { "k", "κ", "к" },
                new() { "m", "rn", "м" }, // <- KEPT
                new() { "n", "η", "ñ" },
                new() { "o", "0", "°", "ö", "ø", "ð", "ó", "q", "9" }, // <- MERGED
                new() { "p", "ρ", "þ" },
                new() { "r", "я" },
                new() { "s", "5", "$" }, // <- REMOVED §
                new() { "t", "7", "+" },
                new() { "u", "ü", "ù", "µ" },
                new() { "v", "∨" },
                new() { "w", "vv", "ω" },
                new() { "x", "×", "χ" },
                new() { "y", "¥", "γ" },
                new() { "z", "2", "ƶ" }
            };


            //public static List<HashSet<string>> SubstitutionSets = new List<HashSet<string>>
            //{
            //    new() { "a", "4", "@" },
            //    new() { "b", "8", "ß" },
            //    new() { "c", "(", "¢", "©" },
            //    new() { "d", "ð" },
            //    new() { "e", "3", "€", "ë", "è", "é", "ê", "æ" },
            //    new() { "f", "ƒ" },
            //    new() { "g", "6", "9" },
            //    new() { "h", "#" },
            //    new() { "i", "1", "!", "|", "¡", "í", "ì" },
            //    new() { "j", "ʝ" },
            //    new() { "k", "κ", "к" },
            //    new() { "l", "1", "|", "£", "!" },
            //    new() { "m", "rn", "м" },
            //    new() { "n", "η", "ñ" },
            //    new() { "o", "0", "°", "ö", "ø", "ð", "ó" },
            //    new() { "p", "ρ", "þ" },
            //    new() { "q", "9" },
            //    new() { "r", "я" },
            //    new() { "s", "5", "$", "§" },
            //    new() { "t", "7", "+" },
            //    new() { "u", "ü", "ù", "µ" },
            //    new() { "v", "∨" },
            //    new() { "w", "vv", "ω" },
            //    new() { "x", "×", "χ" },
            //    new() { "y", "¥", "γ" },
            //    new() { "z", "2", "ƶ" }
            //};
        }

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
