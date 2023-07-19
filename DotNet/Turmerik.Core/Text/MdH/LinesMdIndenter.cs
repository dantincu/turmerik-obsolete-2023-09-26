using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.Text.MdH
{
    public interface ILinesMdIndenter
    {
        string AddIdent(
            string line,
            int emphasizeLinkMaxLength);
    }

    public class LinesMdIndenter : ILinesMdIndenter
    {
        public string AddIdent(string line, int emphasizeLinkMaxLength)
        {
            throw new NotImplementedException();
        }
    }
}
