namespace DeniedWordSearching
{
    public static class Constants
    {
        public static class V2
        {
            public static HashSet<string> EmptyLookingCharacters = new HashSet<string>
            {
                " ", "_", ".", "-"
            };

            // Needs A-Z0-9 + special characters ( \s , . @ £ $ _ - ^ { } \ / [ ] ( )~ I € ! " % & ' * + < >= ?   )
            public static List<HashSet<string>> SubstitutionSets = new()
            {
                new() { "0", "O", "Q", "o", "q" },
                new() { "!", "1", "I", "i", "l", "L" },
                new() { "2", "Z", "z" },
                new() { "3", "E", "e", "€" },
                new() { "4", "@", "A", "a" },
                new() { "$", "5", "S", "s" },
                new() { "&", "6", "G", "g" },
                new() { "+", "7", "T", "t" },
                new() { "%", "8", "B", "b" },
                new() { "9", "P", "p" },
                new() { "(", "C", "c" },
                new() { "D", "d" },
                new() { "F", "f" },
                new() { "#", "H", "h" },
                new() { ";", "J", "j" },
                new() { "K", "X", "k", "x" },
                new() { "M", "m", "nn", "rn" },
                new() { "N", "n" },
                new() { "R", "r" },
                new() { "U", "V", "u", "v" },
                new() { "W", "vv", "w" },
                new() { "Y", "y" },
                new() { "^" },
                new() { "{", "}" },
                new() { "[", "]" },
                new() { "(", ")" },
                new() { "<", ">" },
                new() { ":", "=" },
                new() { "\"", "'", "`" },
                new() { "/", "\\" },
                new() { "£" },
                new() { "?" },
                new() { "*" },
                new() { "~" },
                new() { "," },
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
