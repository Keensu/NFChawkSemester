using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NFChawk.Controllers
{
    public class ErrorController : Controller
    {
        // GET: ErrorController
        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult Error403()
        {
            return View();
        }
    }
}
