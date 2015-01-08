using Cine.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Controller
{
    public class SesionController : ISesionController
    {
        private ISesionService SesionServicio;

        public SesionController(ISesionService sesionService)
        {
            SesionServicio = sesionService;
        }

        public Sesion Cerrar(long id)
        {
            return SesionServicio.Cerrar(id);
        }

        public Sesion Abrir(long id)
        {
            return SesionServicio.Abrir(id);
        }
    }
}