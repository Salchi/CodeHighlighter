using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CodeHighlighter.BusinessLogic.SyntaxHighlighting
{
    class FileExtensionToLexerMapper : IDisposable
    {
        private const string MappingsFile = "Config/FileToLexerMapping.xml";
        private const string AttrFileExtension = "file-extension";
        private const string AttrLexerShortName = "lexer-short-name";
        private Stream stream;
        private XDocument doc;

        public FileExtensionToLexerMapper()
        {
            stream = File.Open(MappingsFile, FileMode.Open);
            doc = XDocument.Load(stream);
        }

        public Task<string> MapAsync(string fileExtension)
        {
            return Task.Factory.StartNew(() =>
            {
                return doc.Root.Elements()
                .Where(x => x.Attribute(AttrFileExtension).Value.Replace(" ", "").ToLower().Split(',').Contains(fileExtension.Replace(".", "").ToLower()))
                .Select(x => x.Attribute(AttrLexerShortName).Value).FirstOrDefault();
            });
        }

        public void Dispose()
        {
            stream.Close();
            stream.Dispose();
        }
    }
}
