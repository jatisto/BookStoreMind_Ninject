using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Domain.Abstract;

namespace WebUI.Controllers
{
    public class BooksController : Controller
    {
        private IBookRepository repository;
        public int pageSize = 4;

        public BooksController(IBookRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult List(int page = 1)
        {
            return View(repository.Books
                .OrderBy(book => book.BookId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize));
        }

    }
}