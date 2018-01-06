using Confere.ProcessosWeb.Consulta.Models;
using Confere.Processos.Database;
using Confere.Processos.Service;
using Confere.Processos.Modelo;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Confere.ProcessosWeb.Consulta.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //enviar os processos que estão sendo monitorados
            var model = new HomeViewModel();
            return View(model);
        }

        public async Task<ActionResult> CadastrarProcesso(Processo processo)
        {
            if (ModelState.IsValid)
            {
                using (var contexto = new ProcessoContext())
                using(var servico = new MateriasService())
                {
                    Materia materia = await servico.RecuperarMateriaPeloCodigo(processo);
                    processo.Sigla = processo.Sigla.ToUpper();
                    processo.Emenda = materia.DadosBasicos.Ementa;
                    processo.Codigo = materia.Identificacao.Codigo;
                    processo.Origem = OrigemProcesso.Senado;
                    processo.DataUltimaAtualizacao = materia.Tramitacoes
                        .OrderByDescending(t => t.Identificacao.Data)
                        .Select(t => t.Identificacao.Data)
                        .First();
                    contexto.Processos.Add(processo);
                    contexto.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View("Index", new HomeViewModel());
        }

        public ActionResult ExcluirProcesso(int id)
        {
            using (var contexto = new ProcessoContext())
            {
                var processo = contexto.Processos.Find(id);
                contexto.Processos.Remove(processo);
                contexto.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult AcompanharProcesso(int processoId, string nome, string email)
        {
            using (var contexto = new ProcessoContext())
            {
                //verificar se o interesse já foi registrado
                var processo = contexto.Processos
                    .Where(p => p.Id == processoId)
                    .Include(p => p.Interesses)
                    .ThenInclude(i => i.Interessado)
                    .Single();

                if (processo.Interesses.Where(i => i.Interessado.Email == email).Count() > 0)
                {
                    ViewBag.MensagemDeAcompanhamentoJaRealizado = "Você já está acompanhando esse processo.";
                    var model = new HomeViewModel(processoId);
                    return View("Index", model);
                }

                var interessado = contexto
                        .Interessados
                        .Where(i => i.Email == email)
                        .FirstOrDefault();
                if (interessado == null)
                {
                    interessado = new Interessado { Nome = nome, Email = email };
                }
                interessado.RegistraInteresse(processo);
                contexto.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult AtualizaDataDoProcesso(int id)
        {
            var novaData = new DateTime(2000, 1, 1);
            Trace.TraceInformation($"Atrasando data do processo {id}...");
            using (var ctx = new ProcessoContext())
            {
                var processo = ctx.Processos.Find(id);
                if(processo != null)
                {
                    processo.DataUltimaAtualizacao = novaData;
                    ctx.Processos.Update(processo);
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

    }
}