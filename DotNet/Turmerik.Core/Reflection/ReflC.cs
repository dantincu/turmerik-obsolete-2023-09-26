using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Reflection
{
    public static class ReflC
    {
        public static class Types
        {
            public static readonly Type ObjectType = typeof(object);
            public static readonly Type StringType = typeof(string);

            public static class Primitives
            {
                public static readonly Type BoolType = typeof(bool);
                public static readonly Type Int32Type = typeof(int);
                public static readonly Type UInt32Type = typeof(uint);
                public static readonly Type Int64Type = typeof(long);
                public static readonly Type UInt64Type = typeof(ulong);
                public static readonly Type Int16Type = typeof(short);
                public static readonly Type UInt16Type = typeof(short);
                public static readonly Type ByteType = typeof(byte);
                public static readonly Type SByteType = typeof(sbyte);
                public static readonly Type DateTimeType = typeof(DateTime);
                public static readonly Type DateTimeOffsetType = typeof(DateTimeOffset);
                public static readonly Type TimeSpanType = typeof(TimeSpan);
            }

            public static class PrimitiveNullables
            {
                public static readonly Type BoolType = typeof(bool?);
                public static readonly Type Int32Type = typeof(int?);
                public static readonly Type UInt32Type = typeof(uint?);
                public static readonly Type Int64Type = typeof(long?);
                public static readonly Type UInt64Type = typeof(ulong?);
                public static readonly Type Int16Type = typeof(short?);
                public static readonly Type UInt16Type = typeof(short?);
                public static readonly Type ByteType = typeof(byte?);
                public static readonly Type SByteType = typeof(sbyte?);
                public static readonly Type DateTimeType = typeof(DateTime?);
                public static readonly Type DateTimeOffsetType = typeof(DateTimeOffset?);
                public static readonly Type TimeSpanType = typeof(TimeSpan?);
            }

            public static class GenericDefs
            {
                public static readonly Type KeyValuePairGenDef = typeof(KeyValuePair<int, int>).GetGenericTypeDefinition();
                public static readonly Type NullableGenDef = typeof(int?).GetGenericTypeDefinition();
                public static readonly Type EnumerableGenDef = typeof(IEnumerable<int>).GetGenericTypeDefinition();

                public static readonly Type ArrayGenDef = typeof(int[]).GetGenericTypeDefinition();

                public static readonly Type ListIntfGenDef = typeof(IList<int>).GetGenericTypeDefinition();
                public static readonly Type ListGenDef = typeof(List<int>).GetGenericTypeDefinition();

                public static readonly Type CllctnIntfGenDef = typeof(ICollection<int>).GetGenericTypeDefinition();
                public static readonly Type CllctnGenDef = typeof(Collection<int>).GetGenericTypeDefinition();

                public static readonly Type RdnlCllctnIntfGenDef = typeof(IReadOnlyCollection<int>).GetGenericTypeDefinition();
                public static readonly Type RdnlCllctnGenDef = typeof(ReadOnlyCollection<int>).GetGenericTypeDefinition();

                public static readonly Type DictnrIntfGenDef = typeof(IDictionary<int, int>).GetGenericTypeDefinition();
                public static readonly Type DictnrGenDef = typeof(Dictionary<int, int>).GetGenericTypeDefinition();

                public static readonly Type RdnlDictnrIntfGenDef = typeof(IReadOnlyDictionary<int, int>).GetGenericTypeDefinition();
                public static readonly Type RdnlDictnrGenDef = typeof(ReadOnlyDictionary<int, int>).GetGenericTypeDefinition();
            }
        }

        public static class PropNames
        {
            public static class KeyValuePair
            {
                public static readonly string Key = nameof(KeyValuePair<int, int>.Key);
                public static readonly string Value = nameof(KeyValuePair<int, int>.Value);
            }
        }

        public static class MethodNames
        {
            public static class Dictionary
            {
                public static readonly string Add = nameof(Dictionary<int, int>.Add);
            }
        }

        public static class Filter
        {
            public static readonly MemberVisibility AllVisibilities = GetAllVisibilities();
            public static readonly MemberScope InstanceOrStatic = GetInstanceOrStatic();
            public static readonly FieldType InitOnlyOrLiteral = GetConstantFieldType();
            public static readonly FieldType FieldTypeCatchAll = GetFieldTypeCatchAll();
            public static readonly BindingFlags AllDeclaredOnlyBindingFlags = GetAllDeclaredOnlyBindingFlags();

            public static MemberVisibility GetAllVisibilities()
            {
                var visibility = MemberVisibility.Public | MemberVisibility.Protected;
                visibility |= MemberVisibility.Internal | MemberVisibility.ProtectedInternal;
                visibility |= MemberVisibility.Private | MemberVisibility.PrivateProtected;

                return visibility;
            }

            public static MemberScope GetInstanceOrStatic() => MemberScope.Instance | MemberScope.Static;
            public static FieldType GetConstantFieldType() => FieldType.InitOnly | FieldType.Literal;
            public static FieldType GetFieldTypeCatchAll() => FieldType.Editable | FieldType.InitOnly | FieldType.Literal;

            public static BindingFlags GetAllDeclaredOnlyBindingFlags() => BindingFlags.Instance | (
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
        }
    }
}
