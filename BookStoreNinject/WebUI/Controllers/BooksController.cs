using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Domain.Abstract;
using WebUI.Models;

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
            BooksListVM model = new BooksListVM()
            {
                Books = repository.Books
                    .OrderBy(book => book.BookId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Books.Count()
                }
            };
            return View(model);
        }

    }
}