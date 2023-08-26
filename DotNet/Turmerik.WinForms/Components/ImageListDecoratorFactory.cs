using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Reflection.Cache;
using Turmerik.Utils;

namespace Turmerik.WinForms.Components
{
    public interface IImageListDecoratorFactory
    {
        TDecorator Create<TDecorator, TDecoratorOpts>(
            TDecoratorOpts opts)
            where TDecorator : ImageListDecoratorBase
            where TDecoratorOpts : ImageListDecoratorOpts.IClnbl;

        TDecorator Create<TDecorator>(
            ImageListDecoratorOpts.IClnbl opts)
            where TDecorator : ImageListDecoratorBase;
    }

    public class ImageListDecoratorFactory : IImageListDecoratorFactory
    {
        public ImageListDecoratorFactory(
            ICachedTypesMap cachedTypesMap)
        {
            CachedTypesMap = cachedTypesMap ?? throw new ArgumentNullException(nameof(cachedTypesMap));
        }

        protected ICachedTypesMap CachedTypesMap { get; }

        public virtual TDecorator Create<TDecorator, TDecoratorOpts>(
            TDecoratorOpts opts)
            where TDecorator : ImageListDecoratorBase
            where TDecoratorOpts : ImageListDecoratorOpts.IClnbl => CachedTypesMap.CreateInstance<TDecorator>(
                null, opts);

        public virtual TDecorator Create<TDecorator>(
            ImageListDecoratorOpts.IClnbl opts)
            where TDecorator : ImageListDecoratorBase => CachedTypesMap.CreateInstance<TDecorator>(
                null, opts);
    }
}
