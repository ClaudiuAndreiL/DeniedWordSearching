using Bogus;
using System.Diagnostics;

namespace DeniedWordSearching
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var builder = new NodeBuilder();

            var deniedWords = new List<string> { "info", "verify", "bank", "ingot" };

            var fakes = new Faker();
            var generatedWords = Enumerable.Range(0, 5000).Select(x => fakes.Random.Word().Split(' ').First()).ToList();
            deniedWords.AddRange(generatedWords);

            var sw = Stopwatch.StartNew();
            foreach (var deniedWord in deniedWords)
                builder.InsertBannedWord(deniedWord);
            sw.Stop();
            Console.WriteLine("Generation took {0} ms for {1} items", sw.Elapsed.TotalMilliseconds, deniedWords.Count);

            for (int i = 0; i< 10; i++)
            {
                sw.Restart();
                var pickRandomDenied = fakes.PickRandom(deniedWords);
                var innerCheck = builder.FindFirstDeniedWord(pickRandomDenied);
                Console.WriteLine("random search took: {0} ms", sw.Elapsed.TotalMilliseconds);
                sw.Stop();
            }

            sw.Restart();
            var check = builder.FindFirstDeniedWord("inf0");  //inf0, 1nnf0
            Console.WriteLine("out search took: {0} ms", sw.Elapsed.TotalMilliseconds);
           

            Console.WriteLine("byebye world ");
        }
    }
}
