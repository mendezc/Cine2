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
        public int NumeroEntradas { get; set; }
        public long SesionId { get; set; }
        public Sesion Sesion { get; set; }
        public double DescuentoAplicado { get; set; }
        public double Total { get; set; }
        public double Diferencia { get; set; }
        public bool Devolucion { get; set; }

        public Venta()
        {
            this.Devolucion = false;
            this.DescuentoAplicado = 0;
            this.Total = 0;
            this.Diferencia = 0;
        }
        public Venta(Sesion sesion, int numentradas)
        {
            this.NumeroEntradas = numentradas;
            this.DescuentoAplicado = 0;
            this.Total = 0;
            this.Diferencia = 0;
            this.Sesion = sesion;
            this.Devolucion = false;

        }
       
    }
}