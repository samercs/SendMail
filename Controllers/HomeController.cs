using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SendMail.Models;

namespace SendMail.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SendMail()
        {
            var model = new SendMailVm();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult SendMail(SendMailVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var message = new MimeMessage()
            {
                Subject = model.Subject,
                Body = new TextPart("plain")
                {
                    Text = model.Message
                }
            };
            message.To.Add(new MailboxAddress("samer", model.To));
            message.From.Add(new MailboxAddress("samer", model.From));
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);

            }
            return View(model);
        }
    }

    public class SendMailVm
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string From { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string To { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
