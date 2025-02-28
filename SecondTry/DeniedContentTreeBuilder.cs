namespace DeniedWordSearching.SecondTry
{

    public class BannedWordGraph
    {
        private readonly GraphTrieNode _root = new();
        private readonly List<HashSet<string>> _substitutions;
        private readonly HashSet<string> _skippableChars;

        public BannedWordGraph(List<HashSet<string>> substitutions, HashSet<string> skippableChars)
        {
            _substitutions = substitutions;
            _skippableChars = skippableChars;
        }

        // **Prevents duplicates by checking if the word already exists**
        public void AddBannedWord(string word)
        {
            var currentNode = _root;

            for (int i = 0; i < word.Length; i++)
            {
                // Default to single-character substitution
                string charString = word[i].ToString().ToLower();

                if (_skippableChars.Contains(charString)) continue; // Skip empty-space chars

                // Look ahead: Try multi-character substitution
                if (i + 1 < word.Length)
                {
                    string multiCharString = charString + word[i + 1].ToString().ToLower();
                    if (_substitutions.Exists(set => set.Contains(multiCharString)))
                    {  
                        charString = multiCharString; // Use multi-character substitution
                        i++; // Skip the next character
                    }
                }

                // Fetch the substitution set or create a new one if missing
                var substitutionSet = _substitutions.FirstOrDefault(set => set.Contains(charString))
                                      ?? [charString];

                if (!currentNode.Children.TryGetValue(substitutionSet, out var nextNode))
                {
                    nextNode = new GraphTrieNode();
                    currentNode.Children[substitutionSet] = nextNode;
                }

                nextNode.CharacterSubstitutions.UnionWith(substitutionSet);
                currentNode = nextNode;
            }


            // **Check if the word was already inserted**
            if (currentNode.BannedWord == null)
            {
                currentNode.BannedWord = word;  // Mark word at its final node
            }
        }

        // **Search function with backtracking**
        public string? FindBannedWord(string input)
        {
            var currentNodes = new List<GraphTrieNode> { _root };

            var sanitizedString = Sanitize(input);

            foreach (char c in input)
            {
                string charString = c.ToString().ToLower();
                if (_skippableChars.Contains(charString)) continue; // Skip ignored characters

                var nextNodes = new List<GraphTrieNode>();
                var substitutionSet = _substitutions.FirstOrDefault(set => set.Contains(charString))
                                      ?? [charString];

                foreach (var node in currentNodes)
                {
                    if (node.Children.TryGetValue(substitutionSet, out var nextNode))
                    {
                        nextNodes.Add(nextNode);
                        if (nextNode.BannedWord != null && ShouldBlockWord(sanitizedString, nextNode.BannedWord)) 
                            return nextNode.BannedWord; // Found match
                    }

                    foreach (var backtrackNode in node.BacktrackNodes)
                    {
                        if (backtrackNode.Children.TryGetValue(substitutionSet, out var backtrackNext))
                        {
                            nextNodes.Add(backtrackNext);
                            if (backtrackNext.BannedWord != null && ShouldBlockWord(sanitizedString, backtrackNext.BannedWord)) 
                                return backtrackNext.BannedWord;
                        }
                    }
                }

                currentNodes = nextNodes.Count > 0 ? nextNodes : new List<GraphTrieNode> { _root };
            }

            return null;
        }

        private bool ShouldBlockWord(string sanitized, string bannedWord)
        {
            return bannedWord.Length > 3 || sanitized.Length == bannedWord.Length;
        }

        private string Sanitize(string input)
        {
            return new string(input.Where(c => !Constants.V2.EmptyLookingCharacters.Contains(c.ToString())).ToArray());
        }

        public List<string> GetAllBannedWords()
        {
            var bannedWords = new List<string>();
            TraverseGraph(_root, bannedWords);
            return bannedWords;
        }

        private void TraverseGraph(GraphTrieNode node, List<string> bannedWords)
        {
            if (node.BannedWord != null)
            {
                bannedWords.Add(node.BannedWord);
            }

            foreach (var child in node.Children.Values)
            {
                TraverseGraph(child, bannedWords);
            }
        }

    }


}
