using System;
namespace Cine.Service
{
    public interface ISesionService
    {
        Cine.Sesion Abrir(long id);
        Cine.Sesion Cerrar(long id);
    }
}
