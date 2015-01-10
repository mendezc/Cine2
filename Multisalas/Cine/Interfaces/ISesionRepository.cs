using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Repository
{
    public interface ISesionRepository
    {
        Sesion Read(long id);
        Sesion Update(long id, bool cerrada);
        bool SesionValidaYAbierta(long sesionId);
    }
}
