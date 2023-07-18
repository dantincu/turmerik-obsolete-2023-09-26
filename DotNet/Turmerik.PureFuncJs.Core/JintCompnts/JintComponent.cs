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
        ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> ExportedMemberNames { get; }

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
        TBehaviour Behaviour { get; }
    }

    public class JintComponent : IJintComponent
    {
        public const string CONSOLE_VAR_NAME = "console";
        public const string TRMRK_ROOT_OBJ_NAME = "trmrk";

        public const string TRMRK_EXP_OBJ_NAME = "exp";
        public const string TRMRK_LIB_OBJ_NAME = "lib";
        public const string TRMRK_IMP_OBJ_NAME = "imp";

        public static readonly string TrmrkExpObjPath = string.Join(".",
            TRMRK_ROOT_OBJ_NAME,
            TRMRK_EXP_OBJ_NAME);

        public static readonly string TrmrkLibObjPath = string.Join(".",
            TRMRK_ROOT_OBJ_NAME,
            TRMRK_LIB_OBJ_NAME);

        public static readonly string TrmrkImpObjPath = string.Join(".",
            TRMRK_ROOT_OBJ_NAME,
            TRMRK_IMP_OBJ_NAME);

        public JintComponent(
            IJintConsole console,
            string jsCode)
        {
            JintConsole = console ?? throw new ArgumentNullException(nameof(console));
            JsCode = jsCode;
            Engine = GetEngine(console).Execute(JsCode);

            ExportedMembers = Engine.Evaluate(
                TrmrkExpObjPath).AsObject();

            ExportedMemberNames = GetExportedMemberNames(
                ExportedMembers);
        }

        public IJintConsole JintConsole { get; }
        public ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> ExportedMemberNames { get; }

        protected string JsCode { get; }
        protected Engine Engine { get; }
        protected ObjectInstance ExportedMembers { get; }

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

        public string Call(string jsCode,
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

        private ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> GetExportedMemberNames(
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
        }

        private JsObject GetConsoleObject(
            Engine engine,
            IJintConsole console)
        {
            var obj = new JsObject(engine);

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

            return obj;
        }

        private Engine GetEngine(
            IJintConsole console)
        {
            var engine = new Engine();
            var consoleObj = GetConsoleObject(engine, console);

            engine = engine.SetValue(
                CONSOLE_VAR_NAME,
                consoleObj);

            return engine;
        }
    }

    public class JintComponent<TBehaviour> : JintComponent, IJintComponent<TBehaviour>
    {
        public JintComponent(
            IJintConsole jintConsole,
            string jsCode,
            Func<IJintComponent<TBehaviour>, ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>>, TBehaviour> behaviourFactory) : base(
                jintConsole,
                jsCode)
        {
            Behaviour = behaviourFactory(
                this,
                ExportedMemberNames);
        }

        public TBehaviour Behaviour { get; }
    }
}
