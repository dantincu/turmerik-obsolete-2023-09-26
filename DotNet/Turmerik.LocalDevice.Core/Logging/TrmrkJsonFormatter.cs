﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Turmerik.Text;
using Turmerik.Utils;
using LE = Turmerik.LocalDevice.Core.Logging.SerializedLogEvent;

namespace Turmerik.LocalDevice.Core.Logging
{
    public class TrmrkJsonFormatter : ITextFormatter
    {
        private const string INDENT_STR = "  ";
        private static readonly int indentStrLen = INDENT_STR.Length;

        private readonly ITimeStampHelper timeStampHelper;
        private readonly IExceptionSerializer exceptionSerializer;
        private readonly StringEnumConverter stringEnumConverter;
        private readonly JsonSerializerSettings settings;
        
        public TrmrkJsonFormatter(
            ITimeStampHelper timeStampHelper,
            IExceptionSerializer exceptionSerializer)
        {
            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
            this.exceptionSerializer = exceptionSerializer ?? throw new ArgumentNullException(nameof(exceptionSerializer));

            stringEnumConverter = new StringEnumConverter();
            settings = GetJsonSerializerSettings();
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            var args = new Args(logEvent, output);

            WriteLogStart(args);
            WriteTimeStampStr(args);
            WriteLogLevel(args);

            WriteMessage(args);
            WriteObjects(args);
            WriteLogEnd(args);
        }

        private void WriteLogStart(Args args)
        {
            Write(args, "{", true, true);
        }

        private void WriteLogEnd(Args args)
        {
            args.DecreaseIndent();
            Write(args, "}", startWithComma: false);
        }

        private void WriteTimeStampStr(Args args)
        {
            var logEvent = args.LogEvent;

            string timeStampStr = timeStampHelper.TmStmp(
                logEvent.Timestamp.LocalDateTime,
                true,
                TimeStamp.Ticks,
                true);

            WriteStrPropVal(
                args,
                nameof(LE.TimeStamp),
                timeStampStr,
                startWithComma: false);
        }

        private void WriteLogLevel(Args args)
        {
            WriteStrPropVal(args, nameof(LE.Level), args.LogEvent.Level.ToString());
        }

        private void WriteMessage(Args args)
        {
            var logEvent = args.LogEvent;

            if (logEvent is TrmrkLogEvent trmrkLogEvent)
            {
                var msgParts = new List<string>();

                foreach (TrmrkMessageTemplateToken token in trmrkLogEvent.MessageTemplate.Tokens)
                {
                    using (StringWriter sw = new StringWriter())
                    {
                        token.Render(trmrkLogEvent.Properties, sw);
                        string msg = sw.ToString();

                        msgParts.Add(msg);
                    }
                }

                string msgStr = string.Concat(msgParts);

                WriteStrPropVal(
                    args,
                    nameof(LE.Message),
                    msgStr);
            }
            else
            {
                string msg = logEvent.MessageTemplate.Render(
                    logEvent.Properties,
                    CultureInfo.InvariantCulture);

                WriteStrPropVal(args, nameof(LE.Message), msg);
            }
        }

        private void WriteObjects(Args args)
        {
            var logEvent = args.LogEvent;

            WriteObjectIfNotNull(
                args,
                nameof(LE.Data),
                (logEvent as TrmrkLogEvent)?.Data);

            WriteObjectIfNotNull(
                args,
                nameof(LE.Exception),
                logEvent.Exception?.WithValue(
                    exc => exceptionSerializer.SerializeException(exc)));
        }

        private void Write(
            Args args,
            string text,
            bool startWithNewLine = true,
            bool? increaseIndentAfter = null,
            bool startWithComma = true)
        {
            if (startWithComma)
            {
                args.Output.Write(",");
            }

            text = string.Concat(args.IndentStr, text);

            if (startWithNewLine)
            {
                args.Output.WriteLine();
            }

            args.Output.Write(text);

            if (increaseIndentAfter.HasValue)
            {
                if (increaseIndentAfter.Value)
                {
                    args.IncreaseIndent();
                }
                else
                {
                    args.DecreaseIndent();
                }
            }
        }

        private void WriteRawProp(
            Args args,
            string propName,
            string rawPropVal,
            bool startWithNewLine = true,
            bool? increaseIndentAfter = null,
            bool startWithComma = true) => Write(
                args,
                $"\"{propName}\": {rawPropVal}",
                startWithNewLine,
                increaseIndentAfter,
                startWithComma);

        private void WriteStrPropVal(
            Args args,
            string propName,
            string strPropVal,
            bool startWithNewLine = true,
            bool? increaseIndentAfter = null,
            bool startWithComma = true) => WriteRawProp(
                args,
                propName,
                EncodeStringForJson(strPropVal),
                startWithNewLine,
                increaseIndentAfter,
                startWithComma);

        private void WriteObjectIfNotNull(
            Args args,
            string propName,
            object @object,
            bool startWithComma = true)
        {
            if (@object != null)
            {
                string indentStr = args.IndentStr;
                string dataJson = JsonH.ToJson(@object, false);

                string[] jsonLines = dataJson.Split('\n').Select(
                        (line, idx) => idx == 0 ? line : string.Concat(
                            indentStr, line)).ToArray();

                dataJson = string.Join("\n", jsonLines);

                WriteRawProp(
                    args,
                    propName,
                    dataJson,
                    startWithComma);
            }
        }

        private string EncodeStringForJson(string str) => JsonConvert.SerializeObject(str);

        private JsonSerializerSettings GetJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            settings.Converters = settings.Converters ?? new List<JsonConverter>();
            settings.Converters.Add(stringEnumConverter);

            return settings;
        }

        private class Args
        {
            public Args(
                LogEvent logEvent,
                TextWriter output)
            {
                LogEvent = logEvent ?? throw new ArgumentNullException(nameof(logEvent));
                Output = output ?? throw new ArgumentNullException(nameof(output));
            }

            public LogEvent LogEvent { get; }
            public TextWriter Output { get; }

            public int Indent { get; private set; }
            public string IndentStr { get; private set; }

            public void IncreaseIndent()
            {
                Indent++;
                IndentStr += INDENT_STR;
           }

            public void DecreaseIndent()
            {
                Indent--;
                IndentStr = IndentStr.SubStr(0, -indentStrLen);
            }
        }
    }
}
