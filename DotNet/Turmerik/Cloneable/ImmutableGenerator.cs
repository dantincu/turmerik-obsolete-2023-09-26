using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;

namespace Turmerik.Cloneable
{
    public interface IImmutableGenerator
    {
    }

    public class ImmutableGenerator : IImmutableGenerator
    {
        public static Type CreateType(string typeName, Type interfaceType)
        {
            var assemblyName = new AssemblyName(typeName);
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(typeName);
            var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);
            typeBuilder.AddInterfaceImplementation(interfaceType);

            ILGenerator ilGenerator = null;

            foreach (var property in interfaceType.GetProperties())
            {
                var propertyBuilder = typeBuilder.DefineProperty(property.Name, PropertyAttributes.None, property.PropertyType, null);
                var fieldBuilder = typeBuilder.DefineField($"_{property.Name}", property.PropertyType, FieldAttributes.Private);

                var getMethodBuilder = typeBuilder.DefineMethod($"get_{property.Name}", MethodAttributes.Public | MethodAttributes.Virtual, property.PropertyType, Type.EmptyTypes);
                ilGenerator = getMethodBuilder.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                ilGenerator.Emit(OpCodes.Ret);
                propertyBuilder.SetGetMethod(getMethodBuilder);

                var setMethodBuilder = typeBuilder.DefineMethod($"set_{property.Name}", MethodAttributes.Public | MethodAttributes.Virtual, null, new[] { property.PropertyType });
                ilGenerator = setMethodBuilder.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Stfld, fieldBuilder);
                ilGenerator.Emit(OpCodes.Ret);
                propertyBuilder.SetSetMethod(setMethodBuilder);
            }

            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
            ilGenerator = constructorBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);

            foreach (var property in interfaceType.GetProperties())
            {
                ilGenerator.Emit(OpCodes.Ldstr, $"Default {property.Name}");
                ilGenerator.Emit(OpCodes.Callvirt, property.SetMethod);
            }

            ilGenerator.Emit(OpCodes.Ret);

            return typeBuilder.CreateType();
        }

    }
}
