using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class Venta
    {
        public long VentaId { get; set; }
        public int numEntradas { get; set; }
        public long SesionId { get; set; }
        public Sesion Sesion { get; set; }
        public double DescuentoAplicado { get; set; }
        public double Total { get; set; }
        public double Diferencia { get; set; }

        public Venta()
        {
            this.Devolucion = false;
        }
        public Venta(Sesion sesion, int numentradas)
        {
            this.numEntradas = numentradas;
            this.Sesion = sesion;
            this.Devolucion = false;

        }
        public bool Devolucion { get; set; }
    }
}