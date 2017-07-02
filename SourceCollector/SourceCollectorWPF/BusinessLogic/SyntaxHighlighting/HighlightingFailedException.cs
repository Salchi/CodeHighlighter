using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCollectorWPF.BusinessLogic.SyntaxHighlighting
{
    class HighlightingFailedException : Exception
    {
        public HighlightingFailedException(string message) : base(message) { }
    }
}
