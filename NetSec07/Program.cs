namespace NetSec07
{
    public class Program
    {
        private static bool verbose = false;
        private static string inputFile = Path.Combine(Environment.CurrentDirectory, "graph.csv");


        public static int Main(string[] args)
        {
            // simple argument parsing. not checking for any bounds, just get the input done.
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--verbose" || args[i] == "-verbose")
                {
                    verbose = true;
                }

                if (args[i] == "--input" || args[i] == "-input")
                {
                    inputFile = args[i + 1];
                    if (!File.Exists(inputFile))
                    {
                        Console.WriteLine("input file not found");
                        return -1;
                    }
                }

                if (args[i] == "--help" || args[i] == "-help")
                {
                    Console.WriteLine("Console Application to calculate Bellman-Ford.");
                    Console.WriteLine("Use --verbose to show intermidate setps");
                    Console.WriteLine("Use --input path/to/file.csv to input a graph. Ortherwise program will look for graph.csv in the current directory.");
                }
            }

            // create algo class and calculate for all vertecies
            new BelmannFord(verbose)
                .ParseCsv(inputFile)
                .Calculate();

            return 0;
        }
    }
}