namespace DeniedWordSearching.sevents
{
    public class ExperimentalGraph2Service
    {
        private GraphNode2 _root = new();

        public bool IsValidSender(string sender)
        {
            return sender.All(c => ExperimentalConstants.Substitutions.ContainsKey(c.ToString()) || ExperimentalConstants.EmptyCharacters.Contains(c));
        }

        public void Reset()
        {
            _root = new();
        }

        public void Insert(string sender, DeniedMatchTypeEnum matchType = DeniedMatchTypeEnum.ContainsMatch)
        {
            var currentNode = _root;

            for (int i = 0; i < sender.Length; i++)
            {
                if (ExperimentalConstants.EmptyCharacters.Contains(sender[i]))
                    continue;

                if (!ExperimentalConstants.SingleCharToStringDict.TryGetValue(sender[i], out var currentCharStr))
                    continue;

                var twoCharSequence = GetTwoCharSequence(sender, i);
                if (twoCharSequence is not null)
                    i++;
                currentCharStr = twoCharSequence ?? currentCharStr;

                if (!ExperimentalConstants.Substitutions.TryGetValue(currentCharStr, out var variants))
                    continue;

                if (!currentNode!.Nodes.TryGetValue(currentCharStr, out var child))
                    child = new GraphNode2();

                foreach (var variant in variants)
                    if (!currentNode.Nodes.ContainsKey(variant))
                        currentNode.Nodes[variant] = child;

                currentNode = child;
            }

            currentNode.DeniedSender = sender;
            currentNode.MatchTypeEnum = matchType;
        }

        public List<string> GetAllItems()
        {
            HashSet<string> results = new();
            Traverse(_root, results);
            return results.ToList();
        }

        public string? Search(string sender)
        {
            return SearchRecursive(sender, 0, _root);
        }

        private string? SearchRecursive(string sender, int index, GraphNode2 currentNode)
        {
            if (index >= sender.Length)
                return currentNode.DeniedSender;

            if (ExperimentalConstants.EmptyCharacters.Contains(sender[index]))
                return SearchRecursive(sender, index + 1, currentNode);

            if (!ExperimentalConstants.SingleCharToStringDict.TryGetValue(sender[index], out var one))
                return SearchRecursive(sender, index + 1, currentNode);

            if (index + 1 < sender.Length)
            {
                var two = GetTwoCharSequence(sender, index);
                if (two != null && currentNode.Nodes.TryGetValue(two, out var node2))
                {
                    var candidateFound = EvaluateDeniedSender(sender, node2);
                    if (!string.IsNullOrEmpty(candidateFound))
                        return node2.DeniedSender;

                    var result = SearchRecursive(sender, index + 2, node2);
                    if (result != null)
                        return result;
                }
            }

            if (currentNode.Nodes.TryGetValue(one, out var node1))
            {
                var candidateFound = EvaluateDeniedSender(sender, node1);
                if (!string.IsNullOrEmpty(candidateFound))
                    return node1.DeniedSender;

                var result = SearchRecursive(sender, index + 1, node1);
                if (result != null)
                    return result;
            }

            return SearchRecursive(sender, index + 1, _root);
        }

        //public bool Remove(string sender)
        //{
        //    var path = new List<(GraphNode2 parent, string key)>();
        //    var target = FindNode(_root, sender, 0, path);
        //    if (target == null)
        //        return false;

        //    // Clear the payload
        //    target.DeniedSender = null;
        //    target.MatchTypeEnum = default;

        //    // Prune back up
        //    for (int i = path.Count - 1; i >= 0; i--)
        //    {
        //        var (parent, key) = path[i];
        //        var child = parent.Nodes[key];

        //        if (child.Nodes.Count == 0 && child.DeniedSender == null)
        //            parent.Nodes.Remove(key);
        //        else
        //            break;
        //    }

        //    return true;
        //}

        //private GraphNode2? FindNode(GraphNode2 current, string sender, int index, List<(GraphNode2 parent, string key)> path)
        //{
        //    if (index >= sender.Length)
        //        return current.DeniedSender == sender ? current : null;

