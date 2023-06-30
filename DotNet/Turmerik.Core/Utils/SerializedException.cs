using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Turmerik.Collections;

namespace Turmerik.Utils
{
    public class SerializedException
    {
        public string Message { get; set; }
        public string TypeFullName { get; set; }
        public string[] StackTrace { get; set; }
        public string Source { get; set; }

        public SerializedException Inner { get; set; }
        public SerializedException[] Inners { get; set; }

        public object AdditionalData { get; set; }
    }
}
