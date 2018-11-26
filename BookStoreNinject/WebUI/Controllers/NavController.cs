using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor;
using Domain.Abstract;
using Domain.Entities;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private IBookRepository repository;
        

        public NavController(IBookRepository repo)
        {
            this.repository = repo;
        }
        public PartialViewResult Menu(string genre = null)
        {
            ViewBag.SelecteGenre = genre;
            IEnumerable<string> genres = repository.Books
                .Select(book => book.Genre)
                .Distinct()
                .OrderBy(x => x);

            return PartialView(genres);
        }
    }
}