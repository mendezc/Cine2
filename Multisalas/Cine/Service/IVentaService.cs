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
        double Calcular();
        bool QuedanEntradas(int sesionId, int numEntradas, int antiguasEntradas);
        double PrecioEntradas(int numeroEntradas);
        int TotalEntradas(Venta venta);
        double TotalDineroSala(int idSala);
        double TotalDineroSesion(int idSesion);
        int EntradasVendidasTotalSala(int idSala);
        int EntradasVendidasTotalSesion(int idSesion);
        bool SesionValida(Sesion ses);
        Sesion BuscaSesion(int sesionID);
    }
}
