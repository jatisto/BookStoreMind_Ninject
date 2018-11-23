using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;

namespace WebUI.Controllers
{
    public class BooksController : Controller
    {
        private IBookRepository repository;

        public BooksController(IBookRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult List()
        {
            return View(repository.Books);
        }

    }
}