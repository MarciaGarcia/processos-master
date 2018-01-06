using Confere.Processos.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caelum.Senado.ConsultaProcessos.Controllers
{
    public class InteressadoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Processos { get; set; }
    }

    public class InteressadoController : Controller
    {
        // GET: Interessado
        public ActionResult Index()
        {
            using (var ctx = new ProcessoContext())
            {
                var interessados = ctx.Interessados
                    .Include(i => i.Interesses)
                    .ThenInclude(ii => ii.Processo)
                    .ToList();
                var interessadosView = new List<InteressadoViewModel>();
                foreach (var item in interessados)
                {
                    var obj = new InteressadoViewModel
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        Email = item.Email
                    };
                    foreach (var interesse in item.Interesses)
                    {
                        obj.Processos += interesse.Processo.ToString() + ", ";
                    }
                    interessadosView.Add(obj);
                }
                return View(interessadosView);
            }
        }
    }
}