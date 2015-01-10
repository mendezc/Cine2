using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine.Repository;
using Cine;

namespace CineTest
{
    [TestClass]
    public class RepositoryTest
    {
        private VentaRepository _repositorio;
        private Venta _venta;
        private Sesion _sesion;

        [TestInitialize]
        public void TestInitialize(){
            _repositorio = new VentaRepository();
            _sesion = new Sesion(1, 1, 19);
          //  _repositorio.CambiarCerradoSesion(_sesion.SesionId);
        //    _sesion = _repositorio.BuscaSesion(1);
        }

        [Ignore]
        [TestMethod]
        public void TestCreateList()
        {
            _venta = new Venta(_sesion, 100);
            _repositorio.Create(_venta);
            Sesion sesion2 = new Sesion(2, 2, 19);
           // _repositorio.CambiarCerradoSesion(sesion2.SesionId);
            _venta = new Venta(sesion2, 10);
            _repositorio.Create(_venta);

            Sesion sesion3 = new Sesion(3, 3, 19);
           // _repositorio.CambiarCerradoSesion(sesion3.SesionId);
            _venta = new Venta(sesion3, 10);
            _repositorio.Create(_venta);
            Assert.AreEqual(3, _repositorio.List().Count);
        }
        [Ignore]
        [TestMethod]
        public void TestReadCreate()
        {
            _venta = new Venta(_sesion, 10);
            _repositorio.Create(_venta);
            Assert.AreEqual(1, _repositorio.Read(1).Sesion.SalaId);
            Assert.AreEqual(1, _repositorio.Read(1).Sesion.SesionId);
            Assert.AreEqual(10, _repositorio.Read(1).numEntradas);
        }
        [Ignore]
        [TestMethod]
        public void TestDelete()
        {
            _venta = new Venta(_sesion, 50);
            _repositorio.Create(_venta);
            Sesion sesion2 = new Sesion(2, 2, 19);
           // _repositorio.CambiarCerradoSesion(sesion2.SesionId);
            _venta = new Venta(sesion2, 40);
            _repositorio.Create(_venta);
            _repositorio.Delete(1);
            Assert.AreEqual(1, _repositorio.List().Count);
        }
        [Ignore]
        [TestMethod]
        public void TestUpdate()
        {
            _venta = new Venta(_sesion, 50);
            _repositorio.Create(_venta);
            _venta = new Venta(_sesion, 0);
            _venta.VentaId = 1;
            _repositorio.Update(_venta);
            Assert.AreEqual(0, _repositorio.Read(1).numEntradas);
        }
    }
}
