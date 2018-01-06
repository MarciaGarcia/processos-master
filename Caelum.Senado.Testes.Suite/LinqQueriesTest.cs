using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Caelum.Senado.Modelo;
using System.Collections.Generic;
using System.Linq;

namespace Caelum.Senado.Testes.Suite
{
    [TestClass]
    public class LinqQueriesTest
    {
        private IEnumerable<Processo> ProcessosComInteressados()
        {
            var p1 = new Processo { Sigla = "PDS", Numero = 16, Ano = 1984 };
            var p2 = new Processo { Sigla = "PLS", Numero = 193, Ano = 2013 };
            var p3 = new Processo { Sigla = "PLC", Numero = 70, Ano = 2013 };
            var p4 = new Processo { Sigla = "PLS", Numero = 16, Ano = 2014 };
            var p5 = new Processo { Sigla = "PLS", Numero = 5, Ano = 2015 };

            var int1 = new Interessado { Nome = "Fulano", Email = "dpcosta@gmail.com" };
            var int2 = new Interessado { Nome = "Beltrano", Email = "beltrano@gmail.com" };

            //quero que int1 esteja interessado nos processos p1, p3, p5:
            int1.RegistraInteresse(p1, p3, p5);
            int2.RegistraInteresse(p2, p4);

            return new List<Processo> { p1, p2, p3, p4, p5 };
        }

        [TestMethod]
        public void AgrupamentoExploracao()
        {
            //interessados em um processo xyz
            var processos = this.ProcessosComInteressados();

            //var interessados = from p in processos
            //                   from ip in p.Interesses
            //                   //group ip.Interessado by ip.InteressadoId into i
            //                   select ip.Interessado;

            var interessados = processos
                .SelectMany(p => p.Interesses)
                .Select(i => i.Interessado)
                .Distinct();

            Assert.AreEqual(2, interessados.Count());


        }
    }
}
