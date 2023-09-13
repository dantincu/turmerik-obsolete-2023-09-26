using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Runtime.Interop;
using Turmerik.Collections;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public interface IJintComponent : IDisposable
    {
        IJintConsole JintConsole { get; }

        string Execute(string jsCode);

        TResult Execute<TResult>(
            string jsCode,
            bool useCamelCase = true);

        string Call(
            string jsCode,
            bool useCamelCase = true,
            params object[] argsArr);

        TResult Call<TResult>(
            string jsCode,
            bool useCamelCase = true,
            params object[] argsArr);

        string CallMethod(
            string methodName,
            bool useCamelCase = true,
            params object[] argsArr);

        TResult CallMethod<TResult>(
            string methodName,
            bool useCamelCase = true,
            params object[] argsArr);

        string CallMethod(
            string[] methodPath,
            bool useCamelCase = true,
            params object[] argsArr);

        TResult CallMethod<TResult>(
            string[] methodPath,
            bool useCamelCase = true,
            params object[] argsArr);
    }

    public interface IJintComponent<TBehaviour> : IJintComponent
    {
        TBehaviour Config { get; }
    }

    public class JintComponent : IJintComponent
    {
        public const string CONSOLE_VAR_NAME = "console";
        public const string GLOBAL_THIS_VAR_NAME = "globalThis";

        public static readonly string ScriptTemplate = string.Join("\n",
            "(function({0}) {",
            "{1}",
            "}(this);");

        public JintComponent(
            string jsCode,
            string cfgObjRetrieverCode,
            string globalThisObjName = null,
            IJintConsole console = null)
        {
            JsCode = GetJsCode(
                jsCode ?? throw new ArgumentNullException(
                    nameof(jsCode)),
                globalThisObjName ?? GLOBAL_THIS_VAR_NAME);

            CfgObjRetrieverCode = cfgObjRetrieverCode ?? throw new ArgumentNullException(
                nameof(cfgObjRetrieverCode));

            JintConsole = console;
            Engine = GetEngine(console).Execute(JsCode);

            CfgObj = Engine.Evaluate(
                CfgObjRetrieverCode).AsObject();
        }

        public IJintConsole JintConsole { get; }

        protected string JsCode { get; }
        protected string CfgObjRetrieverCode { get; }
        protected Engine Engine { get; }
        protected ObjectInstance CfgObj { get; }

        public string Execute(string jsCode) => Engine.Evaluate(jsCode).ToString();

        public TResult Execute<TResult>(
            string jsCode,
            bool useCamelCase = true)
        {
            string json = Execute(jsCode);

            TResult result = JsonH.FromJson<TResult>(
                json,
                useCamelCase);

            return result;
        }

        public string Call(
            string jsCode,
            bool useCamelCase = true,
            params object[] argsArr)
        {
            string argsJson = GetArgsJson(
                useCamelCase,
                argsArr);

            jsCode = JintH.CreateScript(
                jsCode, argsJson);

            var result = Execute(jsCode);
            return result;
        }

        public TResult Call<TResult>(
            string jsCode,
            bool useCamelCase = true,
            params object[] argsArr)
        {
            string json = Call(
                jsCode,
                useCamelCase,
                argsArr);

            TResult result = JsonH.FromJson<TResult>(
                json,
                useCamelCase);

            return result;
        }

        public string CallMethod(
            string methodName,
            bool useCamelCase = true,
            params object[] argsArr)
        {
            string methodCallScript = $"{methodName}();";

            var result = Call(
                methodCallScript,
                useCamelCase,
                argsArr);

            return result;
        }

        public TResult CallMethod<TResult>(
            string methodName,
            bool useCamelCase = true,
            params object[] argsArr)
        {
            string json = CallMethod(
                methodName,
                useCamelCase,
                argsArr);

            TResult result = JsonH.FromJson<TResult>(
                json,
                useCamelCase);

            return result;
        }

        public string CallMethod(
            string[] methodPath,
            bool useCamelCase = true,
            params object[] argsArr)
        {
            string methodName = string.Join(".", methodPath);

            string result = CallMethod(
                methodName,
                useCamelCase,
                argsArr);

            return result;
        }

        public TResult CallMethod<TResult>(
            string[] methodPath,
            bool useCamelCase = true,
            params object[] argsArr)
        {
            string json = CallMethod(
                methodPath,
                useCamelCase,
                argsArr);

            TResult result = JsonH.FromJson<TResult>(
                json,
                useCamelCase);

            return result;
        }

        public void Dispose()
        {
            JintConsole.Dispose();
            Engine.Dispose();
        }

        protected virtual string GetArgsJson(
            bool useCamelCase,
            object[] argsArr)
        {
            string[] argsJsonArr = argsArr.Select(
                arg => GetArgJson(
                    useCamelCase,
                    arg)).ToArray();

            string argsJson = string.Join(", ", argsJsonArr);
            return argsJson;
        }

        protected virtual string GetArgJson(
            bool useCamelCase,
            object arg) => arg.ToJson(useCamelCase);

        /* private ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> GetExportedMemberNames(
            ObjectInstance exportedMembers)
        {
            var mtblMap = new Dictionary<string, Dictionary<string, string>>();
            var ownProperties = exportedMembers.GetOwnProperties();

            foreach (var groupProp in ownProperties)
            {
                var groupName = groupProp.Key;
                var value = groupProp.Value.Value ?? groupProp.Value.Get;

                var type = value.GetType();
                var typeName = type.Name;

                var group = value.AsObject();

                var map = new Dictionary<string, string>();
                mtblMap.Add(groupName.ToString(), map);

                foreach (var memberProp in group.GetOwnProperties())
                {
                    var memberName = memberProp.Key;
                    var member = memberProp.Value.Value.ToString();

                    map.Add(memberName.ToString(), member);
                }
            }

            var rdnlMap = mtblMap.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.RdnlD()).RdnlD();

            return rdnlMap;
        } */

        private string GetJsCode(
            string jsCode,
            string globalThisObjName) => string.Format(
                ScriptTemplate,
                globalThisObjName,
                jsCode);

        private JsObject GetConsoleObject(
            Engine engine,
            IJintConsole console)
        {
            JsObject obj = null;

            if (console != null)
            {
                obj = new JsObject(engine);

                var propsMap = new Dictionary<string, ParamsAction>()
                {
                    { nameof(console.Log), console.Log },
                    { nameof(console.Trace), console.Trace },
                    { nameof(console.Debug), console.Debug },
                    { nameof(console.Info), console.Info },
                    { nameof(console.Warn), console.Warn },
                    { nameof(console.Error), console.Error },
                    { nameof(console.Fatal), console.Fatal },
                };

                foreach (var kvp in propsMap)
                {
                    obj[kvp.Key.DecapitalizeFirstLetter()] =
                        new DelegateWrapper(
                            engine,
                            kvp.Value);
                }
            }

            return obj;
        }

        private Engine GetEngine(
            IJintConsole console)
        {
            var engine = new Engine();
            var consoleObj = GetConsoleObject(engine, console);

            if (consoleObj != null)
            {
                engine = engine.SetValue(
                    CONSOLE_VAR_NAME,
                    consoleObj);
            }

            return engine;
        }
    }

    public class JintComponent<TCfg> : JintComponent, IJintComponent<TCfg>
    {
        public JintComponent(
            string jsCode,
            string cfgObjRetrieverCode,
            string globalThisObjName = null,
            IJintConsole jintConsole = null,
            Func<IJintComponent<TCfg>, ObjectInstance, TCfg> cfgFactory = null) : base(
                jsCode,
                cfgObjRetrieverCode,
                globalThisObjName,
                jintConsole)
        {
            cfgFactory = cfgFactory.FirstNotNull(
                (compnt, cfg) => JsonH.FromJson<TCfg>(
                    cfg.ToString()));

            Config = cfgFactory(
                this,
                CfgObj);
        }

        public TCfg Config { get; }
    }
}
