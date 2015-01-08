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
        //private SalasDB context;
        //private DbContextTransaction transaction;

        private Venta _venta;
        private Venta _ventaDos;
        private Sesion _sesion;
        private IVentaController _controlador;
        private ISesionController _controladorSesion;

        private IUnityContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            //context = new SalasDB();
            //transaction = context.Database.BeginTransaction();

            container = new UnityContainer();

            container.RegisterType(typeof(IVentaRepository), typeof(VentaRepository));
            container.RegisterType(typeof(ISesionRepository), typeof(SesionRepository));
            container.RegisterType(typeof(IVentaService), typeof(VentaService));
            container.RegisterType(typeof(ISesionService), typeof(SesionService));
            container.RegisterType(typeof(IVentaController), typeof(VentaController));
            container.RegisterType(typeof(ISesionController), typeof(SesionController));

            SalasDB.Initializer = new SalasDBInitializerDropCreateDatabaseAlways();

            _controlador = container.Resolve<IVentaController>();
            _controladorSesion = container.Resolve<ISesionController>();
        }

        //[TestCleanup]
        //public void TestCleanUp()
        //{
        //    transaction.Rollback();
        //    transaction.Dispose();
        //    context.Dispose();
        //}
        [TestMethod]
        public void AbrirSesiontest()
        {
            Sesion sesion1 = new Sesion(1, 1, 17.5);
            Assert.AreEqual(sesion1.Cerrado, true);
            sesion1 = _controladorSesion.Abrir(sesion1.SesionId);
            Assert.AreEqual(sesion1.Cerrado, false);
        }

        [TestMethod]
        public void AbriryCerrarSesiontest()
        {
            Sesion sesion1 = new Sesion(1, 1, 17.5);
            sesion1 = _controladorSesion.Abrir(sesion1.SesionId);
            Assert.AreEqual(sesion1.Cerrado, false);

            sesion1 = _controladorSesion.Cerrar(sesion1.SesionId);
            Assert.AreEqual(sesion1.Cerrado, true);
        }
        [TestMethod]
        public void TestCreateVenta()
        {
            _sesion = new Sesion(1, 1, 17.5);
            _venta = new Venta(_sesion, 50);
            _controladorSesion.Abrir(1);

            _venta = _controlador.Create(_venta);
            Assert.AreEqual(315.0d, _venta.Precio, 0.0001d);

            _sesion = new Sesion(1, 1, 17.5);
            _venta = new Venta(_sesion, 49);
            _venta = _controlador.Create(_venta);
            Assert.AreEqual(308.70d, _venta.Precio, 0.0001d);

        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestNoPuedoCreateSiNoAforoSuficiente()
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
        [ExpectedException(typeof(VentaException))]
        public void TestNoPuedoCreateSiSesionCerrada()
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
            _controlador.Update(_venta);
            Assert.AreNotEqual(_venta, dbVenta);
            Assert.AreEqual(_venta.VentaId, dbVenta.VentaId);
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestNoPuedoUpdateSiVentaNoExiste()
        {
            _sesion = new Sesion(4, 1, 17.5);
            _venta = new Venta(_sesion, 50);
            Venta dbVenta = _controlador.Create(_venta);
            Sesion sesion2 = new Sesion(4, 2, 17);
            dbVenta.numEntradas = 2;
            dbVenta.VentaId = 9999;
            _controlador.Update(dbVenta);
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestNoPuedoUpdateSiSesionCerrada()
        {
            _sesion = new Sesion(4, 1, 17.5);
            _venta = new Venta(_sesion, 10);
            _controlador.Create(_venta);
         //   _controlador.CambiarCerradoSesion(_sesion.SesionId);
            _venta.numEntradas = 20;
            _controlador.Update(_venta);
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestNoPuedoUpdateSiNoHaySuficienteAforo()
        {
            _sesion = new Sesion(3, 1, 22);
            _venta = new Venta(_sesion, 20);
            _controlador.Create(_venta);
            _venta.numEntradas = 21;
            _controlador.Update(_venta);
        }
        [Ignore]
        [TestMethod]
        public void TestCreateList()
        {
            _venta = new Venta(_sesion, 80);
            _controlador.Create(_venta);
            _venta = new Venta(_sesion, 10);
            _controlador.Create(_venta);

            Sesion sesion2 = new Sesion(2, 2, 19);
          //  _controlador.CambiarCerradoSesion(sesion2.SesionId);

            _ventaDos = new Venta(sesion2, 10);
            _controlador.Create(_ventaDos);
            Assert.AreEqual(3, _controlador.List().Count);
        }
        [Ignore]
        [TestMethod]
        public void TestCalcular()
        {
            _venta = new Venta(_sesion, 80);
            _controlador.Create(_venta);
            Sesion sesion2 = new Sesion(2, 2, 19);
         //   _controlador.CambiarCerradoSesion(sesion2.SesionId);
            _venta = new Venta(sesion2, 10);
            _controlador.Create(_venta);

            Assert.AreEqual( 567 , _controlador.Calcular() );

        }
        [Ignore]
        [TestMethod]
        public void TestReadCreate()
        {
            _venta = new Venta(_sesion, 10);
            _controlador.Create(_venta);
            Assert.AreEqual(1, _controlador.Read(1).VentaId);
            Assert.AreEqual(1, _controlador.Read(1).Sesion.SesionId);
            Assert.AreEqual(10, _controlador.Read(1).numEntradas);
        }
        [Ignore]
        [TestMethod]
        public void TestDelete()
        {
            _venta = new Venta(_sesion, 50);
            _controlador.Create(_venta);
            Sesion sesion2 = new Sesion(2, 2, 17);
           // _controlador.CambiarCerradoSesion(sesion2.SesionId);
            _venta = new Venta(sesion2, 40);
            _controlador.Create(_venta);


            Assert.AreEqual(315,_controlador.Delete(1));
            Assert.AreEqual(1, _controlador.List().Count);
            Assert.AreEqual(252, _controlador.Calcular());
        }
        [Ignore]
        [TestMethod]
        public void TestUpdate()
        {
            _venta = new Venta(_sesion, 50);
            _controlador.Create(_venta);

            _venta = new Venta(_sesion,0);
            _venta.VentaId = 1;
            _controlador.Update(_venta);
            Assert.AreEqual(0, _controlador.Read(1).numEntradas);
            Assert.AreEqual(0, _controlador.Calcular());
        }
        [Ignore]
        [TestMethod]
        public void ComprobarLasEntradas()
        {
            Cine.Venta entradaPrueba = new Cine.Venta(_sesion,50);                 
            _controlador.Create(entradaPrueba);
            _controlador.TotalEntradas(entradaPrueba);
            Assert.AreEqual(50, _controlador.TotalEntradas(entradaPrueba));
        }
        [Ignore]
        [TestMethod]
        public void ComprobarLasEntradasSala()
        {
            Cine.Venta entradaPrueba = new Cine.Venta(_sesion,50);
            _controlador.Create(entradaPrueba);
            _controlador.TotalEntradas(entradaPrueba);
            //_controlador.Servicio.EntradasVendidasTotalSala(entradaPrueba.Sesion.SalaId);
            Assert.AreEqual(50, _controlador.TotalEntradas(entradaPrueba));
            //Assert.AreEqual(50, _controlador.Servicio.EntradasVendidasTotalSala(entradaPrueba.Sesion.SalaId));
        }
        [Ignore]
        [TestMethod]
        public void ComprobarLasEntradasSesion()
        {
            Cine.Venta entradaPrueba = new Cine.Venta(_sesion, 50);
            _controlador.Create(entradaPrueba);
            _controlador.TotalEntradas(entradaPrueba);
            //_controlador.Servicio.EntradasVendidasTotalSesion(entradaPrueba.Sesion.SesionId);
            Assert.AreEqual(50, _controlador.TotalEntradas(entradaPrueba));
            //Assert.AreEqual(50, _controlador.Servicio.EntradasVendidasTotalSesion(entradaPrueba.Sesion.SesionId));
        }
        [Ignore]
        [TestMethod]
        public void ComprobarDineroSala()
        {
            Cine.Venta entradaPrueba = new Cine.Venta(_sesion, 50);
            _controlador.Create(entradaPrueba);            
            _controlador.TotalDineroSala(1);

            Assert.AreEqual(315, _controlador.TotalDineroSala(1));
        }

        

        [Ignore]
        [TestMethod]
        public void TestUpdateBySalaAndSesion()
        {
            _venta = new Venta(_sesion, 10);
            _controlador.Create(_venta);

            Sesion _sesion2 = new Sesion(2, 2, 17);
           // _controlador.CambiarCerradoSesion(_sesion2.SesionId);
            _sesion2 = _controlador.BuscaSesion(_sesion2.SesionId);

            _venta = new Venta(_sesion2, 15);
            _venta.VentaId = 1;
            _venta = _controlador.Update(_venta);

            Assert.AreEqual(15, _controlador.Read(1).numEntradas);
            Assert.AreEqual(_sesion2, _venta.Sesion);
            
        }
    }
}
