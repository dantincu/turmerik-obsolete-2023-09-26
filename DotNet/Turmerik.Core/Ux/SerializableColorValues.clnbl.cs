using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Utils;

namespace Turmerik.Ux
{
    public partial class SerializableColorValues : ClnblCore<SerializableColorValues.IClnbl, SerializableColorValues.Immtbl, SerializableColorValues.Mtbl>
    {
        public interface IClnbl : IClnblCore
        {
            sbyte Red { get; }
            sbyte Green { get; }
            sbyte Blue { get; }
            sbyte Alpha { get; }
        }

        public class Immtbl : ImmtblCoreBase, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                Red = src.Red;
                Green = src.Green;
                Blue = src.Blue;
                Alpha = src.Alpha;
            }

            public sbyte Red { get; }
            public sbyte Green { get; }
            public sbyte Blue { get; }
            public sbyte Alpha { get; }
        }

        public class Mtbl : MtblCoreBase, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                Red = src.Red;
                Green = src.Green;
                Blue = src.Blue;
                Alpha = src.Alpha;
            }

            public sbyte Red { get; set; }
            public sbyte Green { get; set; }
            public sbyte Blue { get; set; }
            public sbyte Alpha { get; }
        }
    }

    public partial class SerializableColor : ClnblCore<SerializableColor.IClnbl, SerializableColor.Immtbl, SerializableColor.Mtbl>
    {
        public interface IClnbl : IClnblCore
        {
            string RgbaHexCode { get; }

            SerializableColorValues.IClnbl GetValues();
        }

        public class Immtbl : ImmtblCoreBase, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                Values = src.GetValues().AsImmtbl();
            }

            public string RgbaHexCode { get; }
            public SerializableColorValues.Immtbl Values { get; }

            public SerializableColorValues.IClnbl GetValues() => Values;
        }

        public class Mtbl : MtblCoreBase, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                Values = src.GetValues().AsMtbl();
            }

            public string RgbaHexCode { get; set; }
            public SerializableColorValues.Mtbl Values { get; set; }

            public SerializableColorValues.IClnbl GetValues() => Values;
        }
    }
}
