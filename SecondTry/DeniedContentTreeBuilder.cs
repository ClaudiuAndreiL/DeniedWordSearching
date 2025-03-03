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
            var currentNodes = new List<GraphTrieNode> { _root };

            for (int i = 0; i < word.Length; i++)
            {
                string charString = word[i].ToString().ToLower();
                string? multiCharString = (i + 1 < word.Length) ? charString + word[i + 1].ToString().ToLower() : null;

                if (_skippableChars.Contains(charString)) continue; // Skip empty chars

                // Get valid substitutions
                var validSubstitutions = _substitutions
                    .Where(set => set.Contains(charString) || (multiCharString != null && set.Contains(multiCharString)))
                    .ToList();

                if (!validSubstitutions.Any()) validSubstitutions.Add(new HashSet<string> { charString });

                var nextNodes = new List<GraphTrieNode>();

                foreach (var node in currentNodes)
                {
                    foreach (var subSet in validSubstitutions)
                    {
                        if (!node.Children.TryGetValue(subSet, out var nextNode))
                        {
                            nextNode = new GraphTrieNode();
                            node.Children[subSet] = nextNode;
                        }
                        nextNodes.Add(nextNode);
                    }
                }

                currentNodes = nextNodes;
            }

            // Mark as banned word
            foreach (var node in currentNodes)
            {
                node.BannedWord = word;
            }
        }

        public string? FindBannedWord(string input)
        {
            var currentNodes = new List<GraphTrieNode> { _root };
            var sanitizedString = Sanitize(input);

            Console.WriteLine("🔍 Searching for banned words in: " + input);
            Console.WriteLine("Sanitized version: " + sanitizedString);
            Console.WriteLine(new string('-', 60));

            for (int i = 0; i < sanitizedString.Length; i++)
            {
                string charString = sanitizedString[i].ToString().ToLower();
                string? multiCharString = (i + 1 < sanitizedString.Length) ? charString + sanitizedString[i + 1].ToString().ToLower() : null;

                if (_skippableChars.Contains(charString)) continue; // Skip ignored characters

                Console.WriteLine($"\n➡ Searching character index {i}: '{charString}' (multi: '{multiCharString ?? "N/A"}')");

                var nextNodes = new List<GraphTrieNode>();
                bool foundMatch = false; // Track if at least one match is found

                foreach (var node in currentNodes)
                {
                    // 🔍 Find all candidate children based on substitution sets
                    var candidateChildren = node.Children.Keys
                        .Where(set => set.Contains(charString) || (multiCharString != null && set.Contains(multiCharString)))
                        .ToList();

                    // Log candidate sets
                    Console.WriteLine($"🔎 Node has {candidateChildren.Count} candidate children:");
                    foreach (var set in candidateChildren)
                    {
                        Console.WriteLine($"   🟢 Candidate set: {{ {string.Join(", ", set)} }}");
                    }

                    // Process all matched candidates
                    foreach (var subSet in candidateChildren)
                    {
                        if (node.Children.TryGetValue(subSet, out var nextNode))
                        {
                            Console.WriteLine($"✅ Matched node for: '{charString}' (subset: {{ {string.Join(", ", subSet)} }})");

                            nextNodes.Add(nextNode);
                            foundMatch = true;

                            if (nextNode.BannedWord != null && ShouldBlockWord(sanitizedString, nextNode.BannedWord))
                            {
                                Console.WriteLine($"🚨 Found banned word match: {nextNode.BannedWord}");
                                return nextNode.BannedWord;
                            }
                        }
                    }
                }

                // 🛠️ **Fix: Keep root active only if no match found**
                if (!foundMatch)
                {
                    Console.WriteLine("⚠️ No match found, restarting from root...");
                    nextNodes.Add(_root);
                }

                currentNodes = nextNodes;
            }

            Console.WriteLine("✅ No banned words found.");
            return null;
        }



        public void PrintAllBannedWords()
        {
            Console.WriteLine("🔎 Listing all stored banned words in trie...");
            PrintTrieRecursive(_root, "");
        }

        private void PrintTrieRecursive(GraphTrieNode node, string prefix)
        {
            if (node.BannedWord != null)
            {
                Console.WriteLine($"🚨 Banned word stored: {node.BannedWord} (Path: {prefix})");
            }

            foreach (var child in node.Children)
            {
                foreach (var key in child.Key)
                {
                    PrintTrieRecursive(child.Value, prefix + key);
                }
            }
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
