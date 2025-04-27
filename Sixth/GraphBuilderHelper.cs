using DeniedWordSearching.sevents;
using System.Diagnostics;

namespace DeniedWordSearching.Sixth
{
    public class GraphBuilderHelper
    {
        //private readonly GraphBuilderService _graphBuilderService = new();

        private readonly ExperimentalGraph2Service _graphBuilderService = new();

        public GraphBuilderHelper() { }

        public void InsertMultiple(List<string> senders)
        {
            var sw = Stopwatch.StartNew();
            
            var invalid = senders.Where(x => !_graphBuilderService.IsValidSender(x)).ToList();
            var valid = senders.Except(invalid).ToList();
            Console.WriteLine("Valid {0}  |  Invalid {1}. TOOK  {2} ms.", valid.Count, invalid.Count, sw.Elapsed.TotalMilliseconds);

            var toInsert = valid.OrderBy(x => x).ToList();

            sw.Restart();
            int alreadyInserted = 0;
            int inserted = 0;

            foreach (var sender in toInsert)
            {
                var exists = !string.IsNullOrEmpty(_graphBuilderService.Search(sender));
                if (exists)
                {
                    alreadyInserted++;
                    continue;
                }

                var type = sender.Length < 3 ? DeniedMatchTypeEnum.ExactMatch : sender.Length == 3 ? DeniedMatchTypeEnum.FuzzyMatch : DeniedMatchTypeEnum.ContainsMatch;

                inserted++;
                _graphBuilderService.Insert(sender);
                //_graphBuilderService.Insert(sender, type);
            }

            //var all = _graphBuilderService.GetAllItems();

            Console.WriteLine("Bulk insert for {0} with {1} duplicates, and {2} unique took {3} ms. -> total inserted found //{4}",
                toInsert.Count, alreadyInserted, inserted, sw.Elapsed.TotalMilliseconds, null);// all.Count);
        }

        public void InsertSingle(string sender)
        {
            _graphBuilderService.Insert(sender);

            //var isValid = _graphBuilderService.IsValidSender(sender);
            //if (!isValid)
            //    return;

            //var result = _graphBuilderService.Search(sender);
            //if(string.IsNullOrEmpty(result))
            //    _graphBuilderService.Insert(sender);
        }

        public List<string> GetAllSenders()
        {
            return _graphBuilderService.GetAllItems();
        }

        public string? Search(string sender)
        {
            return _graphBuilderService.Search(sender);
        }


    }
}
