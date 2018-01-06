using Caelum.Senado.Database;
using Caelum.Senado.Modelo;
using Caelum.Senado.Service;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Caelum.Senado.Agendador
{
    /*para cada processo sendo monitorado:
    * - ir na internet e buscar suas info
    * - comparar data da última tramitação da internet com data da última tramitação gravada 
    * - se data tramitação internet for mais recente, guardar processo numa lista
    *
    * se lista de processos atualizados não estiver vazia:
    * > para cada processo: atualizar data de última tramitação
    * 
    * > agrupar por interessado; para cada interessado:
    * - enviar email com lista de processos atualizados
    * - conteúdo do email será a lista de processos <li> com link para a página de detalhe no sistema <a>
    */
    public class VerificadorDeAtualizacoesNoProcessoJob : IJob
    {
        private async Task VerificaAtualizacoes()
        {
            var processosAtualizados = new List<Processo>();
            using (var servico = new MateriasService())
            {
                foreach (var processo in servico.ProcessosMonitorados)
                {
                    //consulta o web service para recuperar as tramitações da matéria:
                    var movimentacao = await servico.TramitacoesDaMateria(processo.Codigo);
                    //recupera a data mais recente de tramitação:
                    var dataUltimaTramitacao = movimentacao.Materia.Tramitacoes
                        .OrderByDescending(t => t.Identificacao.Data)
                        .Select(t => t.Identificacao.Data)
                        .First();
                    if (dataUltimaTramitacao.CompareTo(processo.DataUltimaAtualizacao) > 0)
                    {
                        Trace.TraceInformation($"Processo {processo} teve atualizações.");
                        processo.DataUltimaAtualizacao = dataUltimaTramitacao;
                        processosAtualizados.Add(processo);
                    }
                }
            }

            if (processosAtualizados.Count > 0)
            {
                using (var contexto = new ProcessoContext())
                {
                    Trace.TraceInformation("Persistindo processos atualizados.");
                    contexto.Processos.AddRange(processosAtualizados);
                    contexto.SaveChanges();
                }

                EnviarEmailAtualizacao(processosAtualizados.Select(p => p.Id));
            } else
            {
                EnviarEmailAdministrativo();
            }
        }

        private void EnviarEmailAdministrativo()
        {
            
            EnviarEmail(
                toEmail: "dpcosta@gmail.com", 
                assuntoMensagem: "[Consulta Processos] Notificação de processos atualizados", 
                corpoMensagem: "Não houveram novas tramitações para os processos monitorados."
            );
        }

        public void Execute(IJobExecutionContext context)
        {
            VerificaAtualizacoes().Wait();
        }

        private void EnviarEmail(string toEmail, string assuntoMensagem, string corpoMensagem)
        {
            Trace.TraceInformation($"Email para: {toEmail}, assunto: {assuntoMensagem}, corpo: {corpoMensagem}");
            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("dpcosta@gmail.com", "sgdixptpycnaqrmk")
            };

            try
            {
                // Create instance of message
                MailMessage message = new MailMessage();

                // Add receiver
                message.To.Add(toEmail);

                // Sender
                message.From = new MailAddress("dpcosta@gmail.com");

                // Subject
                message.Subject = assuntoMensagem;

                // Body
                message.Body = corpoMensagem;

                // Send the message
                client.Send(message);

                // Clean up
                message = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not send e-mail. Exception caught: " + e);
            }
        }

        private void EnviarEmailAtualizacao(IEnumerable<int> processosAtualizados)
        {
            using (var contexto = new ProcessoContext())
            {
                var interessados = contexto.Processos
                    .Where(p => processosAtualizados.Contains(p.Id))
                    .SelectMany(p => p.Interesses)
                    .Select(i => i.Interessado)
                    .Distinct()
                    .ToList();

                foreach(var interessado in interessados)
                {
                    var processos = contexto.Interessados
                        .Where(i => i.Id == interessado.Id)
                        .SelectMany(i => i.Interesses)
                        .Select(ip => ip.Processo)
                        .Distinct();

                    var corpoMensagem = $"Olá, {interessado.Nome}\n\r";
                    corpoMensagem += "Dos processos que você está acompanhando, os seguintes foram atualizados:";
                    foreach (var p in processos)
                    {
                        corpoMensagem += "\n\r" + p.ToString();
                    }

                    EnviarEmail(interessado.Email, "[Consulta Processos] Notificação de processos atualizados", corpoMensagem);
                    EnviarEmail("dpcosta@gmail.com", "[Consulta Processos] Notificação de processos atualizados", corpoMensagem);
                    EnviarEmail("marcinhagarciarj@gmail.com", "[Consulta Processos] Notificação de processos atualizados", corpoMensagem);

                }
            }
        }
    }
}
