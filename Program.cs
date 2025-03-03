using Bogus;
using DeniedWordSearching.SecondTry;
using System.Diagnostics;

namespace DeniedWordSearching
{
    internal class Program
    {
        private static List<string> SearchWords = new List<string>
        {
            " - i.n6",
            "-  1n f_0x",
            "fly1nf0",
            "--9u ((180m84",
            "pueryv3r_.i_f_y",

            "1ng",
            "flying",

            "k-@2_4r.@15",
            "f4-.nt4stlc",
            "bing0ts"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            #region denied word preparation
            var deniedWords = new List<string> { "info", "ing", "- 1nf0219", "verify", "bank", "ingot", "charisma", "fantastic", "guccibomba", "kazarais"  };
            var fakes = new Faker();
            var generatedWords = Enumerable.Range(0, 5000).Select(x => fakes.Random.Word().Split(' ').First()).ToList();
            //deniedWords.AddRange(generatedWords);
            #endregion

            var sw = Stopwatch.StartNew();
            var builderV2 = new BannedWordGraph(Constants.V2.SubstitutionSets, Constants.V2.EmptyLookingCharacters);            
            foreach(var word in deniedWords)
            {
                //sw.Restart();
                var alreadyExists = builderV2.FindBannedWord(word);
                //Console.WriteLine("Search for {0} took {1}ms", word, sw.Elapsed.TotalMilliseconds);
                if (alreadyExists is not null) 
                { 
                //    Console.WriteLine("Already exists as '{0}'. Will not insert '{1}'", alreadyExists, word);
                    continue; 
                }
                //sw.Restart();
                builderV2.AddBannedWord(word);
                //Console.WriteLine("Added {0} took {1}ms", word, sw.Elapsed.TotalMilliseconds);
            }
            Console.WriteLine("Finished building in {0}ms", sw.Elapsed.TotalMilliseconds);
            sw.Restart();
            var isInfoBanned = builderV2.FindBannedWord("-  1n f_0x");
            Console.WriteLine("Result should be info: {0}. took {1}ms", isInfoBanned, sw.Elapsed.TotalMilliseconds);

            for (int i = 0; i < 10; i++)
            {
                var pickRandomDenied = fakes.PickRandom(deniedWords);
                sw.Restart();
                var innerCheck = builderV2.FindBannedWord(pickRandomDenied);
                Console.WriteLine("random search for [ {0} ] took: {1} ms and Found: '{2}'", pickRandomDenied, sw.Elapsed.TotalMilliseconds, innerCheck);
                sw.Stop();
            }

            var bannedWords = builderV2.GetAllBannedWords();
            Console.WriteLine($"Banned Words: {string.Join(", ", bannedWords)}");
            Console.WriteLine(Environment.NewLine);
            foreach (var item in SearchWords)
            {
                sw.Restart();
                var innerCheck = builderV2.FindBannedWord(item);
                Console.WriteLine("Random search for [ '{0}' ] took: {1} ms. Found '{2}'.", item, sw.Elapsed.TotalMilliseconds, innerCheck);
                sw.Stop();
            }


            var ceva = "hh";


            //var builder = new NodeBuilder();
            
            //foreach (var deniedWord in deniedWords)
            //    builder.InsertBannedWord(deniedWord);
            //sw.Stop();
            //Console.WriteLine("Generation took {0} ms for {1} items", sw.Elapsed.TotalMilliseconds, deniedWords.Count);

            //for (int i = 0; i< 10; i++)
            //{
            //    sw.Restart();
            //    var pickRandomDenied = fakes.PickRandom(deniedWords);
            //    var innerCheck = builder.FindFirstDeniedWord(pickRandomDenied);
            //    Console.WriteLine("random search took: {0} ms", sw.Elapsed.TotalMilliseconds);
            //    sw.Stop();
            //}

            //sw.Restart();
            //var check = builder.FindFirstDeniedWord("inf0");  //inf0, 1nnf0
            //Console.WriteLine("out search took: {0} ms", sw.Elapsed.TotalMilliseconds);
           

            Console.WriteLine("byebye world ");
        }

    }
}
