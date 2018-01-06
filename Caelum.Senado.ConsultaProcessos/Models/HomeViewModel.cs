using Confere.Processos.Database;
using Confere.Processos.Modelo;
using System.Collections.Generic;
using System.Linq;

namespace Confere.ProcessosWeb.Consulta.Models
{
    public class ProcessoAcompanhado
    {
        public Processo Processo { get; set; }
        public bool EstaSendoAcompanhado { get; set; }
    }

    public class HomeViewModel
    {
        public bool AcompanhamentoDuplicado { get; set; }

        public IEnumerable<ProcessoAcompanhado> ProcessosMonitorados { get; }

        // = new List<Processo>
        //{
        //    new Processo  { Sigla =  "PDS", Numero = 16, Ano = 1984 },
        //    new Processo  { Sigla =  "PLS", Numero = 193, Ano = 2013 },
        //    new Processo  { Sigla =  "PLC", Numero = 70, Ano = 2013 },
        //    new Processo  { Sigla =  "PLS", Numero = 16, Ano = 2014 },
        //    new Processo  { Sigla =  "PLS", Numero = 5, Ano = 2015 },
        //    new Processo  { Sigla =  "PLS", Numero = 398, Ano = 2015 },
        //    new Processo  { Sigla =  "PLS", Numero = 410, Ano = 2016 },
        //    new Processo  { Sigla =  "PLS", Numero = 462, Ano = 2016 },
        //    new Processo  { Sigla =  "PLC", Numero = 10, Ano = 2017 },
        //};
        //this.MonitoradosDaCamara = new List<Processo>
        //{
        //    new Processo  { Sigla =  "PL", Numero = 7171, Ano = 2017 },
        //    new Processo  { Sigla =  "PL", Numero = 2546, Ano = 2015 },
        //    new Processo  { Sigla =  "PLC", Numero = 10, Ano = 2017 },
        //    new Processo  { Sigla =  "PL", Numero = 7936, Ano = 1986 },
        //    new Processo  { Sigla =  "PL", Numero = 3890, Ano = 1989 },
        //    new Processo  { Sigla =  "PL", Numero = 2904, Ano = 1992 },
        //    new Processo  { Sigla =  "PL", Numero = 2579, Ano = 1992 },
        //    new Processo  { Sigla =  "PL", Numero = 3925, Ano = 1997 },
        //    new Processo  { Sigla =  "PL", Numero = 4150, Ano = 1998 },
        //    new Processo  { Sigla = "PL", Numero = 6671, Ano = 2002},
        //    new Processo  { Sigla =  "PL", Numero = 880, Ano = 2003 },
        //    new Processo  { Sigla =  "PL", Numero = 1058, Ano = 2003 },
        //    new Processo  { Sigla =  "PL", Numero = 6542, Ano = 2006 },
        //    new Processo  { Sigla =  "PL", Numero = 600, Ano = 2011 },
        //    new Processo  { Sigla =  "PL", Numero = 1004, Ano = 2011 },
        //    new Processo  { Sigla =  "PL", Numero = 4843, Ano = 2012 },
        //    new Processo  { Sigla =  "PL", Numero = 5680, Ano = 2013 },
        //    new Processo  { Sigla =  "PL", Numero = 1206, Ano = 2015 },
        //    new Processo  { Sigla =  "PL", Numero = 1944, Ano = 2015 },
        //    new Processo  { Sigla =  "PL", Numero = 2546, Ano = 2015 },
        //    new Processo  { Sigla =  "PL", Numero = 2668, Ano = 2015 },
        //    new Processo  { Sigla =  "PL", Numero = 3427, Ano = 2015 },
        //    new Processo  { Sigla =  "PL", Numero = 3568, Ano = 2015 },
        //    new Processo  { Sigla =  "PL", Numero = 4793, Ano = 2016 },
        //    new Processo  { Sigla =  "PL", Numero = 4819, Ano = 2016 },
        //    new Processo  { Sigla =  "PL", Numero = 5354, Ano = 2016 },
        //    new Processo  { Sigla =  "PL", Numero = 5364, Ano = 2016 },
        //    new Processo  { Sigla =  "PL", Numero = 7171, Ano = 2017 },
        //    new Processo  { Sigla =  "PEC", Numero = 29, Ano = 2003 },
        //    new Processo  { Sigla =  "PEC", Numero = 185, Ano = 2003 },
        //    new Processo  { Sigla =  "PEC", Numero = 314, Ano = 2004 },
        //    new Processo  { Sigla =  "PLP", Numero = 399, Ano = 2008 },
        //    new Processo  { Sigla =  "PLP", Numero = 448, Ano = 2014 },
        //    new Processo  { Sigla =  "PLP", Numero = 25, Ano = 2007 },
        //};

        public HomeViewModel(int processoSendoAcompanhado = 0)
        {
            using (var contexto = new ProcessoContext())
            {
                this.ProcessosMonitorados = contexto
                    .Processos
                    .OrderByDescending(p => p.DataUltimaAtualizacao)
                    .Select(p => new ProcessoAcompanhado {
                        Processo = p,
                        EstaSendoAcompanhado = (p.Id == processoSendoAcompanhado)
                    })
                    .ToList();
            }
        }
    }
}