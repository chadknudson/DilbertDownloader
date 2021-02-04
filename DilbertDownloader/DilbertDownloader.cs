using HtmlAgilityPack;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

namespace DilbertDownloader
{
    public class DilbertDownloader
    {
        int delay;
        int batchItem;
        int batchSize;

        public DilbertDownloader()
        {
            TargetFolder = @"D:\Temp\Dilbert\";
        }

        public string TargetFolder { get; set; }
        public string[] ExistingFiles { get; set; }

        public void Initialize()
        {
            ExistingFiles = Directory.GetFiles(Path.GetDirectoryName(TargetFolder), "*.*");
        }

        public void DownloadComic(DateTime date)
        {
            string comicStripFilename = GetComicStripFilenameForDate(date);
            if (!DoesComicStripFilenameExist(comicStripFilename))
            {
                Console.WriteLine("Downloading comic for {0}...", date.ToShortDateString());
                string comicStripURL = GetComicStripUrlForDate(date);
                string comicSourceURL = GetComicSourceURL(comicStripURL);
                SaveImage(comicSourceURL, comicStripFilename);
            }
        }

        public void DownloadComics(int year, int month)
        {
            if (year < 1989)
                return; // Dilbert was fist published on April 16th, 1989

            DateTime date = new DateTime(year, 1, 1);
            if (year == 1989)
                date = new DateTime(year, 4, 16);

            while ((date.Year == year) && (date.Month == month))
            {
                DownloadComic(date);
                date = date.AddDays(1);
                if (date > DateTime.Today)
                    break;
            }
        }

        public void DownloadComics(int year)
        {
            if (year < 1989)
                return; // Dilbert was fist published on April 16th, 1989

            DateTime date = new DateTime(year, 1, 1);
            if (year == 1989)
                date = new DateTime(year, 4, 16);

            while (date.Year == year)
            {
                DownloadComic(date);
                date = date.AddDays(1);
                if (date > DateTime.Today)
                    break;
            }
        }

        public string GetComicStripUrlForDate(DateTime date)
        {
            return string.Format("http://dilbert.com/strip/{0}-{1:00}-{2:00}", date.Year, date.Month, date.Day);
        }

        public string GetComicStripFilenameForDate(DateTime date)
        {
            return TargetFolder + string.Format("Dilbert{0}{1:00}{2:00}", date.Year, date.Month, date.Day);
        }

        public string GetComicSourceURL(string url)
        {
            try
            {
                HtmlWeb hw = new HtmlWeb();
                HtmlDocument doc = hw.Load(url);

                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//a[@class='img-comic-link']"))
                {
                    HtmlNode imgNode = node.ChildNodes[1];
                    HtmlAttribute src = imgNode.Attributes[4];
                    return src.Value;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public void SaveImage(string url, string filenameBase)
        {
            if (string.IsNullOrEmpty(url))
                return;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    byte[] data = webClient.DownloadData(url);

                    using (MemoryStream mem = new MemoryStream(data))
                    {
                        using (var image = Image.FromStream(mem))
                        {
                            string targetFolder = Path.GetDirectoryName(filenameBase);
                            string imageFileExtension = GetFilenameExtension(image.RawFormat);
                            string targetFilename = Path.GetFileNameWithoutExtension(filenameBase) + imageFileExtension;
                            image.Save(Path.Combine(targetFolder, targetFilename));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public static string GetFilenameExtension(ImageFormat format)
        {
            string[] extensions = ImageCodecInfo.GetImageEncoders().FirstOrDefault(x => x.FormatID == format.Guid).FilenameExtension.ToLower().Substring(1).Split(new char[] { ';' });

            return extensions[0];
        }

        public bool DoesComicStripFilenameExist(string filename)
        {
            foreach(var file in ExistingFiles)
            {
                if (file.StartsWith(filename))
                    return true;
            }
            return false;
        }
    }
}
