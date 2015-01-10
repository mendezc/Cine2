using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cine
{
    public class SesionException : Exception
    {
        public SesionException()
            : base("No existe la sesión especificada o está cerrada.")
        {

        }
    }
}
