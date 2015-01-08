using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Cine
{
    public class SalasDB: DbContext
    {
        public static IDatabaseInitializer<SalasDB> Initializer { get; set; }

        public SalasDB()
        {
            if (Initializer != null)
            { 
                Database.SetInitializer<SalasDB>(Initializer);
            }
        }


        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Sesion> Sesiones { get; set; }
    }
}
