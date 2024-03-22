using System;
using System.IO;
using System.Text;

namespace crawlerpathlogger
{
    public class CrawlerPathLogger
    {
        private string[] dirsarr;
        private string filepathlog;
        private string logicdiskletter;

        public CrawlerPathLogger(string diskletter)
        {
            logicdiskletter = diskletter;
        }

        public string[] DirectoriesArray
        {
            get { return dirsarr; }
            set { dirsarr = value; }
        }

        public string FilePathLog
        {
            get { return filepathlog; }
            set { filepathlog = value; }
        }

        public void LogDirectories()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filepathlog, true, Encoding.UTF8))
                {
                    foreach (string directory in dirsarr)
                    {
                        WriteDirectory(writer, directory, 0);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Error: access denied to directory - " + ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void WriteDirectory(StreamWriter writer, string directory, int level)
        {
            string[] subdirs;
            try
            {
                subdirs = Directory.GetDirectories(directory);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Error: access denied to directory - " + ex.Message);
                return;
            }

            string indent = new string(' ', level * 4);
            writer.WriteLine($"{DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss")}, {GetDirectoryName(directory)}");

            foreach (string subdir in subdirs)
            {
                WriteDirectory(writer, subdir, level + 1);
            }

            string[] files;
            try
            {
                files = Directory.GetFiles(directory);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Error: access denied to directory - " + ex.Message);
                return;
            }

            foreach (string file in files)
            {
                writer.WriteLine($"{indent} -> {DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss")}, {GetFileName(file)}");
            }
        }

        private string GetDirectoryName(string path)
        {
            return path.Replace(logicdiskletter, "");
        }

        private string GetFileName(string path)
        {
            string filename = Path.GetFileName(path);
            string fileextension = Path.GetExtension(path);
            return $"{filename}, {fileextension}";
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            CrawlerPathLogger crawler = new CrawlerPathLogger("c:\\");
            crawler.FilePathLog = "log.txt";
            crawler.DirectoriesArray = new string[] { "c:\\folder1", "c:\\folder2" };
            crawler.LogDirectories();
        }
    }
}