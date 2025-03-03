using System;
using DeniedWordSearching;
using System.Collections.Generic;
using System.Linq;

public class TreeNode
{
    public Dictionary<string, TreeNode> Children { get; } = new();
    public string? DeniedSender { get; set; } = null;
}

public class TreeBuilder
{
    private readonly TreeNode _root = new();
    private static readonly HashSet<string> MultiCharSubstitutions = new() { "rn", "nn" };

    public bool Insert(string input)
    {
        string sanitizedInput = Sanitize(input);
        bool result = InsertRecursive(_root, sanitizedInput, 0, sanitizedInput);
        Console.WriteLine(result ? $"Inserted: {sanitizedInput}" : $"Skipped: {sanitizedInput}");
        return result;
    }

    private bool InsertRecursive(TreeNode node, string original, int index, string remaining)
    {
        if (index >= remaining.Length)
        {
            if (node.DeniedSender != null)
            {
                Console.WriteLine($"Skipping {original}, already exists as {node.DeniedSender}");
                return false;
            }
            node.DeniedSender = original;
            Console.WriteLine($"Added denied sender: {original}");
            return true;
        }

        string currentSegment = GetValidSegment(remaining, index);
        HashSet<string>? matchingSet = Constants.V2.SubstitutionSets.FirstOrDefault(set => set.Contains(currentSegment));

        if (matchingSet == null)
        {
            throw new ArgumentException($"Invalid character '{remaining[index]}' found in input.");
        }

        string key = string.Join(",", matchingSet.OrderBy(c => c));
        if (!node.Children.ContainsKey(key))
        {
            node.Children[key] = new TreeNode();
        }

        if (GetFirstDeniedSenderMatch(original) != null)
        {
            Console.WriteLine($"Skipping {original}, conflicts with existing denied sender");
            return false;
        }

        return InsertRecursive(node.Children[key], original, index + currentSegment.Length, remaining);
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
        List<string> attemptedPaths = new List<string>();

        for (int i = 0; i < sanitizedInput.Length; i++)
        {
            List<string> path = new List<string>();
            string? match = SearchForFirstMatch(_root, sanitizedInput, i, path);
            if (match != null)
            {
                return match;
            }
            attemptedPaths.Add("{ " + string.Join(" } -> { ", path) + " }");
        }

        Console.WriteLine($"No denied sender found. Paths attempted:\n{string.Join("\n", attemptedPaths)}");
        return null;
    }

    private string? SearchForFirstMatch(TreeNode node, string input, int index, List<string> path)
    {
        if (node == null) return null;
        path.Add("(Root)");
        TreeNode currentNode = node;
        while (index < input.Length)
        {
            string currentSegment = GetValidSegment(input, index);
            HashSet<string>? matchingSet = Constants.V2.SubstitutionSets.FirstOrDefault(set => set.Contains(currentSegment));

            if (matchingSet == null)
            {
                Console.WriteLine($"No match found for '{input[index]}' at index {index}. Path attempted: {{ {string.Join(" } -> { ", path)} }}");
                return null;
            }

            string key = string.Join(",", matchingSet.OrderBy(c => c));
            if (!currentNode.Children.ContainsKey(key))
            {
                Console.WriteLine($"No match found for '{input[index]}' at index {index}. Expected key: {key}. Path attempted: {{ {string.Join(" } -> { ", path)} }}");
                return null;
            }

            path.Add("{ " + string.Join(", ", matchingSet) + " }");
            Console.WriteLine($"Moving to child node with key: {key}");
            currentNode = currentNode.Children[key];
            if (currentNode.DeniedSender != null)
            {
                if (currentNode.DeniedSender.Length > 3 || currentNode.DeniedSender.Length == input.Length)
                {
                    Console.WriteLine($"Found denied sender '{currentNode.DeniedSender}' on path: {string.Join(" -> ", path)}");
                    return currentNode.DeniedSender;
                }
            }
            index += currentSegment.Length;
        }
        return null;
    }

    public void PrintTree()
    {
        PrintTreeRecursive(_root, "");
    }

    private void PrintTreeRecursive(TreeNode node, string prefix)
    {
        if (node == null) return;
        if (node.DeniedSender != null)
        {
            Console.WriteLine(prefix + " -> Denied Sender: " + node.DeniedSender);
        }

        foreach (var child in node.Children)
        {
            Console.WriteLine(prefix + "-> " + child.Key);
            PrintTreeRecursive(child.Value, prefix + "  ");
        }
    }
}