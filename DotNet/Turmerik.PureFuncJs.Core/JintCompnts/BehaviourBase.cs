using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turmerik.Text;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public abstract class BehaviourBase
    {
        protected BehaviourBase(
            string moduleName,
            Engine jsEngine)
        {
            ModuleName = moduleName ?? throw new ArgumentNullException(nameof(moduleName));
            JsEngine = jsEngine ?? throw new ArgumentNullException(nameof(jsEngine));
        }

        public string ModuleName { get; }
        protected Engine JsEngine { get; }

        protected TResult ExecuteJs<TResult>(
            string methodName,
            params object[] args)
        {
            string jsMethodName = $"{ModuleName}[{methodName}]";
            string argsStr = string.Join(", ", args);

            string methodCallScript = $"{jsMethodName}({argsStr})";
            var complVal = JsEngine.Execute(methodCallScript).GetCompletionValue();

            string json = complVal.ToString();
            TResult result = JsonH.FromJson<TResult>(json);

            return result;
        }
    }
}
