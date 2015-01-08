using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using Cine.Repository;
using Cine.Service;
using Moq;
using System.Collections.Generic;

namespace CineTest
{
    [TestClass]
    public class ServiceTest
    {
        private Venta _venta;
        private VentaService _servicio;
        private Mock<IVentaRepository> mock;
        private Sesion _sesion;
        [TestInitialize]
        public void TestInitialize()
        {
            //_servicio = new VentaService();
            List<Venta> listaPrueba = new List<Venta>();
            _sesion = new Sesion(1, 1, 19);
            _venta = new Venta(_sesion,10);
            _venta.VentaId = 1;
            listaPrueba.Add(_venta);

            mock = new Mock<IVentaRepository>();
            mock.Setup(repo => repo.List()).Returns(listaPrueba);
            mock.Setup(repo => repo.Create(_venta)).Returns(_venta);
            mock.Setup(repo => repo.Read(1)).Returns(_venta);
            mock.Setup(repo => repo.ButacasVendidas(1)).Returns(10);
            mock.Setup(repo => repo.SesionValida(_sesion.SesionId)).Returns(true);

            _servicio.Repositorio = mock.Object;
        }

        [TestMethod]
        public void TestCreateList()
        {
            _servicio.Create(_venta);
            Assert.AreEqual(1, _servicio.List().Count);
            mock.Verify(repo => repo.List(),Times.Exactly(1));
            mock.Verify(repo => repo.Create(_venta), Times.Exactly(1));
            mock.Verify(repo => repo.ButacasVendidas(1), Times.Exactly(1));
        }

        [TestMethod]
        public void TestCalcular()
        {
            _servicio.Create(_venta);
            Assert.AreEqual(63, _servicio.Calcular());
            mock.Verify(repo => repo.List(), Times.Exactly(1));
        }

        [TestMethod]
        public void TestReadCreate()
        {
            _servicio.Create(_venta);
            Assert.AreEqual(1, _servicio.Read(1).Sesion.SalaId);
            Assert.AreEqual(1, _servicio.Read(1).Sesion.SesionId);
            Assert.AreEqual(10, _servicio.Read(1).numEntradas);
        }

        [TestMethod]
        public void TestDelete()
        {
            Venta _venta2 = new Venta(_sesion, 40);
            mock.Setup(repo => repo.Create(_venta2)).Returns(_venta2);
            mock.Setup(repo => repo.Delete(2)).Returns(_venta);
            _servicio.Create(_venta2);
            _servicio.Delete(2);
            Assert.AreEqual(1, _servicio.List().Count);
            Assert.AreEqual(63, _servicio.Calcular());
        }

        [TestMethod]
        public void TestUpdate()
        {
            _servicio.Create(_venta);
            Venta _venta2 = new Venta(_sesion, 0);
            _venta2.VentaId = 1;
            mock.Setup(repo => repo.Update(_venta2)).Returns(_venta2);
            Assert.AreEqual(0, _servicio.Update(_venta2).numEntradas);
        }
    }
}
