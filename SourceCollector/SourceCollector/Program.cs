using System;
using System.Collections.Generic;
using System.IO;

namespace SourceCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("3 params required!");
                Console.WriteLine("First: path to source directory");
                Console.WriteLine("Second: search pattern");
                Console.WriteLine("Third: path to output file");
                return;
            }

            var outputFile = args[2];
            File.WriteAllText(outputFile, "");
            var files = GetAllFilesIn(args[0], args[1]);

            foreach (var file in files)
            {
                if (!file.Contains("bootstrap") && !file.Contains("jquery"))
                {
                    var lineBreaks = $"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}";
                    File.AppendAllText(outputFile, $"{lineBreaks}{Path.GetFileName(file)}{lineBreaks}");
                    File.AppendAllText(outputFile, File.ReadAllText(file));
                }
            }

            Console.WriteLine($"copied content of {files.Count} files to '{outputFile}'!");
        }

        private static ICollection<string> GetAllFilesIn(string path, string searchPattern)
        {
            var files = new List<string>();
            var searchPatterns = searchPattern.Split('|');

            foreach (string directory in Directory.GetDirectories(path))
            {
                foreach (var pattern in searchPatterns)
                {
                    foreach (string file in Directory.GetFiles(directory, pattern))
                    {
                        files.Add(file);
                    }
                }

                files.AddRange(GetAllFilesIn(directory, searchPattern));
            }

            return files;
        }
    }
}