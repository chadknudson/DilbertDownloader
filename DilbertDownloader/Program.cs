using CommandLine;
using System;

namespace DilbertDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            var results = Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(o =>
                {
                    DilbertDownloader downloader = new DilbertDownloader();
                    downloader.Initialize();

                    if (o.Year >= 1989) 
                    {
                        if ((o.Month > 0) && (o.Month <= 12))
                        {
                            if (o.Day > 0)
                            {
                                downloader.DownloadComic(new DateTime(o.Year, o.Month, o.Day));
                            }
                            else
                            {
                                downloader.DownloadComics(o.Year, o.Month);
                            }
                        }
                        else
                        {
                            downloader.DownloadComics(o.Year);
                        }
                    }
                    else
                    {
                        if (o.All)
                        {

                        }
                        else
                        {
                            downloader.DownloadComics(DateTime.Now.Year);
                        }
                    }
                });
        }
    }
}
