﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Collections;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public static partial class ParsedTypeDefinition
    {
        public interface IClnbl
        {
            string Name { get; }
            bool IsPartial { get; }
            bool IsInterface { get; }
            bool IsClass { get; }

            ParsedClassDefinition.IClnbl GetNestedParentClass();
            ParsedTypeOrMemberIdentifier.IClnbl GetInheritedParentClass();

            IEnumerable<ParsedCsKeyword> GetModifiers();
            IEnumerable<ParsedAttributeDecoration.IClnbl> GetAttributes();
            IEnumerable<ParsedGenericTypeParameter.IClnbl> GetGenericTypeParameters();
            IEnumerable<ParsedTypeOrMemberIdentifier.IClnbl> GetInterfaces();
            IEnumerable<IClnbl> GetNestedTypes();

            IEnumerable<ParsedTypeMemberDeclaration.IClnbl> GetMemberDeclarations();
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                Name = src.Name;
                IsPartial = src.IsPartial;
                IsInterface = src.IsInterface;
                IsClass = src.IsClass;

                NestedParentClass = src.GetNestedParentClass().AsImmtbl();
                InheritedParentClass = src.GetInheritedParentClass().AsImmtbl();

                Modifiers = src.GetModifiers()?.RdnlC();
                Attributes = src.GetAttributes().AsImmtblCllctn();
                GenericTypeParameters = src.GetGenericTypeParameters().AsImmtblCllctn();
                Interfaces = src.GetInterfaces().AsImmtblCllctn();
                NestedTypes = src.GetNestedTypes().AsImmtblCllctn();
                MemberDeclarations = src.GetMemberDeclarations().AsImmtblCllctn();
            }

            public string Name { get; }
            public bool IsPartial { get; }
            public bool IsInterface { get; }
            public bool IsClass { get; }

            public ParsedClassDefinition.Immtbl NestedParentClass { get; }
            public ParsedTypeOrMemberIdentifier.Immtbl InheritedParentClass { get; }

            public ReadOnlyCollection<ParsedCsKeyword> Modifiers { get; }
            public ReadOnlyCollection<ParsedAttributeDecoration.Immtbl> Attributes { get; }
            public ReadOnlyCollection<ParsedGenericTypeParameter.Immtbl> GenericTypeParameters { get; }
            public ReadOnlyCollection<ParsedTypeOrMemberIdentifier.Immtbl> Interfaces { get; }
            public ReadOnlyCollection<Immtbl> NestedTypes { get; }
            public ReadOnlyCollection<ParsedTypeMemberDeclaration.Immtbl> MemberDeclarations { get; }

            public ParsedClassDefinition.IClnbl GetNestedParentClass() => NestedParentClass;
            public ParsedTypeOrMemberIdentifier.IClnbl GetInheritedParentClass() => InheritedParentClass;

            public IEnumerable<ParsedCsKeyword> GetModifiers() => Modifiers;
            public IEnumerable<ParsedAttributeDecoration.IClnbl> GetAttributes() => Attributes;
            public IEnumerable<ParsedGenericTypeParameter.IClnbl> GetGenericTypeParameters() => GenericTypeParameters;
            public IEnumerable<ParsedTypeOrMemberIdentifier.IClnbl> GetInterfaces() => Interfaces;
            public IEnumerable<IClnbl> GetNestedTypes() => NestedTypes;

            public IEnumerable<ParsedTypeMemberDeclaration.IClnbl> GetMemberDeclarations() => MemberDeclarations;
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                Name = src.Name;
                IsPartial = src.IsPartial;
                IsInterface = src.IsInterface;
                IsClass = src.IsClass;

                NestedParentClass = src.GetNestedParentClass().AsMtbl();
                InheritedParentClass = src.GetInheritedParentClass().AsMtbl();

                Modifiers = src.GetModifiers()?.ToList();
                Attributes = src.GetAttributes().AsMtblList();
                GenericTypeParameters = src.GetGenericTypeParameters().AsMtblList();
                Interfaces = src.GetInterfaces().AsMtblList();
                NestedTypes = src.GetNestedTypes().AsMtblList();
                MemberDeclarations = src.GetMemberDeclarations().AsMtblList();
            }

            public string Name { get; set; }
            public bool IsPartial { get; set; }
            public bool IsInterface { get; set; }
            public bool IsClass { get; set; }

            public ParsedClassDefinition.Mtbl NestedParentClass { get; set; }
            public ParsedTypeOrMemberIdentifier.Mtbl InheritedParentClass { get; set; }

            public List<ParsedCsKeyword> Modifiers { get; set; }
            public List<ParsedAttributeDecoration.Mtbl> Attributes { get; set; }
            public List<ParsedGenericTypeParameter.Mtbl> GenericTypeParameters { get; set; }
            public List<ParsedTypeOrMemberIdentifier.Mtbl> Interfaces { get; set; }
            public List<Mtbl> NestedTypes { get; set; }
            public List<ParsedTypeMemberDeclaration.Mtbl> MemberDeclarations { get; set; }

            public ParsedClassDefinition.IClnbl GetNestedParentClass() => NestedParentClass;
            public ParsedTypeOrMemberIdentifier.IClnbl GetInheritedParentClass() => InheritedParentClass;

            public IEnumerable<ParsedCsKeyword> GetModifiers() => Modifiers;
            public IEnumerable<ParsedAttributeDecoration.IClnbl> GetAttributes() => Attributes;
            public IEnumerable<ParsedGenericTypeParameter.IClnbl> GetGenericTypeParameters() => GenericTypeParameters;
            public IEnumerable<ParsedTypeOrMemberIdentifier.IClnbl> GetInterfaces() => Interfaces;
            public IEnumerable<IClnbl> GetNestedTypes() => NestedTypes;

            public IEnumerable<ParsedTypeMemberDeclaration.IClnbl> GetMemberDeclarations() => MemberDeclarations;
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
    }
}