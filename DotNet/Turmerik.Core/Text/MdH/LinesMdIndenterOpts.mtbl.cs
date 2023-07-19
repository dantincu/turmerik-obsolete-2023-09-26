using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.Text.MdH
{
    public class LinesMdIndenterOpts
    {
        public int EmphasizeLinkMaxLength { get; set; }
        public Func<SliceStrArgs, int> LinkStartCharPredicate { get; set; }
        public Func<SliceStrArgs, int, int> LinkEndCharPredicate { get; set; }
    }
}
