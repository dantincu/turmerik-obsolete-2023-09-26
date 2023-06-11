﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.DriveExplorerCore;
using Turmerik.FileSystem;
using Turmerik.Reflection;
using Turmerik.Reflection.Cache;
using Turmerik.Synchronized;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.Dependencies
{
    public static class TrmrkCoreServiceCollectionBuilder
    {
        public static void RegisterAll(
            IServiceCollection services)
        {
            services.AddSingleton<ITimeStampHelper, TimeStampHelper>();
            services.AddSingleton<IFsPathNormalizer, FsPathNormalizer>();
            services.AddSingleton<ILambdaExprHelper, LambdaExprHelper>();
            services.AddSingleton<ILambdaExprHelperFactory, LambdaExprHelperFactory>();
            services.AddSingleton<IBasicEqualityComparerFactory, BasicEqualityComparerFactory>();

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

            services.AddSingleton(svcProv => LazyH.Lazy(
                    () => svcProv.GetRequiredService<ICachedReflectionItemsFactory>()));

            services.AddTransient<IDriveExplorerService, DriveExplorerService>();
        }
    }
}
