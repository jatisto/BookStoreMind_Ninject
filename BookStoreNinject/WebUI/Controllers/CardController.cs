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

        public ViewResult Index(Card card, string returnUrl)
        {
            return View(new CardIndexVM()
            {
                Card = card,
                ReturnUrl = returnUrl
            });
        }

        /*public Card GetCard()
        {
            Card card = (Card)Session["Card"];
            if (card == null)
            {
                card = new Card();
                Session["Card"] = card;

            }

            return card;
        }*/

        public RedirectToRouteResult AddToCard(Card card, int bookId, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(b => b.BookId == bookId);

            if (book != null)
            {
                card.AddItem(book, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }


        public RedirectToRouteResult RemoveFormCard(Card card, int bookId, string returnUrl)
        {
            Book book = repository.Books
                .Where(b => b.BookId == bookId)
                .FirstOrDefault();

            if (book != null)
            {
                card.RemoveItem(book);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

    }
}