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
        public static ITrmrkCoreServiceCollection RegisterAll(
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

            var immtbl = new TrmrkCoreServiceCollectionImmtbl(mtbl);

            services.AddSingleton(immtbl.TimeStampHelper);
            services.AddSingleton(immtbl.FsPathNormalizer);
            services.AddSingleton(immtbl.LambdaExprHelper);
            services.AddSingleton(immtbl.LambdaExprHelperFactory);
            services.AddSingleton(immtbl.BasicEqualityComparerFactory);
            services.AddSingleton(immtbl.MutexCreator);
            services.AddSingleton(immtbl.OnceExecutedActionFactory);
            services.AddSingleton(immtbl.OnceExecutedAsyncActionFactory);
            services.AddSingleton(immtbl.DisposableComponentFactory);
            services.AddSingleton(immtbl.AsyncDisposableComponentFactory);
            services.AddSingleton(immtbl.InterProcessConcurrentActionComponentFactory);

            services.AddTransient<IThreadSafeActionComponent, ThreadSafeActionComponent>();
            services.AddTransient<IAsyncActionsQueue, AsyncActionsQueue>();
            services.AddTransient<IDriveExplorerService, DriveExplorerService>();

            return immtbl;
        }
    }
}
