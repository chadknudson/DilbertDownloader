using CommandLine;

namespace DilbertDownloader
{
    public class CommandLineOptions
    {
        [Option('a', "all", Default = false, HelpText = "Number of comics to get in each batch.")]
        public bool All { get; set; }

        [Option('b', "batchsize", Default = 80, HelpText = "Number of comics to get in each batch.")]
        public int BatchSize { get; set; }

        [Option('p', "pause", Default = 180, HelpText = "Number of seconds to pause between batches.")]
        public int Pause { get; set; }

        [Option('y', "year", HelpText = "4-digit year to download.")]
        public int Year { get; set; }

        [Option('m', "month", HelpText = "2-digit month to download.")]
        public int Month { get; set; }

        [Option('d', "day", HelpText = "2-digit day of the month to download.")]
        public int Day { get; set; }
    }
}
