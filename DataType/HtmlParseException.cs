﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataType
{
    [Serializable]
    public class HtmlParseException : Exception
    {
        public HtmlParseException() { }
        public HtmlParseException(string message) : base(message) { }
        public HtmlParseException(string message, Exception inner) : base(message, inner){ }
        protected HtmlParseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
