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
        private IOrderProcessor orderProcessor;
        public CardController(IBookRepository repo, IOrderProcessor order)
        {
            this.repository = repo;
            orderProcessor = order;
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

        public PartialViewResult Summary(Card card)
        {
            return PartialView(card);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Card card, ShippingDetails shippingDetails)
        {
            if (card.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извениет но карзина пустя!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(card, shippingDetails);
                card.Clear();
                return View("Completed");
            }
            else
            {
                return View(new ShippingDetails());
            }
            
        }

    }
}