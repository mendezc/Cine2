using System;
namespace Cine.Controller
{
    public interface ISesionController
    {
        void Abrir(long id);
        void Cerrar(long id);
        bool EstaAbierta(long id);
    }
}