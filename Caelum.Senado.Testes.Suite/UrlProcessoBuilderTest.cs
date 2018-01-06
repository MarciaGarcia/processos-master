using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Caelum.Senado.Modelo;
using Caelum.Senado.Service;

namespace Caelum.Senado.Testes.Suite
{
    [TestClass]
    public class UrlProcessoBuilderTest
    {
        private const string URL_BASICA = "http://legis.senado.leg.br/dadosabertos/materia/";

        [TestMethod]
        public void TestUrlComTodosCamposPreenchidos()
        {
            var urlEsperada = URL_BASICA + "pls/85/2017";
            var campos = new Processo { Sigla = "pls", Numero = 85, Ano = 2017 };
            var urlBuilder = new UrlProcessoBuilder(campos);
            Assert.AreEqual(urlEsperada, urlBuilder.Build());
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void TestUrlComSiglaNula()
        {
            var urlBuilder = new UrlProcessoBuilder(
                new Processo { Numero = 12, Ano = 1238 });
            Assert.Fail("Não deveria ter chegado aqui!");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void TestUrlComNumeroInvalido()
        {
            var urlBuilder = new UrlProcessoBuilder(
                new Processo { Sigla = "teste", Ano = 1238 });
            Assert.Fail("Não deveria ter chegado aqui!");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void TestUrlComAnoInvalido()
        {
            var urlBuilder = new UrlProcessoBuilder(
                new Processo { Sigla = "tesr", Numero = 12, Ano = -1238 });
            Assert.Fail("Não deveria ter chegado aqui!");
        }


    }
}
