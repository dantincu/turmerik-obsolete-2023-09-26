using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.DriveExplorerCore;
using Turmerik.FileSystem;
using Turmerik.Reflection;
using Turmerik.Synchronized;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.Dependencies
{
    public static class TrmrkCoreServiceCollectionBuilder
    {
        public static TrmrkCoreServiceCollectionMtbl RegisterAll(
            IServiceCollection services,
            TrmrkCoreServiceCollectionMtbl mtbl = null)
        {
            mtbl = mtbl ?? new TrmrkCoreServiceCollectionMtbl();

            mtbl.TimeStampHelper ??= new TimeStampHelper();
            mtbl.FsPathNormalizer ??= new FsPathNormalizer();
            mtbl.LambdaExprHelper ??= new LambdaExprHelper();

            mtbl.LambdaExprHelperFactory ??= new LambdaExprHelperFactory(
                mtbl.LambdaExprHelper);

            mtbl.BasicEqualityComparerFactory ??= new BasicEqualityComparerFactory();
            mtbl.MutexCreator ??= new MutexCreator();

            mtbl.OnceExecutedActionFactory ??= new OnceExecutedActionFactory();
            mtbl.OnceExecutedAsyncActionFactory ??= new OnceExecutedAsyncActionFactory();

            mtbl.DisposableComponentFactory ??= new DisposableComponentFactory(
                mtbl.OnceExecutedActionFactory);

            mtbl.AsyncDisposableComponentFactory ??= new AsyncDisposableComponentFactory(
                mtbl.OnceExecutedAsyncActionFactory);

            mtbl.InterProcessConcurrentActionComponentFactory ??= new InterProcessConcurrentActionComponentFactory(
                mtbl.DisposableComponentFactory,
                mtbl.MutexCreator);

            services.AddSingleton(mtbl.TimeStampHelper);
            services.AddSingleton(mtbl.FsPathNormalizer);
            services.AddSingleton(mtbl.LambdaExprHelper);
            services.AddSingleton(mtbl.LambdaExprHelperFactory);
            services.AddSingleton(mtbl.BasicEqualityComparerFactory);
            services.AddSingleton(mtbl.MutexCreator);
            services.AddSingleton(mtbl.OnceExecutedActionFactory);
            services.AddSingleton(mtbl.OnceExecutedAsyncActionFactory);
            services.AddSingleton(mtbl.DisposableComponentFactory);
            services.AddSingleton(mtbl.AsyncDisposableComponentFactory);
            services.AddSingleton(mtbl.InterProcessConcurrentActionComponentFactory);

            services.AddTransient<IThreadSafeActionComponent, ThreadSafeActionComponent>();
            services.AddTransient<IAsyncActionsQueue, AsyncActionsQueue>();
            services.AddTransient<IDriveExplorerService, DriveExplorerService>();

            return mtbl;
        }
    }
}
