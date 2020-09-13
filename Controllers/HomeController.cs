using System;
using System.Linq;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataProtector dataProtector;

        public HomeController(IDataProtectionProvider dataProtectionProvider)
        {
            dataProtector = dataProtectionProvider.CreateProtector(purpose: "UserIdProtection");
        }

        public IActionResult Index()
        {
            // Get a random user
            var user = GetUser(1);

            ViewData["PlainUserId"] = user.Id;
            ViewData["UserName"] = user.Name;
            ViewData["EncryptedUserId"] = dataProtector.Protect(user.Id.ToString());

            return View();
        }

        public IActionResult ViewUser(string id)
        {
            // Decrypt user Id
            var decryptedUserId = Convert.ToInt32(dataProtector.Unprotect(id));

            // Get the user
            var user = GetUser(decryptedUserId);

            ViewData["PlainUserId"] = user.Id;
            ViewData["UserName"] = user.Name;

            return View();
        }

        private static readonly User[] _users =
        {
            new User { Id = 1, Name = "Sarah Phillips" },
            new User { Id = 2, Name = "James McVoy" },
            new User { Id = 3, Name = "Bob Lambo" },
            new User { Id = 4, Name = "Matt Delonge" },
            new User { Id = 5, Name = "Sue West" }
        };

        private User GetUser(int id)
        {
            return _users.FirstOrDefault(user => user.Id == id);
        }
    }

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}