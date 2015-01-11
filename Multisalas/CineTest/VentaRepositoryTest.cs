using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using Cine.Repository;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading;

namespace CineTest
{
    [TestClass]
    public class VentaRepositoryTest
    {
        private VentaRepository sut;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void TestInicializa()
        {
            sut = new VentaRepository();
        }
        [TestCleanup]
        public void TestCleanUp()
        {
            using (var context = new SalasDB())
            {
                context.Database.SqlQuery<Venta>("DELETE from Ventas");
                context.SaveChanges();
                context.Dispose();
            }
        }

        [TestMethod]
        public void TestCreate()
        {
            int ventas = sut.List().Count;
            Venta res = sut.Create(new Venta { Sesion = new Sesion(1, 1, 20.0d), NumeroEntradas = 10 });
            Assert.AreEqual(ventas + 1, sut.List().Count);
        }

        [TestMethod]
        public void TestRead()
        {
            Venta cr = sut.Create(new Venta { Sesion = new Sesion(1, 1, 20.0d), SesionId = 1, NumeroEntradas = 10 });
            Venta res = sut.Read(cr.VentaId);
            Assert.AreEqual(cr.VentaId, res.VentaId);
            Assert.AreEqual(10, res.NumeroEntradas);
        }

        [TestMethod]
        public void TestReadNoExisteVenta()
        {
            Venta res = sut.Read(9999);
            Assert.IsNull(res);
        }

        [TestMethod]
        public void TestList()
        {
            sut.Create(new Venta { Sesion = new Sesion(1, 1, 20.0d), SesionId = 1, NumeroEntradas = 20 });
            sut.Create(new Venta { Sesion = new Sesion(1, 1, 20.0d), SesionId = 1, NumeroEntradas = 20 });
            IList<Venta> res = sut.List();
            Assert.IsTrue(res.Count >= 2);
        }

        [TestMethod]
        public void TestUpdate()
        {
            Venta noVinculado = new Venta { SesionId = 1, NumeroEntradas = 20 };
            Venta cr = sut.Create(new Venta { Sesion=new Sesion(1,1,20.0d), SesionId = 1, NumeroEntradas = 20 });
            Venta ventaAActualizar = new Venta { Sesion = new Sesion(1, 1, 20.0d), SesionId = 1, NumeroEntradas = 10 };
            ventaAActualizar.VentaId = cr.VentaId;
            Venta actualizada = sut.Update(ventaAActualizar);
            Assert.AreEqual(10, actualizada.NumeroEntradas);
            Assert.AreNotEqual(noVinculado.NumeroEntradas, actualizada.NumeroEntradas);
        }

        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestUpdateNoExisteVenta()
        {
            Venta ventaAActualizar = new Venta { Sesion=new Sesion(1,1,20.0d), SesionId = 1, NumeroEntradas = 10 };
            ventaAActualizar.VentaId = 999;
            Venta actualizada = sut.Update(ventaAActualizar);
        }

        [TestMethod]
        public void TestDelete()
        {
            Venta cr = sut.Create(new Venta { Sesion = new Sesion(1, 1, 20.0d), SesionId = 1, NumeroEntradas = 20 });
            Venta ventaBorrada = sut.Delete(cr.VentaId);
            cr = sut.Read(cr.VentaId);
            Assert.IsNull(cr);
        }

        [TestMethod]
        public void TestDeleteNoExisteVenta()
        {
            Venta venta = sut.Delete(999);
            Assert.IsNull(venta);
        }
    }
}
