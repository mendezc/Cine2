﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public interface IVentaController
    {
        double Create(Venta venta);
        Venta Read(long id);
        IList<Venta> List();
        Venta Update(Venta venta);
        double Delete(long id);
        double Calcular();
        int TotalEntradas(Venta venta);
        double TotalDineroSala(int idSala);

        double TotalDineroSesion(int idSesion);

        void CambiarCerradoSesion(int idSesion);

        bool SesionValida(Sesion ses);

        Sesion BuscaSesion(int sesionID);
    }
}