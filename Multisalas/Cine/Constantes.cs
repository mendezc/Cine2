using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class Constantes
    {
        public const double PRECIO = 7d;
        public const double DESCUENTO = 0.9d;
        public const int UMBRAL_DESCUENTO = 5;
        public static int[] BUTACAS = new int[3] { 100, 50, 20 };
        public static long[] SESIONES = new long[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public static long[] SALAS = new long[3] { 1, 2, 3 };

        public const long SESIONNOEXISTE = 10;
        public const long SALANOEXISTE = 4;
    }
}
