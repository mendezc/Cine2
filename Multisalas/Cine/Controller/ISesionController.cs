using System;
namespace Cine.Controller
{
    public interface ISesionController
    {
        Cine.Sesion Abrir(long id);
        Cine.Sesion Cerrar(long id);
    }
}