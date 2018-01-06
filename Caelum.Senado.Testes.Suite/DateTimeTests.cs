using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Caelum.Senado.Modelo;
using System.Collections.Generic;
using System.Linq;

namespace Caelum.Senado.Testes.Suite
{
    [TestClass]
    public class ComparadorDeDatasTest
    {
        [TestMethod]
        public void DateTimeExploracao()
        {
            var dataMaisAntiga = new DateTime(2017, 1, 1);
            var dataMaisRecente = new DateTime(2017, 1, 2);
            Assert.IsTrue(dataMaisRecente.CompareTo(dataMaisAntiga) > 0);
        }

        [TestMethod]
        public void DateTimeComparadoComNull()
        {
            DateTime? dataNula = null;
            var dataQualquer = new DateTime(2017, 1, 1);
            Assert.IsTrue(dataQualquer.CompareTo(dataNula) > 0);
        }

        [TestMethod]
        public void OrdenaPorDataNaListaDeTramitacoes()
        {
            var dataMaisRecenteEsperada = new DateTime(2017, 1, 1);
            var tramitacoes = new List<Tramitacao>
            {
                new Tramitacao{ Identificacao = new IdentificacaoTramitacao { Data = new DateTime(2003,1,1) } },
                new Tramitacao{ Identificacao = new IdentificacaoTramitacao { Data = new DateTime(2004,1,1) } },
                new Tramitacao{ Identificacao = new IdentificacaoTramitacao { Data = new DateTime(2005,1,1) } },
                new Tramitacao{ Identificacao = new IdentificacaoTramitacao { Data = new DateTime(2002,1,1) } },
                new Tramitacao{ Identificacao = new IdentificacaoTramitacao { Data = new DateTime(2012,1,1) } },
                new Tramitacao{ Identificacao = new IdentificacaoTramitacao { Data = new DateTime(2000,1,1) } },
                new Tramitacao{ Identificacao = new IdentificacaoTramitacao { Data = dataMaisRecenteEsperada } }
            };
            var dataMaisRecente = tramitacoes
                .OrderByDescending(t => t.Identificacao.Data)
                .Select(t => t.Identificacao.Data)
                .First();
            Assert.AreEqual(dataMaisRecente, dataMaisRecenteEsperada);
        }

        [TestMethod]
        public void QuandoNaoHaTramitacaoNaLista()
        {
            var dataQualquer = new DateTime(2017, 1, 1);
            var tramitacoes = new List<Tramitacao>
            {
            };
            var dataMaisRecente = tramitacoes
                .OrderByDescending(t => t.Identificacao.Data)
                .Select(t => t.Identificacao.Data)
                .FirstOrDefault();
            Assert.IsTrue(dataQualquer.CompareTo(dataMaisRecente)>0);
        }
    }
}
