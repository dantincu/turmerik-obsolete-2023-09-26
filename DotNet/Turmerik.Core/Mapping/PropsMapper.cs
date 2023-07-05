using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Reflection.Cache;

namespace Turmerik.Mapping
{
    public interface IPropsMapper
    {
        object MapTo(
            object srcObj,
            object destnObj = null,
            Type destnType = null,
            Type srcType = null);

        TDestn MapTo<TDestn>(
            object srcObj,
            TDestn destnObj = default,
            Type destnType = null,
            Type srcType = null);
    }

    public class PropsMapper : IPropsMapper
    {
        public PropsMapper(ITypesMappingCache typesMappingCache)
        {
            TypesMappingCache = typesMappingCache ?? throw new ArgumentNullException(nameof(typesMappingCache));
        }

        private ITypesMappingCache TypesMappingCache { get; }

        public object MapTo(
            object srcObj,
            object destnObj = null,
            Type destnType = null,
            Type srcType = null)
        {
            destnType = destnType ?? destnObj.GetType();
            srcType = srcType ?? srcObj.GetType();

            destnObj = destnObj ?? Activator.CreateInstance(destnType);
            var propPairs = TypesMappingCache.PropsCache.Get(srcType).Get(destnType);

            foreach (var pair in propPairs)
            {
                var value = pair.SrcProp.Data.GetValue(srcObj, null);
                pair.DestnProp.Data.SetValue(destnObj, value, null);
            }

            return destnObj;
        }

        public TDestn MapTo<TDestn>(
            object srcObj,
            TDestn destnObj = default,
            Type destnType = null,
            Type srcType = null)
        {
            if (destnObj == null)
            {
                if (destnType == null)
                {
                    destnType = typeof(TDestn);
                }
                
                destnObj = Activator.CreateInstance<TDestn>();
            }

            destnObj = (TDestn)MapTo(
                srcObj,
                (object)destnObj,
                destnType,
                srcType);

            return destnObj;
        }
    }
}
