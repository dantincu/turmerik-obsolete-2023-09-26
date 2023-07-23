using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.DriveExplorerCore;
using Turmerik.FileSystem;
using Turmerik.Infrastucture;
using Turmerik.MathH;
using Turmerik.Reflection;
using Turmerik.Reflection.Cache;
using Turmerik.RegexH;
using Turmerik.Synchronized;
using Turmerik.Text;
using Turmerik.Text.MdH;
using Turmerik.TreeTraversal;
using Turmerik.TrmrkAction;
using Turmerik.Utils;

namespace Turmerik.Dependencies
{
    public static class TrmrkCoreServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services,
            bool addCurrentProcessInfo = false)
        {
            services.AddSingleton<IAppProcessIdentifier>(
                svcProv => new AppProcessIdentifier(
                    addCurrentProcessInfo));

            services.AddSingleton<IExceptionSerializer, ExceptionSerializer>();
            services.AddSingleton<IStringTemplateParser, StringTemplateParser>();
            services.AddSingleton<ITimeStampHelper, TimeStampHelper>();
            services.AddSingleton<IFsPathNormalizer, FsPathNormalizer>();
            services.AddSingleton<ILambdaExprHelper, LambdaExprHelper>();
            services.AddSingleton<ILambdaExprHelperFactory, LambdaExprHelperFactory>();
            services.AddSingleton<IBasicEqualityComparerFactory, BasicEqualityComparerFactory>();
            // services.AddSingleton<IProgramArgsBuilder, ProgramArgsBuilder>();

            services.AddSingleton<ISliceStrResultEqCompr, SliceStrResultEqCompr>();
            services.AddSingleton<ITextReplacerComponent, TextReplacerComponent>();
            services.AddSingleton<ITabsToMdTableConverter, TabsToMdTableConverter>();
            services.AddSingleton<ILinesMdIndenter, LinesMdIndenter>();

            services.AddSingleton<ITreeTraversalComponentFactory, TreeTraversalComponentFactory>();
            services.AddSingleton<IMatrixBuilderFactory, MatrixBuilderFactory>();

            services.AddSingleton<IMutexCreator, MutexCreator>();
            services.AddSingleton<IOnceExecutedActionFactory, OnceExecutedActionFactory>();
            services.AddSingleton<IOnceExecutedAsyncActionFactory, OnceExecutedAsyncActionFactory>();
            services.AddSingleton<IDisposableComponentFactory, DisposableComponentFactory>();
            services.AddSingleton<IAsyncDisposableComponentFactory, AsyncDisposableComponentFactory>();

            services.AddSingleton<IThreadSafeActionComponentFactory, ThreadSafeActionComponentFactory>();
            services.AddSingleton<INonSynchronizedActionComponentFactory, NonSynchronizedActionComponentFactory>();
            services.AddSingleton<IInterProcessConcurrentActionComponentFactory, InterProcessConcurrentActionComponentFactory>();

            services.AddSingleton<IThreadSafeDataCacheFactory, ThreadSafeDataCacheFactory>();
            services.AddSingleton<INonSynchronizedDataCacheFactory, NonSynchronizedDataCacheFactory>();
            services.AddSingleton<IInterProcessConcurrentDataCacheFactory, InterProcessConcurrentDataCacheFactory>();

            services.AddSingleton<IInterProcessConcurrentStaticDataCacheFactory, InterProcessConcurrentStaticDataCacheFactory>();
            services.AddSingleton<IThreadSafeStaticDataCacheFactory, ThreadSafeStaticDataCacheFactory>();
            services.AddSingleton<INonSynchronizedStaticDataCacheFactory, NonSynchronizedStaticDataCacheFactory>();

            services.AddTransient<IThreadSafeActionComponent, ThreadSafeActionComponent>();
            services.AddTransient<INonSynchronizedActionComponent, NonSynchronizedActionComponent>();

            services.AddTransient<IAsyncActionsQueue, AsyncActionsQueue>();

            services.AddSingleton<IMemberAccessibiliyFilterEqualityComparerFactory, MemberAccessibiliyFilterEqualityComparerFactory>();
            services.AddSingleton<ICachedTypesMapFactory, NonSynchronizedCachedTypesMapFactory>();
            services.AddSingleton<ICachedReflectionItemsFactory, NonSynchronizedCachedReflectionItemsFactory>();

            services.AddSingleton(
                svcProv => LazyH.Lazy(
                    () => svcProv.GetRequiredService<ICachedReflectionItemsFactory>()));

            services.AddSingleton(
                svcProv => svcProv.GetRequiredService<ICachedTypesMapFactory>().Create());

            services.AddTransient<IDriveExplorerService, DriveExplorerService>();
        }
    }
}
