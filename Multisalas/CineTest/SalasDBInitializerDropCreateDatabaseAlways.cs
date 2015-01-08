﻿using Cine;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTest
{
    public class SalasDBInitializerDropCreateDatabaseAlways: DropCreateDatabaseAlways<SalasDB>
    {
        protected override void Seed(SalasDB context)
        {

            context.Sesiones.Add(new Sesion(1,1,17.5, false));
            context.Sesiones.Add(new Sesion(2,2,17, false));
            context.Sesiones.Add(new Sesion(3,3,17, false));
            context.Sesiones.Add(new Sesion(4,1,19.5, false));
            context.Sesiones.Add(new Sesion(5,2,19, false));
            context.Sesiones.Add(new Sesion(6,3,19.5, false));
            context.Sesiones.Add(new Sesion(7,1,22, false));
            context.Sesiones.Add(new Sesion(8,2,22, false));
            context.Sesiones.Add(new Sesion(9,3,22.5, false));

            base.Seed(context);
        }
    }
}
