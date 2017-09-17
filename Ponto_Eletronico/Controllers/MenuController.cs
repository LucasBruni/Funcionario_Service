using System.Web.Mvc;
using Ponto_Eletronico.Models;

namespace Ponto_Eletronico.Controllers
{
    public class MenuController : Controller
    {
        public ActionResult Menu_Adm(Funcionario funcionario)
        {
            ViewBag.Title = "Home Page";

            return View(funcionario);
        }

        public ActionResult Menu_Comum()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

    }
}