using Bogus;
using System.Diagnostics;

namespace DeniedWordSearching
{
    internal class Program
    {
        private static Dictionary<string, string?> SearchWords = new Dictionary<string, string?>
        {

            { " - i.n6", "ing" },        // Matches "ing"
            { "-  1n f_0x", "info" },     // Matches "info"
            { "fly1nf0", "info" },       // Matches "info"
            { "--6u ((180m84", "guccibomba" },  // Matches "guccibomba"
            { "pueryv3r_.i_f_y", "verify" }, // Matches "verify"

            { "1ng", "ing" },            // Matches "ing"
            { "flying", null } ,         // Should *not* match "ing"
            //"bingot",
            { "b1ng0t", null },         // Fixed to properly match "ingot" (i → 1, o → 0)

            { "k-@z4r.@15", "kazarais" },      // Fixed to properly match "kazarais" (@ → a, 2 → z, 4 → a)
            { "f4nt45t1c", "fantastic" },      // Fixed to properly match "fantastic" (c → 1, s → 5)
            { "rnama", "mama" }
        };



        static void Main(string[] args)
        {
            var faker = new Faker();
            var builder = new TreeBuilderService();
            var inputWords = new List<string> { "info", "macarena", "ing", "O2", "bomba" };
            var otherWords = Enumerable.Range(0, 5000).Select(x => faker.Random.Word().Split(' ').First()).ToList();
            var totalDeniedWords = inputWords.Union(otherWords).ToList().Distinct().ToList();
            var sw = new Stopwatch();
            builder.CreateGraph(totalDeniedWords);
            Console.WriteLine("Insert took: " + sw.Elapsed.TotalMilliseconds + "ms");
            sw.Restart();
            var items = builder.GetAllItems();
            Console.WriteLine("Got all {0}  in  {1}ms", items.Count, sw.Elapsed.TotalMilliseconds);
            //items.ForEach(x => Console.WriteLine(x));

            var pairs = new Dictionary<string, string?>
            {
                { "lnf0", "info" },
                { "b1nf0zx" , "info" },
                { "O2", "O2" },
                { "br€aking$", "ing" }
            };
            
            foreach(var pair in pairs)
            {
                sw.Restart();
                var found = builder.Search(pair.Key);

                Console.WriteLine($"Input: {pair.Key}, expected {pair.Value}. Result -> {pair.Value == found }   IN  {sw.Elapsed.TotalMilliseconds} ms");
            }



            //TreeBuilder graph = new();

            //var fileReader = File.ReadLines("C:\\Users\\Fremen\\Downloads\\sanitized.txt");
            //var orderedItems = fileReader.OrderBy(x => x.Length).ThenBy(x => x).ToList();
            //var sw = new Stopwatch();            
            //sw.Restart();
            //foreach (var word in orderedItems)
            //{
            //    sw.Restart();
            //    graph.Insert(word);
            //    //Console.WriteLine("----------Insert took {0}ms", sw.Elapsed.TotalMilliseconds);
            //}
            //Console.WriteLine("TOTAL INSERT TOOK {0}ms", sw.Elapsed.TotalMilliseconds);

            //var fullList = graph.PrintTree();   
            
            //var str = string.Join(Environment.NewLine, fullList);


            //foreach (var word in SearchWords)
            //{
            //    sw.Restart();
            //    var result = graph.GetFirstDeniedSenderMatch(word.Key);
            //    Console.WriteLine("------------SEARCH TOOK {0}ms", sw.Elapsed.TotalMilliseconds);
            //    if (result == null && word.Value is not null)
            //    {
            //        Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Failed to find {0} is actually {1}", word.Key, word.Value);
            //        continue;
            //    }

            //    if (result == null && word.Value is null)
            //    {
            //        Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Correctly did not find {0}", word.Key);
            //        continue;
            //    }

            //    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!! For input '{0}' found : '{1}'", word.Key, result);
            //}
            //var ceva = "stop";



            Console.WriteLine("byebye world ");
        }

    }
}
