using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Cine;
using Cine.Service;
using Cine.Repository;
using Cine.Controller;
using Microsoft.Practices.Unity;
using System.Data.Entity;

namespace CineTest
{
    [TestClass]
    public class IntegracionTest
    {
        private Venta _venta;
        private Sesion _sesion;
        private IVentaController _controlador;
        private ISesionController _controladorSesion;
        private IUnityContainer container;

        [TestInitialize]
        public void TestInitialize()
        {

            container = new UnityContainer();

            container.RegisterType<IVentaRepository,VentaRepository>();
            container.RegisterType<ISesionRepository, SesionRepository>();
            container.RegisterType<IVentaService, VentaService>();
            container.RegisterType<ISesionService, SesionService>();
            container.RegisterType<IVentaController, VentaController>();
            container.RegisterType<ISesionController, SesionController>();

            SalasDB.Initializer = new SalasDBInitializerDropCreateDatabaseAlways();

            _controlador = container.Resolve<IVentaController>();
            _controladorSesion = container.Resolve<ISesionController>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            using (var context = new SalasDB())
            {
                context.Database.ExecuteSqlCommand("DELETE FROM Ventas");
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void AbriryCerrarSesiontest()
        {
            const long TESTSESSION = 1;
            bool abierta = _controladorSesion.EstaAbierta(TESTSESSION);
            Assert.IsFalse(abierta); // cerrada por defecto.

            _controladorSesion.Abrir(TESTSESSION);
            abierta = _controladorSesion.EstaAbierta(TESTSESSION);
            Assert.IsTrue(abierta);

            _controladorSesion.Cerrar(TESTSESSION);
            abierta = _controladorSesion.EstaAbierta(TESTSESSION);
            Assert.IsFalse(abierta);
        }

        [TestMethod]
        public void TestCreateVenta()
        {
            _sesion = new Sesion(1, 1, 17.5);
            _venta = new Venta(_sesion, 50);
            _controladorSesion.Abrir(1);

            _venta = _controlador.Create(_venta);
            Assert.AreEqual(315.0d, _venta.Total, 0.0001d);

            _sesion = new Sesion(1, 1, 17.5);
            _venta = new Venta(_sesion, 49);
            _venta = _controlador.Create(_venta);
            Assert.AreEqual(308.70d, _venta.Total, 0.0001d);
        }

        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestCreateNoPuedoSiNoAforoSuficiente()
        {
            _sesion = new Sesion(4, 1, 17.5);
            _venta = new Venta(_sesion, 100);
            _controladorSesion.Abrir(4);
            _controlador.Create(_venta);
            _sesion = new Sesion(4, 1, 17.5);
            _venta = new Venta(_sesion, 1);
            _controlador.Create(_venta);
        }
        [TestMethod]
        [ExpectedException(typeof(SesionException))]
        public void TestCreateNoPuedoSiSesionCerrada()
        {
            _sesion = new Sesion(2, 1, 17.5);
            _venta = new Venta(_sesion, 1);
            _controladorSesion.Cerrar(2);
            _controlador.Create(_venta);
        }
        [TestMethod]
        public void TestUpdateCambioVenta()
        {
            _sesion = new Sesion(7, 1, 17.5);
            _venta = new Venta(_sesion, 10);
            _controladorSesion.Abrir(7);
            Venta dbVenta = _controlador.Create(_venta);
            Sesion sesion2 = new Sesion(7, 1, 17.5);
            _venta = new Venta(sesion2, 20);
            _venta.VentaId = dbVenta.VentaId;
            _venta = _controlador.Update(_venta);
            Assert.AreNotEqual(_venta, dbVenta);
            Assert.AreEqual(_venta.VentaId, dbVenta.VentaId);
            Assert.AreEqual(63.0d, _venta.Diferencia);
        }

        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestUpdateNoPuedoSiVentaNoExiste()
        {
            _sesion = new Sesion(4, 1, 17.5);
            _venta = new Venta(_sesion, 50);
            Venta dbVenta = _controlador.Create(_venta);
            Sesion sesion2 = new Sesion(4, 2, 17);
            dbVenta.NumeroEntradas = 2;
            dbVenta.VentaId = 9999;
            _controlador.Update(dbVenta);
        }

        [TestMethod]
        [ExpectedException(typeof(SesionException))]
        public void TestUpdateNoPuedoSiSesionCerrada()
        {
            _sesion = new Sesion(6, 1, 17.5);
            _venta = new Venta(_sesion, 10);
            _controlador.Create(_venta);
            _controladorSesion.Cerrar(6);
            _venta.NumeroEntradas = 20;
            _controlador.Update(_venta);
        }

        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestUpdateNoPuedoSiNoHaySuficienteAforo()
        {
            _sesion = new Sesion(6, 1, 22);
            _controladorSesion.Abrir(6);
            _venta = new Venta(_sesion, 20);
            Venta venta = _controlador.Create(_venta);
            venta.NumeroEntradas = 21;
            _controlador.Update(_venta);
        }

        [TestMethod]
        public void TestDelete()
        {
            Sesion sesion1 = new Sesion(8, 1, 17.5);
            _controladorSesion.Abrir(sesion1.SesionId);
            _venta = new Venta(sesion1, 2);
            _controlador.Create(_venta);

            _venta = _controlador.Delete(_venta.VentaId);
            Assert.AreEqual(_venta.Devolucion, true);
        }

        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestDeleteNoPuedoSiVentaNoExiste()
        {
            _controlador.Delete(9999);
        }

        [TestMethod]
        [ExpectedException(typeof(SesionException))]
        public void TestDeleteNoPuedoSiSesionCerrada()
        {
            _sesion = new Sesion(8, 1, 17.5);
            _controladorSesion.Abrir(_sesion.SesionId);
            Venta _venta = new Venta(_sesion, 2);
            _venta = _controlador.Create(_venta);
            _controladorSesion.Cerrar(_sesion.SesionId);
            _controlador.Delete(_venta.VentaId);
        }

        [TestMethod]
        public void TestTotalizarEntradas()
        {

        }

        [TestMethod]
        public void TestTotalizarVentas()
        {

        }
   }
}
