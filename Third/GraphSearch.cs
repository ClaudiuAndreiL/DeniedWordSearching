namespace DeniedWordSearching.Third
{
    class Node
    {
        public HashSet<string> Key { get; }
        public Dictionary<HashSet<string>, Node> Children { get; } = new();
        public string? DeniedSender { get; set; }

        public Node(HashSet<string> key)
        {
            Key = key;
        }
    }

    class SubstitutionGraph
    {
        private readonly Node _root = new(new HashSet<string>());

        public void BuildGraph(IEnumerable<string> deniedWords, Dictionary<string, HashSet<string>> substitutions)
        {
            foreach (string word in deniedWords)
            {
                Node current = _root;
                for (int i = 0; i < word.Length; i++)
                {
                    string ch = word[i].ToString();
                    HashSet<string> keySet = substitutions.ContainsKey(ch) ? substitutions[ch] : new HashSet<string> { ch };

                    if (!current.Children.TryGetValue(keySet, out Node? nextNode))
                    {
                        nextNode = new Node(keySet);
                        current.Children[keySet] = nextNode;
                    }
                    current = nextNode;
                }
                current.DeniedSender = word;
            }
        }

        public (bool IsBlocked, string? DeniedWord) Search(string input, Dictionary<string, HashSet<string>> substitutions)
        {
            for (int start = 0; start < input.Length; start++)
            {
                var result = RecursiveSearch(_root, input, start, substitutions);
                if (result.IsBlocked)
                {
                    return result; // Return the blocked status and the denied word
                }
            }
            return (false, null); // No match found
        }

        private (bool IsBlocked, string? DeniedWord) RecursiveSearch(Node current, string input, int index, Dictionary<string, HashSet<string>> substitutions)
        {
            if (current.DeniedSender != null)
            {
                return (true, current.DeniedSender); // Found a denied word, return it
            }

            if (index >= input.Length)
            {
                return (false, null);
            }

            string ch = input[index].ToString();
            HashSet<string> keySet = substitutions.ContainsKey(ch) ? substitutions[ch] : new HashSet<string> { ch };

            foreach (var child in current.Children)
            {
                if (child.Key.Overlaps(keySet))
                {
                    var result = RecursiveSearch(child.Value, input, index + 1, substitutions);
                    if (result.IsBlocked)
                    {
                        return result; // Propagate the found word
                    }
                }
            }

            return (false, null);
        }
    }
}
