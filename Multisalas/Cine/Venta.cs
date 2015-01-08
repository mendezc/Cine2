using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class Venta
    {
        public Venta()
        {

        }

        public Venta(Sesion sesion, int numentradas)
        {
            this.numEntradas = numentradas;
            this.Sesion = sesion;
        }

        public long VentaId
        {
            get;
            set;
        }

        public int numEntradas
        {
            get;
            set;
        }

        //public int SesionId { get; set; }

        public Sesion Sesion
        {
            get;
            set;
        }

        public double Precio { get; set; }
    }
}