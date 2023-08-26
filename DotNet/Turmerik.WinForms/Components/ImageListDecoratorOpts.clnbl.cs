using Jint.Native;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Cloneable;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.WinForms.Components
{
    public static class ImageListDecoratorOpts
    {
        public interface IClnbl
        {
            ImageList ImageList { get; }

            IEnumerable<KeyValuePair<string, Image>> GetImageMap();
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                ImageList = src.ImageList;
                ImageMap = src.GetImageMap()?.ToDictnr().RdnlD();
            }

            public ImageList ImageList { get; }

            public ReadOnlyDictionary<string, Image> ImageMap { get; }

            public IEnumerable<KeyValuePair<string, Image>> GetImageMap() => ImageMap;
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                ImageList = src.ImageList;
                ImageMap = src.GetImageMap()?.ToDictnr();
            }

            public ImageList ImageList { get; set; }

            public Dictionary<string, Image> ImageMap { get; set; }

            public IEnumerable<KeyValuePair<string, Image>> GetImageMap() => ImageMap;
        }

        public static Immtbl ToImmtbl(
            this IClnbl src) => new Immtbl(src);

        public static Immtbl AsImmtbl(
            this IClnbl src) => src as Immtbl ?? src?.ToImmtbl();

        public static Mtbl ToMtbl(
            this IClnbl src) => new Mtbl(src);

        public static Mtbl AsMtbl(
            this IClnbl src) => src as Mtbl ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl> ToImmtblCllctn(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl> AsImmtblCllctn(
            this IEnumerable<IClnbl> src) =>
            src as ReadOnlyCollection<Immtbl> ?? src?.ToImmtblCllctn();

        public static List<Mtbl> ToMtblList(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl> AsMtblList(
            this IEnumerable<IClnbl> src) => src as List<Mtbl> ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl> AsImmtblDictnr<TKey>(
            IEnumerable<KeyValuePair<TKey, IClnbl>> src) => src as ReadOnlyDictionary<TKey, Immtbl> ?? (src as Dictionary<TKey, Mtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl> AsMtblDictnr<TKey>(
            IEnumerable<KeyValuePair<TKey, IClnbl>> src) => src as Dictionary<TKey, Mtbl> ?? (src as ReadOnlyDictionary<TKey, Immtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsMtbl());

        public static IEnumerable<KeyValuePair<TKey, IClnbl>> ToClnblDictnr<TKey>(
            this Dictionary<TKey, Mtbl> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl>());

        public static IEnumerable<KeyValuePair<TKey, IClnbl>> ToClnblDictnr<TKey>(
            this ReadOnlyDictionary<TKey, Immtbl> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl>());
    }
}
