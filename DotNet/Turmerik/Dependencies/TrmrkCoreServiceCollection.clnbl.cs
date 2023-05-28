using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Reflection;
using Turmerik.Synchronized;
using Turmerik.Text;
using Turmerik.FileSystem;
using Turmerik.Utils;

namespace Turmerik.Dependencies
{
    public interface ITrmrkCoreServiceCollection
    {
        ITimeStampHelper TimeStampHelper { get; }
        IFsPathNormalizer FsPathNormalizer { get; }
        ILambdaExprHelper LambdaExprHelper { get; }
        ILambdaExprHelperFactory LambdaExprHelperFactory { get; }
        IBasicEqualityComparerFactory BasicEqualityComparerFactory { get; }
        IMutexCreator MutexCreator { get; }
        IOnceExecutedActionFactory OnceExecutedActionFactory { get; }
        IOnceExecutedAsyncActionFactory OnceExecutedAsyncActionFactory { get; }
        IDisposableComponentFactory DisposableComponentFactory { get; }
        IAsyncDisposableComponentFactory AsyncDisposableComponentFactory { get; }
        IInterProcessConcurrentActionComponentFactory InterProcessConcurrentActionComponentFactory { get; }
    }

    public class TrmrkCoreServiceCollectionImmtbl : ITrmrkCoreServiceCollection
    {
        public TrmrkCoreServiceCollectionImmtbl(ITrmrkCoreServiceCollection src)
        {
            TimeStampHelper = src.TimeStampHelper;
            FsPathNormalizer = src.FsPathNormalizer;
            LambdaExprHelper = src.LambdaExprHelper;
            LambdaExprHelperFactory = src.LambdaExprHelperFactory;
            BasicEqualityComparerFactory = src.BasicEqualityComparerFactory;
            MutexCreator = src.MutexCreator;
            OnceExecutedActionFactory = src.OnceExecutedActionFactory;
            OnceExecutedAsyncActionFactory = src.OnceExecutedAsyncActionFactory;
            DisposableComponentFactory = src.DisposableComponentFactory;
            AsyncDisposableComponentFactory = src.AsyncDisposableComponentFactory;
            InterProcessConcurrentActionComponentFactory = src.InterProcessConcurrentActionComponentFactory;
        }

        public ITimeStampHelper TimeStampHelper { get; }
        public IFsPathNormalizer FsPathNormalizer { get; }
        public ILambdaExprHelper LambdaExprHelper { get; }
        public ILambdaExprHelperFactory LambdaExprHelperFactory { get; }
        public IBasicEqualityComparerFactory BasicEqualityComparerFactory { get; }
        public IMutexCreator MutexCreator { get; }
        public IOnceExecutedActionFactory OnceExecutedActionFactory { get; }
        public IOnceExecutedAsyncActionFactory OnceExecutedAsyncActionFactory { get; }
        public IDisposableComponentFactory DisposableComponentFactory { get; }
        public IAsyncDisposableComponentFactory AsyncDisposableComponentFactory { get; }
        public IInterProcessConcurrentActionComponentFactory InterProcessConcurrentActionComponentFactory { get; }
        public IAsyncActionsQueue AsyncActionsQueue { get; }
    }

    public class TrmrkCoreServiceCollectionMtbl : ITrmrkCoreServiceCollection
    {
        public TrmrkCoreServiceCollectionMtbl()
        {
        }

        public TrmrkCoreServiceCollectionMtbl(ITrmrkCoreServiceCollection src)
        {
            TimeStampHelper = src.TimeStampHelper;
            FsPathNormalizer = src.FsPathNormalizer;
            LambdaExprHelper = src.LambdaExprHelper;
            LambdaExprHelperFactory = src.LambdaExprHelperFactory;
            BasicEqualityComparerFactory = src.BasicEqualityComparerFactory;
            MutexCreator = src.MutexCreator;
            OnceExecutedActionFactory = src.OnceExecutedActionFactory;
            OnceExecutedAsyncActionFactory = src.OnceExecutedAsyncActionFactory;
            DisposableComponentFactory = src.DisposableComponentFactory;
            AsyncDisposableComponentFactory = src.AsyncDisposableComponentFactory;
            InterProcessConcurrentActionComponentFactory = src.InterProcessConcurrentActionComponentFactory;
        }

        public ITimeStampHelper TimeStampHelper { get; set; }
        public IFsPathNormalizer FsPathNormalizer { get; set; }
        public ILambdaExprHelper LambdaExprHelper { get; set; }
        public ILambdaExprHelperFactory LambdaExprHelperFactory { get; set; }
        public IBasicEqualityComparerFactory BasicEqualityComparerFactory { get; set; }
        public IMutexCreator MutexCreator { get; set; }
        public IOnceExecutedActionFactory OnceExecutedActionFactory { get; set; }
        public IOnceExecutedAsyncActionFactory OnceExecutedAsyncActionFactory { get; set; }
        public IDisposableComponentFactory DisposableComponentFactory { get; set; }
        public IAsyncDisposableComponentFactory AsyncDisposableComponentFactory { get; set; }
        public IInterProcessConcurrentActionComponentFactory InterProcessConcurrentActionComponentFactory { get; set; }
    }
}
