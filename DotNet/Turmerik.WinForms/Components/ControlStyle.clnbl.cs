﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Utils;
using Turmerik.Collections;
using System.Windows.Forms;

namespace Turmerik.WinForms.Components
{
    public static class ControlStyle
    {
        public interface IClnbl
        {
            Color BackColor { get; }
            Color ForeColor { get; }
            Font Font { get; }
            float FontSize { get; }
            FontFamily FontFamily { get; }
            FontStyle FontStyle { get; }
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                BackColor = src.BackColor;
                ForeColor = src.ForeColor;
                Font = src.Font;
                FontSize = src.FontSize;
                FontFamily = src.FontFamily;
                FontStyle = src.FontStyle;
            }

            public Color BackColor { get; }
            public Color ForeColor { get; }
            public Font Font { get; }
            public float FontSize { get; }
            public FontFamily FontFamily { get; }
            public FontStyle FontStyle { get; }
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                BackColor = src.BackColor;
                ForeColor = src.ForeColor;
                Font = src.Font;
                FontSize = src.FontSize;
                FontFamily = src.FontFamily;
                FontStyle = src.FontStyle;
            }

            public Color BackColor { get; set; }
            public Color ForeColor { get; set; }
            public Font Font { get; set; }
            public float FontSize { get; set; }
            public FontFamily FontFamily { get; set; }
            public FontStyle FontStyle { get; set; }

            public static Mtbl FromControl<TControl>(TControl control)
                where TControl : Control
            {
                var font = control.Font;

                var mtbl = new Mtbl
                {
                    BackColor = control.BackColor,
                    ForeColor = control.ForeColor,
                    Font = font,
                    FontSize = font.Size,
                    FontFamily = font.FontFamily,
                    FontStyle = font.Style
                };

                return mtbl;
            }
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
            IDictionaryCore<TKey, IClnbl> src) => src as ReadOnlyDictionary<TKey, Immtbl> ?? (src as Dictionary<TKey, Mtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl> AsMtblDictnr<TKey>(
            IDictionaryCore<TKey, IClnbl> src) => src as Dictionary<TKey, Mtbl> ?? (src as ReadOnlyDictionary<TKey, Immtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsMtbl());

        public static IDictionaryCore<TKey, IClnbl> ToClnblDictnr<TKey>(
            this Dictionary<TKey, Mtbl> src) => (IDictionaryCore<TKey, IClnbl>)src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl>());

        public static IDictionaryCore<TKey, IClnbl> ToClnblDictnr<TKey>(
            this ReadOnlyDictionary<TKey, Immtbl> src) => (IDictionaryCore<TKey, IClnbl>)src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl>());
    }
}
