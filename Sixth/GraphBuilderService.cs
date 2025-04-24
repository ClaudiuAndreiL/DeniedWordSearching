namespace DeniedWordSearching.Sixth
{
    public class GraphBuilderService
    {
        private readonly GraphNode _root = new();

        public bool IsValidSender(string sender)
        {
            return sender.All(c => Constants.V5.Substitutions.ContainsKey(c.ToString()) || Constants.V5.EmptyCharacters.Contains(c));
        }

        /// <summary>
        /// 1. ordering by length handled separately, ahead
        /// - this ensures that when doing validation ahead of calling insert for a string, 
        /// it may return info that word already inserted in a smaller form 
        /// and will not be needed to add again
        /// e.g. ing -> playing (if ing is blocked, then playing is also blocked
        /// 
        /// 2. checking whether already exists handled separately, ahead
        /// - given that items will be added from smallest length to highest, 
        /// checking before adding anything will ensure that we'll have a 
        /// minimum number of items in the graph and that any input when searching 
        /// will result exactly zero or one match, never more 
        /// - before each insert we'll do a search returning whether a variation already exists to not insert again.
        /// 
        /// validation against unknown chars is also done ahead
        /// - no more issues checking and handling keys not matching when doing unsafe dictionary/hashset access via key dict[key]
        /// </summary>
        /// <param name="sender"></param>
        public void Insert(string sender, DeniedMatchTypeEnum matchType = DeniedMatchTypeEnum.ContainsMatch)
        {
            var currentNode = _root;

            for(int i = 0; i < sender.Length; i++)
            {
                if (Constants.V5.EmptyCharacters.Contains(sender[i]))
                    continue;

                if (!Constants.V5.Substitutions.ContainsKey(sender[i].ToString())) // this should be prevented, not even needed via validation
                    continue;

                string currentCharStr;
                var currentChar = sender[i];
                var twoCharSequence = GetTwoCharSequence(sender, i, currentChar);
                if(twoCharSequence is not null)
                    i++;
                currentCharStr = twoCharSequence ?? currentChar.ToString();

                var key = Constants.V5.Substitutions[currentCharStr];
                if (!currentNode!.Nodes.TryGetValue(key, out var child))
                {
                    child = new GraphNode();
                    currentNode.Nodes[key] = child;
                }

                currentNode = child;
            }

            currentNode.DeniedSender = sender;
            currentNode.MatchTypeEnum = matchType;
        }

        public List<string> GetAllItems()
        {
            List<string> results = new();
            Traverse(_root, results);
            return results;
        }

        public string? Search(string sender)
        {
            return SearchRecursive(sender, 0, _root);
        }

        private string? SearchRecursive(string sender, int index, GraphNode currentNode)
        {
            if (index >= sender.Length)
                return currentNode.DeniedSender;

            if (Constants.V5.EmptyCharacters.Contains(sender[index]))
                return SearchRecursive(sender, index + 1, currentNode);

            char currentChar = sender[index];
            string currentCharStr = currentChar.ToString();
            var twoCharSequence = GetTwoCharSequence(sender, index, sender[index]);

            var candidateKeys = new List<string>
            {
                // Add substitutions for current single character            
                currentCharStr
            };

            // Add substitutions for two-char sequences
            if (twoCharSequence is not null)
            {
                candidateKeys.Add(twoCharSequence);
            }

            foreach (var key in candidateKeys)
            {
                //foreach(var candidateNodeKvp in currentNode.Nodes)
                //{
                //    if (!candidateNodeKvp.Key.Contains(key))
                //        continue;

                //    var candidateFound = EvaluateDeniedSender(sender, candidateNodeKvp.Value);
                //    if (!string.IsNullOrEmpty(candidateFound))
                //        return candidateNodeKvp.Value.DeniedSender;

                //    var nextIndex = key.Length == 2 ? index + 2 : index + 1;
                //    var result = SearchRecursive(sender, nextIndex, candidateNodeKvp.Value);
                //    if (result != null)
                //        return result;
                //}

                foreach (var candidateNodeKvp in currentNode.Nodes.Where(x => x.Key.Contains(key)))
                {
                    // evaluate that we found a match
                    var candidateFound = EvaluateDeniedSender(sender, candidateNodeKvp.Value);
                    if (!string.IsNullOrEmpty(candidateFound))
                        return candidateNodeKvp.Value.DeniedSender;

                    var nextIndex = key.Length == 2 ? index + 2 : index + 1;
                    var result = SearchRecursive(sender, nextIndex, candidateNodeKvp.Value);
                    if (result != null)
                        return result;
                }
            }

            return SearchRecursive(sender, index + 1, _root);
        }

        private static string? EvaluateDeniedSender(string sender, GraphNode candidateNode)
        {
            if (candidateNode.DeniedSender is null)
                return null;

            if(candidateNode.MatchTypeEnum == DeniedMatchTypeEnum.ContainsMatch)
                return candidateNode.DeniedSender;

            if(candidateNode.MatchTypeEnum == DeniedMatchTypeEnum.ExactMatch)
                return sender.Equals(candidateNode.DeniedSender) ? candidateNode.DeniedSender : null;

            if(candidateNode.MatchTypeEnum == DeniedMatchTypeEnum.FuzzyMatch)
                return sender.Length == candidateNode.DeniedSender!.Length ? candidateNode.DeniedSender : null;

            return null;
        }


        private static void Traverse(GraphNode node, List<string> results)
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

        private static string? GetTwoCharSequence(string sender, int i, char currentChar)
        {
            if (i < sender.Length - 1)
            {
                var twoCharCandidate = string.Create(2, (currentChar, sender[i + 1]), (span, pair) =>
                {
                    span[0] = pair.Item1;
                    span[1] = pair.Item2;
                });
                if(Constants.V5.MultiCharSubstitutions.ContainsKey(twoCharCandidate))
                    return twoCharCandidate;
            }

            return null;
        }
    }
}
