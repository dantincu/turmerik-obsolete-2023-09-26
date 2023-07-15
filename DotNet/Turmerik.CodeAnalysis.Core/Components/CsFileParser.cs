using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public interface ICsFileParser
    {
        ParsedCsFile.Immtbl ParseCsFile(string csFileCode);
    }

    public class CsFileParser : ICsFileParser
    {
        public ParsedCsFile.Immtbl ParseCsFile(string csFileCode)
        {
            throw new NotImplementedException();
        }
    }
}
