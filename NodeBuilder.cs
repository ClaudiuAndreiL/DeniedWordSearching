namespace DeniedWordSearching
{
    public class NodeBuilder
    {
        public readonly GraphNode _root = new();

        public void InsertBannedWord(string word)
        {
            var currentNode = _root;

            foreach (char ch in word)
            {
                if (Constants.SkippableChars.Contains(ch)) continue; // Skip unnecessary characters

                // Get all valid character substitutions (including original)
                var validChars = Constants.CharacterReplacements.TryGetValue(ch, out var subs)
                    ? new HashSet<string>(subs.Append(ch.ToString())) // Include original character
                    : new HashSet<string> { ch.ToString() };

                // Add node for this character substitution set
                var existingKey = currentNode.Children.Keys.FirstOrDefault(k => k.SetEquals(validChars));

                if (existingKey == null)
                {
                    // Create a new node for this character set
                    var nextNode = new GraphNode();
                    currentNode.Children[validChars] = nextNode;
                    currentNode = nextNode;
                }
                else
                {
                    // Move to the existing node
                    currentNode = currentNode.Children[existingKey];
                }
            }

            // Store the full banned word at the last node
            currentNode.DeniedSender = word;
        }

        public string? FindFirstDeniedWord(string input)
        {
            var currentNode = _root;

            foreach (char ch in input)
            {
                if (Constants.SkippableChars.Contains(ch)) continue; // Ignore skippable characters

                if (currentNode.DeniedSender != null)
                {
                    return currentNode.DeniedSender; // Found banned word
                }

                // Find the correct key in the dictionary that contains this character
                var matchingKey = currentNode.Children.Keys.FirstOrDefault(k => k.Contains(ch.ToString()));

                if (matchingKey == null)
                {
                    return null; // No match, word is safe
                }

                currentNode = currentNode.Children[matchingKey];
            }

            return currentNode.DeniedSender; // Return banned word if found at the end
        }
    }
}
