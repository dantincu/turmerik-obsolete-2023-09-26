﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Collections;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace Turmerik.Core.Text
{
    public static class JsonH
    {
        public static string ToJson(
            object obj,
            bool useCamelCase = true,
            bool ignoreNullValues = true,
            Formatting formatting = Formatting.Indented)
        {
            var settings = new JsonSerializerSettings();

            if (ignoreNullValues)
            {
                settings.NullValueHandling = NullValueHandling.Ignore;
            }

            if (useCamelCase)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            string json = JsonConvert.SerializeObject(
                obj,
                formatting,
                settings);

            return json;
        }

        public static TData? FromJson<TData>(string json)
        {
            TData? data = JsonConvert.DeserializeObject<TData>(json);
            return data;
        }

        public static JToken? TryGetToken(
            this JObject jObject,
            string[] altPropNames)
        {
            JToken? token = null;

            foreach (var propName in altPropNames)
            {
                token = jObject.GetValue(propName);

                if (token != null)
                {
                    break;
                }
            }

            return token;
        }

        public static JToken? TryGetToken(
            this JObject jObject,
            string propName,
            bool tryCamelCaseIfNotFound = false)
        {
            string[]? altPropNames = null;

            if (tryCamelCaseIfNotFound)
            {
                string camelCasePropName = propName.DecapitalizeFirstLetter();

                if (camelCasePropName != propName)
                {
                    altPropNames = propName.Arr(camelCasePropName);
                }
            }

            altPropNames = altPropNames ?? propName.Arr();
            var token = jObject.TryGetToken(altPropNames);

            return token;
        }

        public static JToken? TryGetToken(
            this JObject jObject,
            JsonPropRetrieverOpts opts) => jObject.TryGetToken(
                opts.PropName,
                opts.TryCamelCaseIfNotFound ?? false);

        public static TValue GetValueOrDefault<TValue>(
            this JToken token,
            Func<TValue> defaultPropValFactory = null)
        {
            TValue retVal;

            if (token != null)
            {
                retVal = token.ToObject<TValue>();
            }
            else if (defaultPropValFactory != null)
            {
                retVal = defaultPropValFactory();
            }
            else
            {
                retVal = default;
            }

            return retVal;
        }

        public static TValue TryGetValue<TValue>(
            this JObject jObject,
            string[] altPropNames,
            Func<TValue> defaultPropValFactory = null) => jObject.TryGetToken(
                altPropNames).GetValueOrDefault(
                defaultPropValFactory);

        public static TValue TryGetValue<TValue>(
            this JObject jObject,
            string propName,
            Func<TValue> defaultPropValFactory = null) => jObject.TryGetToken(
                propName).GetValueOrDefault(
                defaultPropValFactory);

        public static TValue TryGetValue<TValue>(
            this JObject jObject,
            string propName,
            bool tryCamelCaseIfNotFound,
            Func<TValue> defaultPropValFactory = null) => jObject.TryGetToken(
                propName,
                tryCamelCaseIfNotFound).GetValueOrDefault(
                defaultPropValFactory);

        public static TVal TryGetValue<TVal>(
            this JObject jObject,
            JsonPropRetrieverOpts opts) => jObject.TryGetValue<TVal>(
                opts.PropName,
                opts.TryCamelCaseIfNotFound ?? false);
    }

    public class JsonPropRetrieverOpts
    {
        public string PropName { get; set; }
        public bool? TryCamelCaseIfNotFound { get; set; }
    }
}
