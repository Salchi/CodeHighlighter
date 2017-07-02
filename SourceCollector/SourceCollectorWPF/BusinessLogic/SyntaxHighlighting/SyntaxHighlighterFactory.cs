using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCollectorWPF.BusinessLogic.SyntaxHighlighting
{
    static class SyntaxHighlighterFactory
    {
        private const string baseUri = "http://hilite.me";
        private const string resource = "/api";
        private const string ParamCode = "code";
        private const string ParamLanguage = "lexer";
        private const string ParamTheme = "style";

        public static ISyntaxHighlighter CreateNew(string language)
        {
            return new RESTHighlighter(new Uri(baseUri), resource, ParamCode, new Dictionary<string, string>() {
                { ParamTheme, "borland" },
                { ParamLanguage, language }   
            });
        }
    }
}
