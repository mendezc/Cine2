using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Controller
{
    public class VentaController : IVentaController
    {
        public IVentaService Servicio
        {
            get;
            set;
        }

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

        public double Calcular()
        {
            return Servicio.Calcular();
        }

        public int TotalEntradas(Venta venta)
        {
            return Servicio.TotalEntradas(venta);
        }
        public double TotalDineroSala(int idSala)
        {
            return Servicio.TotalDineroSala(idSala);
        }
        public double TotalDineroSesion(int idSesion)
        {
            return Servicio.TotalDineroSesion(idSesion);
        }

        public bool SesionValida(Sesion ses)
        {
            return Servicio.SesionValida(ses);
        }

        public Sesion BuscaSesion(int sesionID)
        {
            return Servicio.BuscaSesion(sesionID);
        }
    }
}
