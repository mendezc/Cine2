using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cine
{
    public class VentaException : Exception
    {
        public VentaException(string message)
            : base(message)
        {

        }
    }
}
