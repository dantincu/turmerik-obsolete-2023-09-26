using Jint;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public interface IJintComponentFactory
    {
        IJintComponent Create(
            string jsCode,
            Func<string, string> exportedMembersRootObjVarNameFactory = null,
            Func<string, string, string> jsCodeAugmenter = null);

        IJintComponent<TBehaviour> Create<TBehaviour>(
            string jsCode,
            Func<IJintComponent<TBehaviour>, ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>>, TBehaviour> behaviourFactory,
            Func<string, string> exportedMembersRootObjVarNameFactory = null,
            Func<string, string, string> jsCodeAugmenter = null);
    }

    public class JintComponentFactory : IJintComponentFactory
    {
        public IJintComponent Create(
            string jsCode,
            Func<string, string> exportedMembersRootObjVarNameFactory = null,
            Func<string, string, string> jsCodeAugmenter = null) => new JintComponent(
                jsCode,
                exportedMembersRootObjVarNameFactory,
                jsCodeAugmenter);

        public IJintComponent<TBehaviour> Create<TBehaviour>(
            string jsCode,
            Func<IJintComponent<TBehaviour>, ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>>, TBehaviour> behaviourFactory,
            Func<string, string> exportedMembersRootObjVarNameFactory = null,
            Func<string, string, string> jsCodeAugmenter = null) => new JintComponent<TBehaviour>(
                jsCode,
                behaviourFactory,
                exportedMembersRootObjVarNameFactory,
                jsCodeAugmenter);
    }
}
