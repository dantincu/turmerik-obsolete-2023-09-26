using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.MsVSTextTemplating.Components;
using Turmerik.Synchronized;
using static Turmerik.MsVSTextTemplating.Components.ClnblTypesCodeGeneratorConfigCore;

namespace Turmerik.MsVSTextTemplating
{
    public interface IAppConfig : IAppConfigCore<ClnblTypesCodeGeneratorConfig.Immtbl, ClnblTypesCodeGeneratorConfigSrlzbl.Mtbl>
    {
    }

    public class AppConfig : AppConfigCoreBase<ClnblTypesCodeGeneratorConfig.Immtbl, ClnblTypesCodeGeneratorConfigSrlzbl.Mtbl>, IAppConfig
    {
        public AppConfig(
            IAppEnv appEnv,
            IInterProcessConcurrentActionComponentFactory concurrentActionComponentFactory) : base(
                appEnv,
                concurrentActionComponentFactory)
        {
        }

        protected override ClnblTypesCodeGeneratorConfigSrlzbl.Mtbl GetDefaultConfig() => new ClnblTypesCodeGeneratorConfigSrlzbl.Mtbl
        {
            HelperClassSuffix = ClnblTypesCodeGeneratorConfigDefaults.HX_SFFX,
            GetterMethodsPrefix = ClnblTypesCodeGeneratorConfigDefaults.GET,
            IncludedNamespaceStatements = ClnblTypesCodeGeneratorConfigDefaults.IncludedNamespaces,
            TypeNames = new TypeNames.Mtbl
            {
                CloneableInterface = ClnblTypesCodeGeneratorConfigDefaults.ICLNBL,
                Immutable = ClnblTypesCodeGeneratorConfigDefaults.IMMTBL,
                Mutable = ClnblTypesCodeGeneratorConfigDefaults.MTBL,
                EnumerableInterface = ClnblTypesCodeGeneratorConfigDefaults.EnumerableIntfTypeName,
                DictionaryCoreInterface = ClnblTypesCodeGeneratorConfigDefaults.DictionaryCoreIntfTypeName,
                List = ClnblTypesCodeGeneratorConfigDefaults.ListTypeName,
                Dictionary = ClnblTypesCodeGeneratorConfigDefaults.DictionaryTypeName,
                ReadOnlyCollection = ClnblTypesCodeGeneratorConfigDefaults.ReadOnlyCollectionTypeName,
                ReadOnlyDictionary = ClnblTypesCodeGeneratorConfigDefaults.ReadOnlyCollectionTypeName,
                ClnblNs = ClnblTypesCodeGeneratorConfigDefaults.ClnblNsTypeAttrTypeName
            },
            HelperMethodNames = new HelperMethodNames.Mtbl
            {
                ToImmtbl = ClnblTypesCodeGeneratorConfigDefaults.ToImmtbl,
                AsImmtbl = ClnblTypesCodeGeneratorConfigDefaults.AsImmtbl,
                ToMtbl = ClnblTypesCodeGeneratorConfigDefaults.ToMtbl,
                AsMtbl = ClnblTypesCodeGeneratorConfigDefaults.AsMtbl,
                ToImmtblCllctn = ClnblTypesCodeGeneratorConfigDefaults.ToImmtblCllctn,
                AsImmtblCllctn = ClnblTypesCodeGeneratorConfigDefaults.AsImmtblCllctn,
                ToMtblList = ClnblTypesCodeGeneratorConfigDefaults.ToMtblList,
                AsMtblList = ClnblTypesCodeGeneratorConfigDefaults.AsMtblList,
                AsImmtblDictnr = ClnblTypesCodeGeneratorConfigDefaults.AsImmtblCllctn,
                AsMtblDictnr = ClnblTypesCodeGeneratorConfigDefaults.AsMtblDictnr,
            }
        };

        protected override ClnblTypesCodeGeneratorConfig.Immtbl NormalizeConfig(
            ClnblTypesCodeGeneratorConfigSrlzbl.Mtbl config)
        {
            var configMtbl = new ClnblTypesCodeGeneratorConfig.Mtbl(config);
            var configImmtbl = new ClnblTypesCodeGeneratorConfig.Immtbl(configMtbl);

            return configImmtbl;
        }
    }
}
