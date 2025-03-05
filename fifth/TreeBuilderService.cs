using DeniedWordSearching;

public class Node
{
    public string? DeniedSender { get; set; } = null;
    public Dictionary<HashSet<string>, Node> Children { get; } = new();
}

public class TreeBuilderService
{
    private readonly Node _root = new();

    public void CreateGraph(List<string> senderIds)
    {
        senderIds.Sort((a, b) => a.Length.CompareTo(b.Length)); // Sort by length to prevent redundant inserts

        foreach (var senderId in senderIds)
        {
            if (!IsValidSender(senderId))
            {
                //Console.WriteLine($"Skipping '{senderId}': Contains invalid characters.");
                continue;
            }

            if (Search(senderId) != null)
            {
                //Console.WriteLine($"Skipping '{senderId}': Already covered by a shorter or equivalent entry.");
                continue;
            }

            InsertWord(senderId);
            //Console.WriteLine($"Inserted '{senderId}' into the graph.");
        }
    }

    private bool IsValidSender(string sender)
    {
        return sender.All(c => Constants.V5.Substitutions.ContainsKey(c.ToString()) || Constants.V5.EmptyCharacters.Contains(c));
    }

    private void InsertWord(string word)
    {
        Node current = _root;

        for (int i = 0; i < word.Length; i++)
        {
            string currentChar = word[i].ToString();

            if (Constants.V5.EmptyCharacters.Contains(word[i]))
                continue;

            // Handle multi-char cases ("vv", "rn", "nn")
            string? twoCharSequence = i < word.Length - 1 ? word.Substring(i, 2) : null;
            if (twoCharSequence is "vv" or "rn" or "nn" && Constants.V5.Substitutions.ContainsKey(twoCharSequence))
            {
                currentChar = twoCharSequence;
                i++; // Skip next character as it's part of the sequence
            }

            HashSet<string> key = new(Constants.V5.Substitutions.ContainsKey(currentChar) ? Constants.V5.Substitutions[currentChar] : new HashSet<string> { currentChar });

            if (!current.Children.TryGetValue(key, out var child))
            {
                child = new Node();
                current.Children[key] = child;
            }

            current = child;
        }

        current.DeniedSender = word;
    }

    public List<string> GetAllItems()
    {
        List<string> results = new();
        Traverse(_root, results);
        return results;
    }

    private void Traverse(Node node, List<string> results)
    {
        if (node.DeniedSender != null)
        {
            results.Add(node.DeniedSender);
        }

        foreach (var child in node.Children.Values)
        {
            Traverse(child, results);
        }
    }

    public string? Search(string input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            string? match = SearchRecursive(_root, input, i);
            if (match != null)
            {
                return match;
            }
        }
        return null;
    }

    private string? SearchRecursive(Node currentNode, string input, int index)
    {
        if (currentNode.DeniedSender != null)
        {
            return ValidateMatch(currentNode.DeniedSender, input);
        }

        if (index >= input.Length)
        {
            return null;
        }

        char currentChar = input[index];
        if (Constants.V5.EmptyCharacters.Contains(currentChar))
        {
            return SearchRecursive(currentNode, input, index + 1);
        }

        string currentCharStr = currentChar.ToString();

        // Handle "vv", "rn", "nn" sequences
        string? twoCharSequence = index < input.Length - 1 ? input.Substring(index, 2) : null;
        if (twoCharSequence is "vv" or "rn" or "nn" && Constants.V5.Substitutions.ContainsKey(twoCharSequence))
        {
            currentCharStr = twoCharSequence;
            index++; // Skip next character as it's part of the sequence
        }

        HashSet<string> keySet = new(Constants.V5.Substitutions.ContainsKey(currentCharStr) ? Constants.V5.Substitutions[currentCharStr] : new HashSet<string> { currentCharStr });

        foreach (var (key, child) in currentNode.Children)
        {
            if (key.Overlaps(keySet))
            {
                string? match = SearchRecursive(child, input, index + 1);
                if (match != null)
                {
                    return match;
                }
            }
        }

        return null;
    }

    private string? ValidateMatch(string deniedSender, string input)
    {
        return deniedSender.Length <= 2 ? (deniedSender == input ? deniedSender : null) : deniedSender;
    }
}