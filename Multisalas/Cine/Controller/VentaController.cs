using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cine.Service;

namespace Cine.Controller
{
    public class VentaController : IVentaController
    {
        public IVentaService Servicio { get; set; }

        public VentaController(IVentaService ventaService)
        {
            Servicio = ventaService;
        }

        public Venta Create(Venta venta)
        {
            return Servicio.Create(venta);
        }

        public Venta Read(long id)
        {
            return Servicio.Read(id);
        }

        public IList<Venta> List()
        {
            return Servicio.List();
        }

        public Venta Update(Venta venta)
        {
            return Servicio.Update(venta);
        }

        public Venta Delete(long id)
        {
           return Servicio.Delete(id);
        }

        public int ButacasVendidasSesion(long idSesion)
        {
            return Servicio.ButacasVendidasSesion(idSesion);
        }

        public int ButacasVendidasSala(int idSala)
        {
            return Servicio.ButacasVendidasSala(idSala);
        }

        public int ButacasVendidas()
        {
            return Servicio.ButacasVendidas();
        }

        public double TotalPrecioSesion(long idSesion)
        {
            return Servicio.TotalPrecioSesion(idSesion);
        }

        public double TotalPrecioSala(int idSala)
        {
            return Servicio.TotalPrecioSala(idSala);
        }

        public double TotalPrecio()
        {
            return Servicio.TotalPrecio();
        }
    }
}
