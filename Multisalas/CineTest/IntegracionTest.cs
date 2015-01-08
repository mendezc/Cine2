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
        public void TestCreate()
        {
            _sesion = new Sesion(1, 1, 17.5);
            _venta = new Venta(_sesion, 50);

            _controlador.Create(_venta);

            _sesion = new Sesion(1, 1, 17.5);
            _venta = new Venta(_sesion, 49);
            
            _controlador.Create(_venta);


            //_sesion = new Sesion(1, 1, 17.5);
            //_venta = new Venta(_sesion, 2);

            //_controlador.Create(_venta);
            //Assert.AreEqual(0, _controlador.Calcular());
        }





        [TestMethod]
        public void TestCreateList()
        {
            _venta = new Venta(_sesion, 80);
            _controlador.Create(_venta);
            _venta = new Venta(_sesion, 10);
            _controlador.Create(_venta);

            Sesion sesion2 = new Sesion(2, 2, 19);
            _controlador.CambiarCerradoSesion(sesion2.SesionId);

            _ventaDos = new Venta(sesion2, 10);
            _controlador.Create(_ventaDos);
            Assert.AreEqual(3, _controlador.List().Count);
        }

        [TestMethod]
        public void TestCalcular()
        {
            _venta = new Venta(_sesion, 80);
            _controlador.Create(_venta);
            Sesion sesion2 = new Sesion(2, 2, 19);
            _controlador.CambiarCerradoSesion(sesion2.SesionId);
            _venta = new Venta(sesion2, 10);
            _controlador.Create(_venta);

            Assert.AreEqual( 567 , _controlador.Calcular() );

        }

        [TestMethod]
        public void TestReadCreate()
        {
            _venta = new Venta(_sesion, 10);
            _controlador.Create(_venta);
            Assert.AreEqual(1, _controlador.Read(1).VentaId);
            Assert.AreEqual(1, _controlador.Read(1).Sesion.SesionId);
            Assert.AreEqual(10, _controlador.Read(1).numEntradas);
        }
        
        [TestMethod]
        public void TestDelete()
        {
            _venta = new Venta(_sesion, 50);
            _controlador.Create(_venta);
            Sesion sesion2 = new Sesion(2, 2, 17);
            _controlador.CambiarCerradoSesion(sesion2.SesionId);
            _venta = new Venta(sesion2, 40);
            _controlador.Create(_venta);


            Assert.AreEqual(315,_controlador.Delete(1));
            Assert.AreEqual(1, _controlador.List().Count);
            Assert.AreEqual(252, _controlador.Calcular());
        }

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
        [TestMethod]
        public void ComprobarLasEntradas()
        {
            Cine.Venta entradaPrueba = new Cine.Venta(_sesion,50);                 
            _controlador.Create(entradaPrueba);
            _controlador.TotalEntradas(entradaPrueba);
            Assert.AreEqual(50, _controlador.TotalEntradas(entradaPrueba));
        }
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
        [TestMethod]
        public void ComprobarDineroSala()
        {
            Cine.Venta entradaPrueba = new Cine.Venta(_sesion, 50);
            _controlador.Create(entradaPrueba);            
            _controlador.TotalDineroSala(1);

            Assert.AreEqual(315, _controlador.TotalDineroSala(1));
        }

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
        public void TestUpdateBySalaAndSesion()
        {
            _venta = new Venta(_sesion, 10);
            _controlador.Create(_venta);

            Sesion _sesion2 = new Sesion(2, 2, 17);
            _controlador.CambiarCerradoSesion(_sesion2.SesionId);
            _sesion2 = _controlador.BuscaSesion(_sesion2.SesionId);

            _venta = new Venta(_sesion2, 15);
            _venta.VentaId = 1;
            _venta = _controlador.Update(_venta);

            Assert.AreEqual(15, _controlador.Read(1).numEntradas);
            Assert.AreEqual(_sesion2, _venta.Sesion);
            
        }
    }
}
