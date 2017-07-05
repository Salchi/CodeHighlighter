using CodeHighlighter.BusinessLogic.SyntaxHighlighting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeHighlighter.BusinessLogic
{
    internal class FileHandler
    {
        public Task<(int totalFiles, int handledFiles)> CreateHighlightedHtmlOfContentsAsync(string sourceDirectory, string searchPattern, 
            string skipPattern, string outputFile, IProgress<double> progress)
        {
            return Task.Factory.StartNew(() =>
            {
                EnsureNotNullOrEmpty(sourceDirectory, nameof(sourceDirectory));
                EnsureNotNullOrEmpty(outputFile, nameof(outputFile));

                File.WriteAllText(outputFile, "");

                var files = GetAllFilesIn(sourceDirectory, searchPattern, skipPattern).ToList();
                var totalFiles = files.Count;
                var handledFiles = 0;

                try
                {
                    var highlightedFiles = HighlightAllFilesInParallel(files, progress);
                    handledFiles = highlightedFiles.Count;

                    foreach (var kvp in highlightedFiles.OrderBy(x => x.Key))
                    {
                        PrintFileContent(outputFile, kvp.Value.file, kvp.Value.content);
                    }
                }
                catch (Exception) {}

                return (totalFiles, handledFiles);
            });
        }

        private void PrintFileContent(string outputFile, string inputFile, string content)
        {
            File.AppendAllText(outputFile, $"<h2>{Path.GetFileName(inputFile)}</h2>");
            File.AppendAllText(outputFile, content);
        }

        private IDictionary<int, (string file, string content)> HighlightAllFilesInParallel(List<string> files, IProgress<double> progress)
        {
            using (var fileExtensionToLexerMapper = new FileExtensionToLexerMapper())
            {
                var locker = new object();
                var handledFiles = 0;
                var totalFiles = files.Count;
                var result = new ConcurrentDictionary<int, (string, string)>();

                Parallel.ForEach(files, file =>
                {
                    var fileContent = File.ReadAllText(file);
                    int index;

                    lock (locker)
                    {
                        index = files.IndexOf(file);
                    }

                    var tuple = (file, SyntaxHighlighterFactory.CreateNew(
                        fileExtensionToLexerMapper.MapAsync(Path.GetExtension(file)).Result
                    ).HighlightCodeAsync(fileContent).Result);

                    result.TryAdd(index, tuple);

                    Interlocked.Increment(ref handledFiles);
                    progress.Report(handledFiles * 100.0 / totalFiles);
                });

                return result;
            }
        }

        private void EnsureNotNullOrEmpty(string param, string name)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentException($"{name} can not be null or empty");
            }
        }

        private string[] PatternToArray(string pattern)
        {
            return pattern.Split('|');
        }
        public ICollection<string> GetAllFilesIn(string path, string searchPattern, string skipPattern)
        {
            var files = new List<string>();

            try
            {
                if (!Directory.Exists(path))
                {
                    return files;
                }

                var skipPatterns = PatternToArray(string.IsNullOrEmpty(skipPattern) ? string.Empty : skipPattern);

                foreach (var pattern in PatternToArray(string.IsNullOrEmpty(searchPattern) ? "*" : searchPattern))
                {
                    foreach (string file in Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories))
                    {
                        if (skipPattern.Length > 0 && !skipPatterns.Any(x => file.Contains(x)))
                        {
                            files.Add(file);
                        }
                    }
                }
            }
            catch (Exception) { }

            return files;
        }
    }
}