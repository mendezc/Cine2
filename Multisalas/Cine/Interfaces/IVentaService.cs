using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public interface IVentaService
    {
        Venta Create(Venta venta);
        Venta Read(long id);
        IList<Venta> List();
        Venta Update(Venta venta);
        Venta Delete(long id);
        bool QuedanEntradas(int sesionId, int numEntradas, int antiguasEntradas);
        Venta PrecioEntradas(Venta venta, Venta antiguaVenta);
        int ButacasVendidasSesion(long idSesion);
        int ButacasVendidasSala(int idSala);
        int ButacasVendidas();
        double TotalPrecioSesion(long idSesion);
        double TotalPrecioSala(int idSala);
        double TotalPrecio();
    }
}
