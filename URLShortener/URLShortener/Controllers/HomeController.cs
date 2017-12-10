using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Helpers;
using URLShortener.Models;
using Microsoft.AspNetCore.Identity;
using URLShortener.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace URLShortener.Controllers
{
    public class HomeController : Controller
    {
        protected Context Context { get;}
        protected UserManager<User> UserManager;
        protected SignInManager<User> SignInManager;

        public HomeController(Context context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            Context = context;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [Route("/{link}")]
        public IActionResult ShortLink(string link)
        {
            var linkFound = Context.Links.SingleOrDefault(x => x.Short == link);
            if (linkFound == null)
            {
                return NotFound();
            }
            else
            {
                var originalLink = linkFound.Long;
                linkFound.NumberOfClicks += 1;
                Context.SaveChanges();
                return Redirect(originalLink);
            }

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Result(Link link)
        {
            if (ModelState.IsValid)
            {
                var linkIsInDatabase = Context.Links.SingleOrDefault(x => x.Long == link.Long);

                if (linkIsInDatabase == null)
                {
                    link.NumberOfClicks = 0;
                    link.Short = Shortener.Hash(link.Long);
                    Context.Links.Add(link);
                    Context.SaveChanges();
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


        [HttpPost]
        public IActionResult Personalize(Link link)
        {
            if (ModelState.IsValid)
            {

                if (string.IsNullOrEmpty(link.Short))
                {
                    link.NumberOfClicks = 0;
                    var linkIsInDatabase = Context.Links.SingleOrDefault(x => x.Long == link.Long);

                    if (linkIsInDatabase == null)
                    {
                        link.NumberOfClicks = 0;
                        link.Short = Shortener.Hash(link.Long);
                        Context.Users.Include(x => x.Links).Single(x => x.UserName == User.Identity.Name).Links.Add(link);
                        Context.SaveChanges();
                        ViewBag.Message = "http://localhost:59290/" + link.Short;
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "http://localhost:59290/" + linkIsInDatabase.Short;
                        return View();
                    }
                }
                else
                {

                    var linkIsInDatabase = Context.Links.SingleOrDefault(x => x.Short == link.Short);
                    if (linkIsInDatabase == null)
                    {
                        link.NumberOfClicks = 0;
                        Context.Users.Include(x => x.Links).Single(x => x.UserName == User.Identity.Name).Links.Add(link);
                        Context.SaveChanges();
                        ViewBag.Message = "http://localhost:59290/" + link.Short;
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "This personalized option is already in use!";
                        return View();
                    }
                }

            }

            return View("Index", link);
        }

        public IActionResult ShowLinks(Link link)
        {
            var links = Context.Users.Include(x => x.Links).Single(x => x.UserName == User.Identity.Name).Links.ToList();
            return View(links);
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            var link = Context.Links.Single(x => x.Id == id);
            return View(link);
        }

        [HttpPost]
        public IActionResult ConfirmRemove(int id)
        {

            var link = Context.Links.Single(x => x.Id == id);
            Context.Remove(link);
            Context.SaveChanges();
            return RedirectToAction("ShowLinks");

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var link = Context.Links.Single(x => x.Id == id);
            return View(link);
        }


        [HttpPost]
        public IActionResult Edit(int id, string @short)
        {

            if (string.IsNullOrEmpty(@short))
            {
                Link link = Context.Links.Single(x => x.Id == id);

                if (Context.Links.Count(x => x.Long == link.Long) == 1)
                {
                    link.Short = Shortener.Hash(link.Long);
                    Context.Links.Update(link);
                    Context.SaveChanges();
                    return RedirectToAction("ShowLinks");
                }
                else
                {
                    ViewBag.Message = "This shorten form is already in database!";
                    return View(link);
                }
            }
            else
            {
                var linkIsInDatabase = Context.Links.SingleOrDefault(x => x.Short == @short);

                if (linkIsInDatabase == null)
                {
                    var linkToChange = Context.Links.Single(x => x.Id == id);
                    linkToChange.Short = @short;
                    Context.Links.Update(linkToChange);
                    Context.SaveChanges();
                    return RedirectToAction("ShowLinks");
                }
                else
                {
                    ViewBag.Message = "This shorten form is already in database!";
                    return View();
                }
            }
        }


        //=======================================================
        //Account
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
                var user = new User();
                user.UserName = registerViewModel.Login;
                user.Email = registerViewModel.Email;
                IdentityResult identityResult = await UserManager.CreateAsync(user, registerViewModel.Password);
                if (identityResult.Succeeded)
                {
                    await SignInManager.SignInAsync(user, true);

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

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel logInViewModel)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await SignInManager.PasswordSignInAsync(logInViewModel.Login, logInViewModel.Password, true, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login or password incorrect!");
                }
            }

            return View(logInViewModel);

        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }
    }
}
