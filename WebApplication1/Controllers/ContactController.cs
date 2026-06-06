using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFChawk.Models.Interfaces;
using NFChawk.Models.ViewModels;

namespace NFChawk.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        // GET: ContactController
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateCollection()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Send(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            string body = $@"
                <h2>New message from the website</h2>

                <p><strong>Name:</strong> {model.Name}</p>
                <p><strong>Email:</strong> {model.Email}</p>
                <p><strong>Subject:</strong> {model.Subject}</p>
                <p><strong>Message  :</strong></p>
                <p>{model.Message}</p>
            ";

            await _emailService.SendEmailAsync(
                "work3do1send@gmail.com",
                $"Contact Form: {model.Subject}",
                body
            );

            TempData["Success"] = "Message sent successfully!";

            return RedirectToAction("Index");
        }

    }
}
