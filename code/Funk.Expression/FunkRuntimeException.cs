﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funk.Expression
{
    internal class FunkRuntimeException : Exception
    {
        public FunkRuntimeException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}
