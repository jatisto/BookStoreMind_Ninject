using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;

namespace WebUI.Controllers
{
    public class CardController : Controller
    {
        private IBookRepository repository;
        public CardController(IBookRepository repo)
        {
            this.repository = repo;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CardIndexVM()
            {
                Card = GetCard(),
                ReturnUrl = returnUrl
            });
        }

        public Card GetCard()
        {
            Card card = (Card)Session["Card"];
            if (card == null)
            {
                card = new Card();
                Session["Card"] = card;

            }

            return card;
        }

        public RedirectToRouteResult AddToCard(int bookId, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(b => b.BookId == bookId);

            if (book != null)
            {
                GetCard().AddItem(book, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }


        public RedirectToRouteResult RemoveFormCard(int bookId, string returnUrl)
        {
            Book book = repository.Books
                .Where(b => b.BookId == bookId)
                .FirstOrDefault();

            if (book != null)
            {
                GetCard().RemoveItem(book);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

    }
}