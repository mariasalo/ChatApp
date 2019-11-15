using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string NickName)
        {
            using (var db = new AcademyChatContext())
            {
                var found = (from p in db.Person
                            where p.NickName == NickName
                            select p).FirstOrDefault();
                if (found != null)
                {
                    return RedirectToAction("Index", "Message", new { id = found.PersonId });
                }
                else
                {
                    ModelState.AddModelError("NickName", "Nickname does not exist. Please create a new profile");
                    return View();
                }
            }
        }
    }
}