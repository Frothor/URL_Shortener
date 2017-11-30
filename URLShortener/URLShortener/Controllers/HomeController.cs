using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Helpers;
using URLShortener.Models;
using Microsoft.AspNetCore.Identity;
using URLShortener.ViewModels;

namespace URLShortener.Controllers
{
    public class HomeController : Controller
    {
        protected UserManager<IdentityUser> UserManager;
        protected SignInManager<IdentityUser> SignInManager;
        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [Route("/{link}")]
        public IActionResult ShortLink(string link, [FromServices] Context context)
        {
            var linkFound = context.Links.SingleOrDefault(x => x.Short == link);
            if (linkFound == null)
            {
                return NotFound();
            }
            else
            {
                var originalLink = linkFound.Long;
                linkFound.NumberOfClicks += 1;
                context.SaveChanges();
                return Redirect(originalLink);
            }

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Result(Link link, [FromServices] Context context)
        {
            if (ModelState.IsValid)
            {
                var linkIsInDatabase = context.Links.SingleOrDefault(x => x.Long == link.Long);

                if (linkIsInDatabase == null)
                {
                    link.NumberOfClicks = 0;
                    link.Short = Shortener.Hash(link.Long);
                    context.Links.Add(link);
                    context.SaveChanges();
                    ViewBag.Message = "http://localhost:59290/" + link.Short;
                    return View();
                }
                else
                {
                    ViewBag.Message = "http://localhost:59290/" + linkIsInDatabase.Short;
                    return View();
                }
            }
            
            return View("Index", link);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser(registerViewModel.Login);
                user.Email = registerViewModel.Email;
                IdentityResult identityResult = await UserManager.CreateAsync(user, registerViewModel.Password);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                }

            }
            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }
    }
}
