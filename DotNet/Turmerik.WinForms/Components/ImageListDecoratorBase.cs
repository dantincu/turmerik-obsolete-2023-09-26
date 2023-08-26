using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Cache;
using Turmerik.Collections;
using Turmerik.Reflection.Cache;

namespace Turmerik.WinForms.Components
{
    public abstract class ImageListDecoratorBase
    {
        public ImageListDecoratorBase(
            ICachedTypesMap cachedTypesMap,
            ImageListDecoratorOpts.IClnbl opts)
        {
            CachedTypesMap = cachedTypesMap ?? throw new ArgumentNullException(
                nameof(cachedTypesMap));

            ThisType = CachedTypesMap.Get(GetType());
            IdxProps = GetIdxProps();

            var optsImmtbl = NormalizeOpts(opts.AsMtbl());

            ImageListCore = optsImmtbl.ImageList ?? throw new ArgumentNullException(
                nameof(optsImmtbl.ImageList));

            ImageMap = optsImmtbl.ImageMap ?? throw new ArgumentNullException(
                nameof(optsImmtbl.ImageMap));

            ImagePropNames = AddImages(
                ImageListCore,
                ImageMap);

            AssignIdxProps(
                ImagePropNames);
        }

        protected ICachedTypesMap CachedTypesMap { get; }
        protected ICachedTypeInfo ThisType { get; }
        protected ReadOnlyCollection<ICachedPropertyInfo> IdxProps { get; }
        protected ImageList ImageListCore { get; }
        protected ReadOnlyDictionary<string, Image> ImageMap { get; }
        protected ReadOnlyCollection<string> ImagePropNames { get; }

        private ImageListDecoratorOpts.Immtbl NormalizeOpts(
            ImageListDecoratorOpts.Mtbl opts) => opts.AsImmtbl();

        private ReadOnlyCollection<string> AddImages(
            ImageList imageList,
            ReadOnlyDictionary<string, Image> imageMap)
        {
            var imagePropNames = imageMap.Keys.ToArray().RdnlC();
            imageList.Images.AddRange(imageMap.Values.ToArray());

            return imagePropNames;
        }

        private void AssignIdxProps(
            ReadOnlyCollection<string> imagePropNames)
        {
            for (int idx = 0; idx < imagePropNames.Count; idx++)
            {
                var propName = imagePropNames[idx];
                AssignIdxProp(propName, idx);
            }
        }

        private void AssignIdxProp(
            string propName,
            int idx)
        {
            var propInfo = IdxProps.SingleOrDefault(
                prop => prop.Name == propName);

            propInfo?.Data.SetValue(this, idx, null);
        }

        private ReadOnlyCollection<ICachedPropertyInfo> GetIdxProps(
            ) => ThisType.InstanceProps.Value.AllVisible.Value.Filtered.Get(
                new Reflection.PropertyAccessibilityFilter(
                Reflection.MemberScope.Instance, true, true,
                Reflection.MemberVisibility.Public,
                Reflection.MemberVisibility.Protected));
    }
}
