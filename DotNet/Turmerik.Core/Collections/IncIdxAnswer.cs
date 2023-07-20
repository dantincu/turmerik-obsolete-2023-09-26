using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.Collections
{
    public readonly struct IncIdxAnswer
    {
        public IncIdxAnswer(bool answer, int? incIdx)
        {
            Answer = answer;
            IncIdx = incIdx;
        }

        public bool Answer { get; }
        public int? IncIdx { get; }
    }

    public static class IncIdxAnswerH
    {
        public static IncIdxAnswer ToIncIdxAnswer(
            this bool answer,
            Func<bool, int?> incIdxFactory)
        {
            int? incIdx = null;

            if (incIdxFactory != null)
            {
                incIdx = incIdxFactory(answer);
            }

            return new IncIdxAnswer(answer, incIdx);
        }

        public static IncIdxAnswer ToIncIdxAnswer(
            this bool answer,
            int? trueVal,
            int? falseVal) => answer.ToIncIdxAnswer(
                ans => ans ? trueVal : falseVal);

        public static IncIdxAnswer ToIncIdxAnswer(
            this bool answer,
            int? val) => answer.ToIncIdxAnswer(
                ans => val);
    }
}
