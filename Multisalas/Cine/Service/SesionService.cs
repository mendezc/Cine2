using Cine.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Service
{
    public class SesionService : ISesionService
    {

        public ISesionRepository SesionRepositorio { get; set; }

        public SesionService(ISesionRepository sesionRepository)
        {
            SesionRepositorio = sesionRepository;
        }

        public Sesion Read(long id)
        {
            Sesion sesion = SesionRepositorio.Read(id);
            if (sesion == null)
            {
                Trace.WriteLine("");
                throw new SesionException();
            }
            return sesion;
        }
        public Sesion Cerrar(long id)
        {
            Sesion sesion = SesionRepositorio.Update(id, true);
            return sesion;
        }

        public Sesion Abrir(long id)
        {
            Sesion sesion = SesionRepositorio.Update(id, false);
            return sesion;
        }
        public bool SesionValidaYAbierta(long id)
        {
            return SesionRepositorio.SesionValidaYAbierta(id);
        }
    }
}

