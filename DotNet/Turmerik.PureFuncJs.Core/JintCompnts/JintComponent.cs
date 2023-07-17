using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Jint;
using Jint.Native;
using Jint.Native.Object;
using Turmerik.Collections;
using Turmerik.Text;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public interface IJintComponent
    {
        string ExportedMembersRootObjVarName { get; }
        ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> ExportedMemberNames { get; }

        string Execute(string jsCode);

        TResult Execute<TResult>(
            string jsCode,
            bool useCamelCase = true);
    }

    public interface IJintComponent<TBehaviour> : IJintComponent
    {
        TBehaviour Behaviour { get; }
    }

    public class JintComponent : IJintComponent
    {
        public JintComponent(
            string jsCode,
            Func<string, string> exportedMembersRootObjVarNameFactory = null,
            Func<string, string, string> jsCodeAugmenter = null)
        {
            ExportedMembersRootObjVarName = exportedMembersRootObjVarNameFactory?.Invoke(jsCode) ?? GetExportedMembersRootObjVarName(jsCode);
            JsCode = jsCodeAugmenter?.Invoke(jsCode, ExportedMembersRootObjVarName) ?? AugmentJsCode(jsCode, ExportedMembersRootObjVarName);

            Engine = new Engine().Execute(JsCode);
            ExportedMembers = Engine.GetCompletionValue().AsObject();

            ExportedMemberNames = GetExportedMemberNames(
                ExportedMembersRootObjVarName,
                ExportedMembers);
        }

        public string ExportedMembersRootObjVarName { get; }
        public ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> ExportedMemberNames { get; }

        protected string JsCode { get; }
        protected Engine Engine { get; }
        protected ObjectInstance ExportedMembers { get; }

        public string Execute(string jsCode) => Engine.Execute(jsCode).GetCompletionValue().ToString();

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

        protected virtual string GetExportedMembersRootObjVarName(
            string jsCode)
        {
            _ = jsCode.SliceStr(
                args => char.IsLetter(args.Char) ? 0 : 1,
                (args, stIdx) => char.IsLetter(args.Char) ? 1 : 0,
                false,
                result =>
                {
                    if (result.SlicedStr != "var")
                    {
                        throw new InvalidOperationException(
                            $"The provided javascript code's first word should be 'var', but it is '{result.SlicedStr}'");
                    }
                }).SlicedStr ?? throw new InvalidOperationException(
                    "Invalid javascript code");

            string exportedMembersRootObjVarName = jsCode.SliceStr(
                args => args.Char.IsValidCodeIdentifier() ? 0 : 1,
                (args, stIdx) => args.Char.IsValidCodeIdentifier() ? 1 : 0).SlicedStr ?? throw new InvalidOperationException(
                    "Invalid javascript code");

            return exportedMembersRootObjVarName;
        }

        protected virtual string AugmentJsCode(
            string jsCode,
            string exportedMembersRootObjVarName)
        {
            string completionValueJsStatement = $"{exportedMembersRootObjVarName};";

            if (!jsCode.TrimEnd().EndsWith(";"))
            {
                completionValueJsStatement = $";{completionValueJsStatement}";
            }

            jsCode = string.Concat(
                jsCode,
                completionValueJsStatement);

            return jsCode;
        }

        private ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> GetExportedMemberNames(
            string exportedMembersRootObjVarName,
            ObjectInstance exportedMembers)
        {
            var mtblMap = new Dictionary<string, Dictionary<string, string>>();

            var exportedMembersRootObj = exportedMembers.GetProperty(
                exportedMembersRootObjVarName).Value.AsObject();

            foreach (var groupProp in exportedMembersRootObj.GetOwnProperties())
            {
                var groupName = groupProp.Key;
                var group = groupProp.Value.Get.AsObject();

                var map = new Dictionary<string, string>();
                mtblMap.Add(groupName, map);

                foreach (var memberProp in group.GetOwnProperties())
                {
                    var memberName = memberProp.Key;
                    var member = memberProp.Value.Get.ToString();

                    map.Add(memberName, member);
                }
            }

            var rdnlMap = mtblMap.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.RdnlD()).RdnlD();

            return rdnlMap;
        }
    }

    public class JintComponent<TBehaviour> : JintComponent, IJintComponent<TBehaviour>
    {
        public JintComponent(
            string jsCode,
            Func<IJintComponent<TBehaviour>, ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>>, TBehaviour> behaviourFactory,
            Func<string, string> exportedMembersRootObjVarNameFactory = null,
            Func<string, string, string> jsCodeAugmenter = null) : base(
                jsCode,
                exportedMembersRootObjVarNameFactory,
                jsCodeAugmenter)
        {
            Behaviour = behaviourFactory(
                this,
                ExportedMemberNames);
        }

        public TBehaviour Behaviour { get; }
    }
}
