using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.PureFuncJs.Core.JintCompnts;

namespace Turmerik.TextUtils.WinFormsApp.Components
{
    public interface IBehavioursAgg
    {
    }

    public class BehavioursAgg : BehavioursAggBase, IBehavioursAgg
    {
        public BehavioursAgg(Engine jsEngine) : base(jsEngine)
        {
        }
    }
}
