using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.PureFuncJs.Core.JintCompnts;

namespace Turmerik.WinForms.Behaviours
{
    public interface ITimeStampParseBehaviour
    {
        TryFindNextTimeStampResult TryFindNextTimeStamp(
            string inputText,
            int startIdx);
    }

    public class TimeStampParseBehaviour : BehaviourBase, ITimeStampParseBehaviour
    {
        public TimeStampParseBehaviour(
            string moduleName,
            Engine jsEngine) : base(
                moduleName,
                jsEngine)
        {
        }

        public TryFindNextTimeStampResult TryFindNextTimeStamp(
            string inputText,
            int startIdx) => ExecuteJs<TryFindNextTimeStampResult>(
                nameof(TryFindNextTimeStamp),
                inputText,
                startIdx);
    }

    public class TryFindNextTimeStampResult
    {
        public int NextTimeStampEndIdx { get; set; }
        public string NextTimeStampText { get; set; }
        public DateTime NextTimeStampValue { get; set; }
    }
}
