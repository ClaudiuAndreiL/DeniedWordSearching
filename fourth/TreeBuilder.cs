﻿using DeniedWordSearching;

public class TreeNode
{
    public Dictionary<HashSet<string>, TreeNode> Children { get; } = new();
    public string? DeniedSender { get; set; } = null;
}

public class TreeBuilder
{
    private readonly TreeNode _root = new();
    private static readonly HashSet<string> MultiCharSubstitutions = new() { "rn", "nn" };

    public bool Insert(string input)
    {
        var alreadyExists = GetFirstDeniedSenderMatch(input);
        if (alreadyExists is not null)
        {
            //Console.Writeline("ALREADY EXISTS {0} as {1}", input, alreadyExists);
            return false;
        }

        string sanitizedInput = Sanitize(input);
        var path = new List<HashSet<string>>();
        bool result = InsertRecursive(_root, sanitizedInput, 0, sanitizedInput, path);

        //Console.Writeline(result ? $"Inserted successfully. Path: {FormatPath(path)}" : "Insertion skipped.");
        return result;
    }

    private bool InsertRecursive(TreeNode node, string original, int index, string remaining, List<HashSet<string>> path)
    {
        if (index >= remaining.Length)
        {
            if (node.DeniedSender != null)
            {
                //Console.Writeline($"Skipping {original}, already exists as {node.DeniedSender}");
                return false;
            }
            node.DeniedSender = original;
            //Console.Writeline($"Added denied sender: {original}");
            return true;
        }

        string currentSegment = GetValidSegment(remaining, index);
        HashSet<string>? matchingSet = Constants.V2.SubstitutionSets.FirstOrDefault(set => set.Contains(currentSegment));

        if (matchingSet == null)
        {
            throw new ArgumentException($"Invalid character '{remaining[index]}' found in input.");
        }

        path.Add(matchingSet);
        if (!node.Children.ContainsKey(matchingSet))
        {
            node.Children[matchingSet] = new TreeNode();
        }

        return InsertRecursive(node.Children[matchingSet], original, index + currentSegment.Length, remaining, path);
    }

    private string GetValidSegment(string input, int index)
    {
        if (index + 1 < input.Length && MultiCharSubstitutions.Contains(input.Substring(index, 2)))
        {
            return input.Substring(index, 2);
        }
        return input[index].ToString();
    }

    private string Sanitize(string input)
    {
        return new string(input.Where(c => !Constants.V2.EmptyLookingCharacters.Contains(c.ToString())).ToArray());
    }

    public string? GetFirstDeniedSenderMatch(string input)
    {
        string sanitizedInput = Sanitize(input);
        //Console.Writeline($"Searching: {sanitizedInput}");
        List<HashSet<string>> path = new List<HashSet<string>>();
        for (int i = 0; i < sanitizedInput.Length; i++)
        {
            path.Clear();
            string? match = SearchForFirstMatch(_root, sanitizedInput, i, path);
            if (match != null)
            {
                //Console.Writeline($"Found denied sender: {match}. Path: {FormatPath(path)}");
                return match;
            }
        }
        //Console.Writeline("No denied sender found.");
        return null;
    }

    private string? SearchForFirstMatch(TreeNode node, string input, int index, List<HashSet<string>> path)
    {
        if (node == null) return null;
        TreeNode currentNode = node;
        while (index < input.Length)
        {
            string currentSegment = GetValidSegment(input, index);
            HashSet<string>? matchingSet = Constants.V2.SubstitutionSets.FirstOrDefault(set => set.Contains(currentSegment));

            if (matchingSet == null || !currentNode.Children.ContainsKey(matchingSet))
            {
                return null;
            }

            path.Add(matchingSet);
            currentNode = currentNode.Children[matchingSet];
            if (currentNode.DeniedSender != null)
            {
                if (currentNode.DeniedSender.Length > 3 || currentNode.DeniedSender.Length == input.Length)
                {
                    return currentNode.DeniedSender;
                }
            }
            index += currentSegment.Length;
        }
        return null;
    }

    private string FormatPath(List<HashSet<string>> path)
    {
        return string.Join(" -> ", path.Select(set => "{ " + string.Join(", ", set) + " }"));
    }

    public void PrintTree()
    {
        //Console.Writeline(Environment.NewLine);
        //Console.Writeline("-------------------------Printing tree-------------------------");
        PrintTreeRecursive(_root, "");
        //Console.Writeline("-------------------------End of tree-------------------------");
        //Console.Writeline(Environment.NewLine);
    }

    private void PrintTreeRecursive(TreeNode node, string prefix)
    {
        if (node == null) return;
        if (node.DeniedSender != null)
        {
            //Console.Writeline(prefix + " -> Denied Sender: " + node.DeniedSender);
        }

        foreach (var child in node.Children)
        {
            //Console.Writeline(prefix + "-> " + FormatPath(new List<HashSet<string>> { child.Key }));
            PrintTreeRecursive(child.Value, prefix + "  ");
        }
    }
}
