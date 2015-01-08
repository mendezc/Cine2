using Cine.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Service
{
    public class VentaService : IVentaService
    {
        private int[] BUTACAS = new int[3] { 100, 50, 20 };
        private const double PRECIO = 7d;
        private const double DESCUENTO = 0.9d;
        private const int UMBRAL_DESCUENTO = 5;
        public IVentaRepository Repositorio
        {
            get;
            set;
        }

        public ISesionRepository SesionRepositorio
        {
            get;
            set;
        }

        public VentaService(IVentaRepository ventaRepository, ISesionRepository sesionRepositorio)
        {
            this.Repositorio = ventaRepository;
            this.SesionRepositorio = sesionRepositorio;
        }


        public double Create(Venta venta)
        {
            if (Repositorio.SesionValida(venta.Sesion.SesionId) && QuedanEntradas(venta.Sesion.SesionId, venta.numEntradas))
            {
                double precio = PrecioEntradas(venta.numEntradas);
                venta.Precio = precio;
                Repositorio.Create(venta);
                return precio;
            }
            else
            {
                Trace.WriteLine("Imposible vender entradas, no quedan entradas o la sesion no existe o esta cerrada");
                throw new Exception("No quedan entradas en la sala o la sesion no existe o esta cerrada");
            }
        }

        public Venta Read(long id)
        {
            return Repositorio.Read(id);
        }

        public IList<Venta> List()
        {
            return Repositorio.List();
        }

        public Venta Update(Venta venta)
        {
            return Repositorio.Update(venta);
        }

        public double Delete(long id)
        {
            var venta = Repositorio.Delete(id);
            double precio = PrecioEntradas(venta.numEntradas);
            Trace.WriteLine("El dinero correspondiente a la venta es: " + precio);
            return precio;
        }

        public double Calcular()
        {
            return Repositorio.List().Sum(v => PrecioEntradas(v.numEntradas));
        }

        public bool QuedanEntradas(int sesionId, int numEntradas)
        {
            Sesion sesion = SesionRepositorio.Read(sesionId);
            int numeroEntradasConLasSolicitadas = Repositorio.ButacasVendidas(sesionId) + numEntradas;
            bool resultado = numeroEntradasConLasSolicitadas <= NumeroEntradas(sesion.SalaId);
            return resultado; 
        }

        private int NumeroEntradas(int salaId)
        {
            return BUTACAS[(salaId - 1)];
        }

        public double PrecioEntradas(int numeroEntradas)
        {
            var precio = (numeroEntradas * PRECIO);
            if (numeroEntradas >= UMBRAL_DESCUENTO)
            {
                Trace.WriteLine("Descuento del 10%");
                precio *= DESCUENTO;
            }
            return precio;
        }

        public int TotalEntradas(Venta venta)
        {
            return Repositorio.List().Sum(total => total.numEntradas);
        }

        public double TotalDineroSala(int idSala)
        {
            var ventaSala = Repositorio.EntradasVendidasSala(idSala);
            return ventaSala.Sum(v => PrecioEntradas(v.numEntradas));
        }

        public int EntradasVendidasTotalSala(int idSala)
        {
            return Repositorio.EntradasVendidasTotalSala(idSala);
        }

        public double TotalDineroSesion(int idSesion)
        {
            var ventaSala = Repositorio.EntradasVendidasSala(idSesion);
            return ventaSala.Sum(v => PrecioEntradas(v.numEntradas));
        }

        public int EntradasVendidasTotalSesion(int idSesion)
        {
            return Repositorio.EntradasVendidasTotalSesion(idSesion);
        }

        public void CambiarCerradoSesion(int idSesion)
        {
            Repositorio.CambiarCerradoSesion(idSesion);
        }

        public bool SesionValida(Sesion ses)
        {
            return Repositorio.SesionValida(ses.SesionId);
        }

        public Sesion BuscaSesion(int sesionID)
        {
            return Repositorio.BuscaSesion(sesionID);
        }
    }
}