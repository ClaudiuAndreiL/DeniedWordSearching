namespace DeniedWordSearching.Third
{
    public class SubstitutionGenerator
    {
        public static Dictionary<string, HashSet<string>> GetFullSubstitutions()
        {
            // Base substitutions (direct mappings)
            Dictionary<string, HashSet<string>> baseSubstitutions = new()
        {
            { "0", new HashSet<string> { "O", "Q", "D" } },
            { "1", new HashSet<string> { "I", "l" } },
            { "2", new HashSet<string> { "Z", "7" } },
            { "3", new HashSet<string> { "E" } },
            { "4", new HashSet<string> { "H", "A", "@" } },
            { "5", new HashSet<string> { "S" } },
            { "6", new HashSet<string> { "G", "B" } },
            { "7", new HashSet<string> { "T", "Z" } },
            { "8", new HashSet<string> { "B", "X" } },
            { "9", new HashSet<string> { "G", "P" } },
            { "a", new HashSet<string> { "4" } },
            { "A", new HashSet<string> { "4", "@" } },
            { "b", new HashSet<string> { "8" } },
            { "B", new HashSet<string> { "8", "6" } },
            { "e", new HashSet<string> { "3" } },
            { "E", new HashSet<string> { "3" } },
            { "g", new HashSet<string> { "6", "9" } },
            { "G", new HashSet<string> { "6", "9" } },
            { "h", new HashSet<string> { "4" } },
            { "H", new HashSet<string> { "4" } },
            { "i", new HashSet<string> { "1", "l" } },
            { "I", new HashSet<string> { "1", "l" } },
            { "j", new HashSet<string> { "l" } },
            { "J", new HashSet<string> { "I" } },
            { "k", new HashSet<string> { "x" } },
            { "K", new HashSet<string> { "X" } },
            { "l", new HashSet<string> { "1", "i" } },
            { "L", new HashSet<string> { "1", "I" } },
            { "m", new HashSet<string> { "nn", "rn" } },
            { "o", new HashSet<string> { "0", "q", "d" } },
            { "O", new HashSet<string> { "0", "Q", "D" } },
            { "P", new HashSet<string> { "p" } },
            { "Q", new HashSet<string> { "O", "D" } },
            { "s", new HashSet<string> { "5" } },
            { "S", new HashSet<string> { "5" } },
            { "t", new HashSet<string> { "7" } },
            { "T", new HashSet<string> { "7" } },
            { "u", new HashSet<string> { "v" } },
            { "U", new HashSet<string> { "V" } },
            { "v", new HashSet<string> { "u" } },
            { "V", new HashSet<string> { "U" } },
            { "x", new HashSet<string> { "k" } },
            { "X", new HashSet<string> { "K" } },
            { "z", new HashSet<string> { "2", "7" } },
            { "Z", new HashSet<string> { "2", "7" } },
            // Special characters (direct mappings)
            { "@", new HashSet<string> { "A", "4" } },
            { "€", new HashSet<string> { "E", "3" } },
            { "$", new HashSet<string> { "S", "5" } },
        };

            return ComputeFullSubstitutions(baseSubstitutions);
        }

        private static Dictionary<string, HashSet<string>> ComputeFullSubstitutions(Dictionary<string, HashSet<string>> baseSubstitutions)
        {
            var expanded = new Dictionary<string, HashSet<string>>();

            foreach (var kvp in baseSubstitutions)
            {
                var key = kvp.Key;
                if (!expanded.ContainsKey(key))
                    expanded[key] = new HashSet<string>();

                ExpandSubstitutions(key, baseSubstitutions, expanded[key], new HashSet<string>());
            }

            return expanded;
        }

        private static void ExpandSubstitutions(string key, Dictionary<string, HashSet<string>> baseSubstitutions, HashSet<string> result, HashSet<string> visited)
        {
            if (visited.Contains(key)) return;
            visited.Add(key);
            result.Add(key);

            if (baseSubstitutions.TryGetValue(key, out var subs))
            {
                foreach (var sub in subs)
                {
                    ExpandSubstitutions(sub, baseSubstitutions, result, visited);
                }
            }
        }

        public static void PrintFullSubstitutions(Dictionary<string, HashSet<string>> substitutions)
        {
            foreach (var kvp in substitutions)
            {
                Console.WriteLine($"{kvp.Key} -> {{ {string.Join(", ", kvp.Value)} }}");
            }
        }
    }
}