        //    if (ExperimentalConstants.EmptyCharacters.Contains(sender[index]))
        //        return FindNode(current, sender, index + 1, path);

        //    // two-char branch
        //    if (index + 1 < sender.Length)
        //    {
        //        var two = GetTwoCharSequence(sender, index);
        //        if (two != null && current.Nodes.TryGetValue(two, out var node2))
        //        {
        //            path.Add((current, two));
        //            var found2 = FindNode(node2, sender, index + 2, path);
        //            if (found2 != null) return found2;
        //            path.RemoveAt(path.Count - 1);
        //        }
        //    }

        //    // single-char branch
        //    var one = sender[index].ToString();
        //    if (current.Nodes.TryGetValue(one, out var node1))
        //    {
        //        path.Add((current, one));
        //        var found1 = FindNode(node1, sender, index + 1, path);
        //        if (found1 != null) return found1;
        //        path.RemoveAt(path.Count - 1);
        //    }

        //    // fallback
        //    return FindNode(current, sender, index + 1, path);
        //}

        //private static readonly string[] _twoCharSequences =
        //    ExperimentalConstants.MultiCharSubstitutions.Keys.ToArray();

        //private static string? GetTwoCharSequence(string sender, int i)
        //{
        //    if (i + 1 >= sender.Length) return null;
        //    char c0 = sender[i]; char c1 = sender[i + 1];
        //    foreach (var seq in _twoCharSequences)
        //    {
        //        // compare chars directly
        //        if (seq[0] == c0 && seq[1] == c1)
        //            return seq;
        //    }
        //    return null;
        //}


        private static readonly Dictionary<ushort, string> _twoCharCache =
            ExperimentalConstants.MultiCharSubstitutions.Keys
                .ToDictionary(
                    k => (ushort)(k[0] << 8 | k[1]),
                    k => k);

        private static string? GetTwoCharSequence(string sender, int i)
        {
            if (i + 1 >= sender.Length)
                return null;
            ushort code = (ushort)(sender[i] << 8 | sender[i + 1]);
            return _twoCharCache.TryGetValue(code, out var seq) ? seq : null;
        }

        //private static string? GetTwoCharSequence(string sender, int i)
        //{
        //    if (i + 1 >= sender.Length) return null;
        //    var seq = string.Concat(sender[i], sender[i + 1]);
        //    return ExperimentalConstants.MultiCharSubstitutions.ContainsKey(seq) ? seq : null;
        //}

        private static string? EvaluateDeniedSender(string sender, GraphNode2 candidateNode)
        {
            if (candidateNode.DeniedSender is null)
                return null;

            if (candidateNode.MatchTypeEnum == DeniedMatchTypeEnum.ContainsMatch)
                return candidateNode.DeniedSender;

            if (candidateNode.MatchTypeEnum == DeniedMatchTypeEnum.ExactMatch)
                return sender.Equals(candidateNode.DeniedSender) ? candidateNode.DeniedSender : null;

            if (candidateNode.MatchTypeEnum == DeniedMatchTypeEnum.FuzzyMatch)
                return sender.Length == candidateNode.DeniedSender!.Length ? candidateNode.DeniedSender : null;

            return null;
        }


        private static void Traverse(GraphNode2 node, HashSet<string> results)
        {
            if (node.DeniedSender != null)
            {
                results.Add(node.DeniedSender);
            }

            foreach (var child in node.Nodes.Values)
            {
                Traverse(child, results);
            }
        }

        //private static string? GetTwoCharSequence(string sender, int i, char currentChar)
        //{
        //    if (i < sender.Length - 1)
        //    {
        //        var twoCharCandidate = string.Create(2, (currentChar, sender[i + 1]), (span, pair) =>
        //        {
        //            span[0] = pair.Item1;
        //            span[1] = pair.Item2;
        //        });
        //        if (ExperimentalConstants.MultiCharSubstitutions.ContainsKey(twoCharCandidate))
        //            return twoCharCandidate;
        //    }

        //    return null;
        //}
    }
}
