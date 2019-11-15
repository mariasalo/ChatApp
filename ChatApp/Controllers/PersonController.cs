using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class PersonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string nickname)
        {
            using (var db = new AcademyChatContext())
            { 
                 var user = db.Person.SingleOrDefault(x => x.NickName == nickname);

                if (user != null)
                {
                    return RedirectToAction("Index", "Message", new { id = user.PersonId });
                }
                else
                {

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string NickName, string Hometown, string Description)
        {
            using (var db = new AcademyChatContext())
            {
                var found = db.Person.Where(x => x.NickName == NickName);
                if (found.Count() == 0)
                {
                    var newProfile = new Person();
                    newProfile.NickName = NickName;
                    newProfile.Hometown = Hometown;
                    newProfile.Description = Description;
                    newProfile.RegistrationDate = DateTime.Now;

                    db.Add(newProfile);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Message", new { id = newProfile.PersonId });
                }
                else
                {
                    ModelState.AddModelError("NickName", "Nickname is already in use. Choose another.");
                    if (string.IsNullOrEmpty(Description))
                    {
                        ModelState.AddModelError("Description", "Description cannot be empty.");
                    }
                    return View();
                }
            }
        }
    }
}