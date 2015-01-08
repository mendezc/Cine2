using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class Sesion
    {
        public Sesion()
        { }

        public Sesion(int id, int idSala, double hora)
        {
            this.SesionId = id;
            this.SalaId = idSala;
            this.Hora = hora;
            Cerrado = true;
        }

        public Sesion(int id, int idSala, double hora, Boolean cerrado)
        {
            this.SesionId = id;
            this.SalaId = idSala;
            this.Hora = hora;
            this.Cerrado = cerrado;
        }

        public int SesionId //idSesion
        {
            get;
            set;
        }

        public int SalaId
        {
            get;
            set;
        }

        public bool Cerrado
        {
            get;
            set;
        }

        public double Hora
        {
            get;
            set;
        }
    }    
}