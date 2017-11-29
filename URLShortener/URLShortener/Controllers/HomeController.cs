using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Helpers;
using URLShortener.Models;

namespace URLShortener.Controllers
{
    public class HomeController : Controller
    {
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
    }
}
