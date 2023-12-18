using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Bookshop.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Linq;

namespace Bookshop.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;

        public ContactController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Contact()
        {
            ViewData["Title"] = "Kontakt";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(Contact model)
        {
            if (ModelState.IsValid)
            {
                await SendEmail(model);
                ViewBag.Message = "Wiadomość została wysłana";
                return View();
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Error: {error.ErrorMessage}");
            }

            return View(model);
        }

        private async Task SendEmail(Contact model)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = emailSettings["SmtpServer"];

            // Sprawdzenie czy SmtpPort jest dostępny i nie jest null
            if (int.TryParse(emailSettings["SmtpPort"], out var smtpPort))
            {
                var email = emailSettings["EmailAddress"];
                var password = emailSettings["EmailPassword"];

                var message = new MailMessage();
                message.To.Add("bookshopcontact@op.pl");
                message.From = new MailAddress(model.Email);
                message.Subject = model.Subject;
                message.Body = $"Od: {model.FullName}\nEmail: {model.Email}\n\n{model.Message}";

                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(email, password);
                    smtpClient.EnableSsl = true;
                    await smtpClient.SendMailAsync(message);
                }
            }
            else
            {
                // Obsługa błędu, gdy SmtpPort nie jest liczbą
                // Możesz dodać odpowiednie logowanie lub inny sposób obsługi błędu
                // Na przykład:
                // logger.LogError("Invalid SmtpPort value");
                // throw new ApplicationException("Invalid SmtpPort value");
            }
        }
    }
}
