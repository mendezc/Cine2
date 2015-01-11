using System;
namespace Cine.Service
{
    public interface ISesionService
    {
        Sesion Read(long id);
        Sesion Abrir(long id);
        Sesion Cerrar(long id);
        bool SesionValidaYAbierta(long sesionId);
    }
}
