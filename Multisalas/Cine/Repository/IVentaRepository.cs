using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public interface IVentaRepository
    {
        Venta Create(Venta venta);
        Venta Read(long id);
        IList<Venta> List();
        Venta Update(Venta venta);
        Venta Delete(long id);
        int ButacasVendidas(long idSesion);
        IList<Venta> EntradasVendidasSala(int idSala);
        int EntradasVendidasTotalSala(int idSala);
        IList<Venta> EntradasVendidasSesion(int idSesion);
        int EntradasVendidasTotalSesion(int idSesion);

        void CambiarCerradoSesion(int idSesion);
        bool SesionValida(int sesionId);
        Sesion BuscaSesion(int sesionID);
    }
}
