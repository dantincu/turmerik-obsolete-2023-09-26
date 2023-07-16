using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> ExportedMemberNames { get; }

        string Execute(string jsCode);

        TResult Execute<TResult>(
            string jsCode,
            bool useCamelCase = true);
    }

    public class JintComponent : IJintComponent
    {
        public JintComponent(
            string jsCode)
        {
            Engine = new Engine().Execute(jsCode);
            ExportedMembers = Engine.GetCompletionValue().AsObject();

            ExportedMemberNames = GetExportedMemberNames(ExportedMembers);
        }

        public ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> ExportedMemberNames { get; }

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

        private ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> GetExportedMemberNames(
            ObjectInstance exportedMembers)
        {
            var mtblMap = new Dictionary<string, Dictionary<string, string>>();

            foreach (var groupProp in exportedMembers.GetOwnProperties())
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
}
