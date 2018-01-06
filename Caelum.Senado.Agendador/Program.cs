using Caelum.Senado.Database;
using Caelum.Senado.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Diagnostics;
using System.Linq;

namespace Caelum.Senado.Agendador
{
    class Program
    {
        static void Main(string[] args)
        {
            //garante que o banco de dados estará atualizado para as versões mais recentes
            using (var contexto = new ProcessoContext())
            {
                Trace.TraceInformation("Atualizando banco de dados se necessário...");
                contexto.Database.Migrate();
            }

            //a cada x tempo, consultar a web para verificar se existem atualizações
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();

            var job = JobBuilder.Create<VerificadorDeAtualizacoesNoProcessoJob>().Build();

            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever())
                            .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        /// <summary>
        /// Apenas para testes
        /// </summary>
        private static void CadastrarInteressados()
        {
            using (var contexto = new ProcessoContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(new MyFilteredLoggerProvider());

                var processosMonitorados = contexto.Processos;
                var daniel = new Interessado() { Nome = "Daniel Portugal", Email = "dpcosta@gmail.com" };
                var marcia = new Interessado() { Nome = "Marcia Garcia", Email = "marcinhagarciarj@gmail.com" };

                daniel.RegistraInteresse(processosMonitorados);
                marcia.RegistraInteresse(processosMonitorados);

                contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Apenas para testes
        /// </summary>
        private static void TestarQueries()
        {
            var processosAtualizados = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            using (var contexto = new ProcessoContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(new MyFilteredLoggerProvider());

                var interessados = contexto.Processos
                    .Where(p => processosAtualizados.Contains(p.Id))
                    .SelectMany(p => p.Interesses )
                    .Select( i => i.Interessado )
                    .Distinct()
                    .ToList(); //para executar logo a consulta

                foreach (var interessado in interessados)
                {
                    var processos = contexto.Interessados
                        .Where(i => i.Id == interessado.Id)
                        .SelectMany(i => i.Interesses)
                        .Select(ip => ip.Processo)
                        .Distinct();

                    var corpoMensagem = "Os seguintes processos foram atualizados:";
                    foreach (var p in processos)
                    {
                        corpoMensagem += "\n\r" + p.ToString();
                    }

                    System.Console.WriteLine(corpoMensagem);
                }
            }
        }
    }
}
