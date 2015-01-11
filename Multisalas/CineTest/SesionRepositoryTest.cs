using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Cine;
using Cine.Repository;
using System.Data.Entity;
using System.Collections.Generic;

namespace CineTest
{
    [TestClass]
    public class SesionRepositoryTest
    {
        private SesionRepository sut;

        [TestInitialize]
        public void TestInicializa()
        {
            sut = new SesionRepository();
        }
        [TestCleanup]
        public void TestCleanUp()
        {
        }
        [TestMethod]
        public void TestRead()
        {
            for (int i = 0; i < Constantes.SESIONES.Length; i++)
            {
                Sesion sesion = sut.Read(Constantes.SESIONES[i]);
                Assert.IsNotNull(sesion);
                Assert.AreEqual(Constantes.SESIONES[i], sesion.SesionId);
                Assert.AreEqual(Constantes.SALAS[i % Constantes.SALAS.Length], sesion.SalaId);
            }
        }

        [TestMethod]
        public void TestReadNoExisteSesion()
        {
            Sesion sesionNoExiste = sut.Read(Constantes.SESIONNOEXISTE);
            Assert.IsNull(sesionNoExiste);
        }

        [TestMethod]
        public void TestUpdate()
        {
            Sesion sesion1 = sut.Update(Constantes.SESIONES[0], true);
            Assert.AreEqual(true, sesion1.EstaCerrada);
            sesion1 = sut.Update(Constantes.SESIONES[0], false);
            Assert.AreEqual(false, sesion1.EstaCerrada);
        }

        [TestMethod]
        [ExpectedException(typeof(SesionException))]
        public void TestUpdateNoExisteSesion()
        {
            Sesion sesion = sut.Update(Constantes.SESIONNOEXISTE, false);
        }

        [TestMethod]
        public void TestEsValidaYEstaAbierta()
        {
            sut.Update(1, false);
            bool valida = sut.SesionValidaYAbierta(1);
            Assert.IsTrue(valida);
        }

        [TestMethod]
        public void TestNoEsValida()
        {
            bool valida = sut.SesionValidaYAbierta(Constantes.SESIONNOEXISTE);
            Assert.IsFalse(valida);
        }

        [TestMethod]
        public void TestEsValidaYEstaCerrada()
        {
            sut.Update(1, true);
            bool valida = sut.SesionValidaYAbierta(1);
            Assert.IsFalse(valida);
        }
    }
}
