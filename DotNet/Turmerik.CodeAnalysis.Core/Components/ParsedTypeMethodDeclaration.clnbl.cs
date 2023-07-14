using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public static partial class ParsedTypeMethodDeclaration
    {
        public interface IClnbl : ParsedTypeMemberDeclaration.IClnbl
        {
            IEnumerable<ParsedParameterDefinition.IClnbl> GetParameters();
        }

        public class Immtbl : ParsedTypeMemberDeclaration.Immtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                Parameters = src.GetParameters().AsImmtblCllctn();
            }

            public ReadOnlyCollection<ParsedParameterDefinition.Immtbl> Parameters { get; }

            public IEnumerable<ParsedParameterDefinition.IClnbl> GetParameters() => Parameters;
        }

        public class Mtbl : ParsedTypeMemberDeclaration.Mtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                Parameters = src.GetParameters().AsMtblList();
            }

            public List<ParsedParameterDefinition.Mtbl> Parameters { get; }

            public IEnumerable<ParsedParameterDefinition.IClnbl> GetParameters() => Parameters;
        }
    }
}
