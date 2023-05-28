using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Utils;

namespace Turmerik.Core.Ux
{
    public interface ISerializableColorValues
    {
        sbyte Red { get; }
        sbyte Green { get; }
        sbyte Blue { get; }
        sbyte Alpha { get; }
    }
    public interface ISerializableColor
    {
        string RgbaHexCode { get; }

        ISerializableColorValues GetValues();
    }

    public class SerializableColorValuesImmtbl : ISerializableColorValues
    {
        public SerializableColorValuesImmtbl(ISerializableColorValues src)
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

    public class SerializableColorValuesMtbl : ISerializableColorValues
    {
        public SerializableColorValuesMtbl()
        {
        }

        public SerializableColorValuesMtbl(ISerializableColorValues src)
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

    public class SerializableColorImmtbl : ISerializableColor
    {
        public SerializableColorImmtbl(ISerializableColor src)
        {
            Values = src.GetValues()?.CreateInstance<SerializableColorValuesImmtbl>();
        }

        public string RgbaHexCode { get; }
        public SerializableColorValuesImmtbl Values { get; }

        public ISerializableColorValues GetValues() => Values;
    }

    public class SerializableColorMtbl : ISerializableColor
    {
        public SerializableColorMtbl()
        {
        }

        public SerializableColorMtbl(ISerializableColor src)
        {
            Values = src.GetValues()?.CreateInstance<SerializableColorValuesMtbl>();
        }

        public string RgbaHexCode { get; set; }
        public SerializableColorValuesMtbl Values { get; set; }

        public ISerializableColorValues GetValues() => Values;
    }
}
