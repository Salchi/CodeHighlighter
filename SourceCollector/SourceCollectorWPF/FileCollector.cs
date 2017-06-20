using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCollectorWPF
{
    internal class FileCollector
    {
        public Task<int> CopyContents(string sourceDirectory, string searchPattern, string skipPattern, string outputFile)
        {
            return Task.Factory.StartNew(() =>
            {
                if (string.IsNullOrEmpty(sourceDirectory) || string.IsNullOrEmpty(outputFile))
                {
                    return 0;
                }

                File.WriteAllText(outputFile, "");

                var files = GetAllFilesIn(sourceDirectory, searchPattern, skipPattern);

                foreach (var file in files)
                {
                    var lineBreaks = $"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}";
                    File.AppendAllText(outputFile, $"{lineBreaks}{Path.GetFileName(file)}{lineBreaks}");
                    File.AppendAllText(outputFile, File.ReadAllText(file));
                }

                return files.Count;
            });
        }

        private string[] PatternToArray(string pattern)
        {
            return pattern.Split('|');
        }
        private ICollection<string> GetAllFilesIn(string path, string searchPattern, string skipPattern)
        {
            var files = new List<string>();

            var searchPatternToUse = string.IsNullOrEmpty(searchPattern) ? "*" : searchPattern;

            var searchPatterns = PatternToArray(searchPatternToUse);
            var skipPatterns = PatternToArray(string.IsNullOrEmpty(skipPattern) ? string.Empty : skipPattern);

            var dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories).ToList();
            if (Directory.Exists(path))
            {
                dirs.Add(path);
            }

            foreach (string directory in dirs)
            {
                foreach (var pattern in searchPatterns)
                {
                    foreach (string file in Directory.GetFiles(directory, pattern))
                    {
                        if (skipPattern.Length > 0 && !skipPatterns.Any(x => file.Contains(x)))
                        {
                            files.Add(file);
                        }
                    }
                }
            }

            return files;
        }
    }
}