using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cine.Repository;

namespace Cine.Service
{
    public class VentaService : IVentaService
    {
       
        public IVentaRepository Repositorio { get; set; }
        public ISesionRepository SesionRepositorio { get; set; }

        public VentaService(IVentaRepository ventaRepository, ISesionRepository sesionRepositorio)
        {
            this.Repositorio = ventaRepository;
            this.SesionRepositorio = sesionRepositorio;
        }


        public Venta Create(Venta venta)
        {
            if (!SesionRepositorio.SesionValidaYAbierta(venta.Sesion.SesionId))
            {
                Trace.WriteLine("Imposible vender entradas, la sesión no existe o está cerrada.");
                throw new SesionException();
            }
            if(!QuedanEntradas(venta.Sesion.SesionId, venta.numEntradas))
            {
                Trace.WriteLine("Imposible vender entradas, no quedan entradas");
                throw new VentaException("No quedan entradas en la sala");
            }
            venta = PrecioEntradas(venta);
            Repositorio.Create(venta);
            return venta;
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
            if (!SesionRepositorio.SesionValidaYAbierta(venta.Sesion.SesionId))
            {
                throw new SesionException();
            }
            Venta antigua = Repositorio.Read(venta.VentaId);
            bool haySuficientesEntradas = false;
            if (antigua == null)
            {
                throw new VentaException("La venta que intentó actualizar no existe.");
            }
            if (antigua.Sesion.SesionId == venta.Sesion.SesionId)
            {
                haySuficientesEntradas = QuedanEntradas(venta.Sesion.SesionId, venta.numEntradas, antigua.numEntradas);
            }
            else
            {
                haySuficientesEntradas = QuedanEntradas(venta.Sesion.SesionId, venta.numEntradas);
            }
            if (!haySuficientesEntradas)
            {
                Trace.WriteLine("Imposible cambiar entradas, no hay suficiente aforo para la sesión seleccionada.");
                throw new VentaException("No hay suficiente aforo para realizar el cambio en la venta");
            }
            venta = PrecioEntradas(venta, antigua);
            Trace.WriteLine("El dinero correspondiente a la venta es: " + venta.Total);
            return Repositorio.Update(venta);
        }

        public Venta Delete(long id)
        {
            Venta venta = Repositorio.Read(id);
            if (venta == null)
            {
                Trace.WriteLine("Imposible borrar una venta inexistente.");
                throw new VentaException("La venta que intentó borrar no existe.");
            }
            if (!SesionRepositorio.SesionValidaYAbierta(venta.Sesion.SesionId))
            {
                Trace.WriteLine("Imposible borrar la venta porque la sesión correspondiente está cerrada.");
                throw new SesionException();
            }
            venta = Repositorio.Delete(id);
            double precio = PrecioEntradas(venta).Total;
            Trace.WriteLine("El dinero correspondiente a la venta es: " + precio);
            return venta;
        }

        public bool QuedanEntradas(int sesionId, int numEntradas, int antiguasEntradas = 0)
        {
            Sesion sesion = SesionRepositorio.Read(sesionId);
            int numeroEntradasConLasSolicitadas = Repositorio.ButacasVendidasSesion(sesionId) - antiguasEntradas;
            numeroEntradasConLasSolicitadas += numEntradas;
            bool resultado = numeroEntradasConLasSolicitadas <= NumeroEntradas(sesion.SalaId);
            return resultado;
        }

        private int NumeroEntradas(int salaId)
        {
            return Constantes.BUTACAS[(salaId - 1)];
        }

        public Venta PrecioEntradas(Venta venta, Venta ventaAnterior = null)
        {
            venta.Total = (venta.numEntradas * Constantes.PRECIO);
            if (venta.numEntradas >= Constantes.UMBRAL_DESCUENTO)
            {
                Trace.WriteLine("Descuento del 10%");
                venta.Total *= Constantes.DESCUENTO;
            }
            if (ventaAnterior != null)
            {
                venta.Diferencia = venta.Total - ventaAnterior.Total;
            }
            return venta;
        }

        public int ButacasVendidasSesion(long idSesion)
        {
            return Repositorio.ButacasVendidasSesion(idSesion);
        }

        public int ButacasVendidasSala(int idSala)
        {
            return Repositorio.ButacasVendidasSala(idSala);
        }

        public int ButacasVendidas()
        {
            return Repositorio.ButacasVendidas();
        }

        public double TotalPrecioSesion(long idSesion)
        {
            return Repositorio.TotalPrecioSesion(idSesion);
        }

        public double TotalPrecioSala(int idSala)
        {
            return Repositorio.TotalPrecioSala(idSala);
        }

        public double TotalPrecio()
        {
            return Repositorio.TotalPrecio();
        }
    }
}