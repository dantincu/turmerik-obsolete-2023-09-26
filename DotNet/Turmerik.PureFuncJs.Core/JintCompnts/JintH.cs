using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public static class JintH
    {
        public static string CreateScript(
            string jsCode,
            string argsJson)
        {
            jsCode = jsCode.Trim().TrimEnd(';').TrimEnd(')');
            string trailingStr = jsCode.First() == '(' ? "));" : ");";

            jsCode = string.Concat(
                jsCode,
                argsJson,
                trailingStr);

            return jsCode;
        }

        public static string CreateScript(
            string jsCode,
            string[] argsJsonArr)
        {
            string argsJson = string.Join(", ", argsJsonArr);
            jsCode = CreateScript(jsCode, argsJson);

            return jsCode;
        }
    }
}
