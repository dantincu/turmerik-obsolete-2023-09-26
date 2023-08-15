using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;

namespace Turmerik.AspNetCore.Infrastucture
{
    public class ClientAppSettingsCore<TClnbl>
    {
        public interface IClnblCore
        {
            string TrmrkPrefix { get; }
            decimal MaxDecimalValue { get; }
            decimal MinDecimalValue { get; }
        }
    }

    public class ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.ImmtblCore, TClnbl
        where TMtbl : ClientAppSettingsCore<TClnbl, TImmtbl, TMtbl>.MtblCore, TClnbl
    {
        public interface IClnblCore : ClientAppSettingsCore<TClnbl>.IClnblCore
        {
        }

        public class ImmtblCore : IClnblCore
        {
            public ImmtblCore(TClnbl src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
                MaxDecimalValue = src.MaxDecimalValue;
                MinDecimalValue = src.MinDecimalValue;
            }

            public string TrmrkPrefix { get; }
            public decimal MaxDecimalValue { get; }
            public decimal MinDecimalValue { get; }
        }

        public class MtblCore : IClnblCore
        {
            public MtblCore()
            {
            }

            public MtblCore(TClnbl src)
            {
                TrmrkPrefix = src.TrmrkPrefix;
                MaxDecimalValue = src.MaxDecimalValue;
                MinDecimalValue = src.MinDecimalValue;
            }

            public string TrmrkPrefix { get; set; }
            public decimal MaxDecimalValue { get; set; }
            public decimal MinDecimalValue { get; set; }
        }
    }
}
