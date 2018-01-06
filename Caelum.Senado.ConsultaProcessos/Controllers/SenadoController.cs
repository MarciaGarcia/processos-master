using Confere.Processos.Service;
using Confere.Processos.Modelo;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Confere.ProcessosWeb.Consulta.Controllers
{
    [Route("/senado")]
    public class SenadoController : Controller
    {
        public async Task<ActionResult> Index(Processo processo)
        {
            if (ModelState.IsValid)
            {
                var service = new MateriasService();
                var materia = await service.RecuperarMateriaPeloCodigo(processo);
                return View(materia);
            }
            return View("Index", "Home");
        }
    }
}