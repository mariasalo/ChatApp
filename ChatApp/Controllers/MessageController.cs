using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index(int? id) // int? id jos halutaan, että id:n oletusarvo on nolla
        {
            AcademyChatContext db = new AcademyChatContext();

            if (id != null)
            {
                var messages = from m in db.Message
                               where m.ToPersonId == id || m.ToPersonId == null
                               orderby m.SendTime descending
                               select m;


                ViewBag.Id = id;
                return View(messages.ToList());
            }
            else
            {
                return RedirectToAction("login", "Login");
            }
        }

        [HttpGet]

        public IActionResult Create(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormCollection fc)
        {
            AcademyChatContext db = new AcademyChatContext();

            var id = Convert.ToInt32(fc["FromPersonId"]);
            var newMessage = new Message();
            newMessage.MessageText = fc["MessageText"];
            newMessage.Subject = fc["Subject"];
            newMessage.FromPersonId = id; // pitää muutta int muotoon
            newMessage.SendTime = DateTime.Now;
            newMessage.PrivateMessage = false;
            db.Message.Add(newMessage);
            db.SaveChanges();
            return RedirectToAction("Index", "Message", new { id = id });
        }
    }
}