using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;

namespace UnitTests
{
    [TestClass]
    public class CardTest
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Организация
            Book book1 = new Book { BookId = 1, Name = "Book1" }; // Создаём данные
            Book book2 = new Book { BookId = 2, Name = "Book2" };

            Card card = new Card();

            // Действие
            card.AddItem(book1, 1); // добовляем данные
            card.AddItem(book2, 1);
            List<CardLine> results = card.Lines.ToList(); // Получаем данные

            // Утвержение
            Assert.AreEqual(results.Count(), 2); // проверяем колличество
            Assert.AreEqual(results[0].Book, book1); // проверяем по экземпляру отдельно
            Assert.AreEqual(results[1].Book, book2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация
            Book book1 = new Book { BookId = 1, Name = "Book1" };
            Book book2 = new Book { BookId = 2, Name = "Book2" };

            Card card = new Card();

            // Действие
            card.AddItem(book1, 1);
            card.AddItem(book2, 1);
            card.AddItem(book1, 5);
            List<CardLine> results = card.Lines.OrderBy(c => c.Book.BookId).ToList();

            // Утвержение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация
            Book book1 = new Book { BookId = 1, Name = "Book1" };
            Book book2 = new Book { BookId = 2, Name = "Book2" };
            Book book3 = new Book { BookId = 3, Name = "Book3" };

            Card card = new Card();

            // Действие
            card.AddItem(book1, 1);
            card.AddItem(book2, 1);
            card.AddItem(book1, 5);
            card.AddItem(book3, 2);
            card.RemoveItem(book2);

            // Утвержение
            Assert.AreEqual(card.Lines.Where(c => c.Book == book2).Count(), 0);
            Assert.AreEqual(card.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация
            Book book1 = new Book { BookId = 1, Name = "Book1", Price = 100 };
            Book book2 = new Book { BookId = 2, Name = "Book2", Price = 55 };

            Card card = new Card() ;

            // Действие
            card.AddItem(book1, 1);
            card.AddItem(book2, 1);
            card.AddItem(book1, 5);
            decimal result = card.ComputeTotalValue();

            // Утвержение
            Assert.AreEqual(result, 655);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация
            Book book1 = new Book { BookId = 1, Name = "Book1", Price = 100 };
            Book book2 = new Book { BookId = 2, Name = "Book2", Price = 55 };

            Card card = new Card();

            // Действие
            card.AddItem(book1, 1);
            card.AddItem(book2, 1);
            card.AddItem(book1, 5);
            card.Clear();

            // Утвержение
            Assert.AreEqual(card.Lines.Count(), 0);
        }


        [TestMethod]
        public void Can_Add_Too_Card()
        {
            // Организация
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book>()
            {
                new Book(){BookId = 1, Name = "Book1", Genre = "Genre1"}
            });
            Card card = new Card();

            CardController controller = new CardController(mock.Object, null);
            // Действие
            controller.AddToCard(card, 1, null);

            // Утвержение
            Assert.AreEqual(card.Lines.Count(), 1);
            Assert.AreEqual(card.Lines.ToList()[0].Book.BookId, 1);
        }


        [TestMethod]
        public void Adding_Book_To_Card_Goes_To_Card_Screen()
        {
            // Организация
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book>()
            {
                new Book(){BookId = 1, Name = "Book1", Genre = "Genre1"}
            });
            Card card = new Card();

            CardController controller = new CardController(mock.Object, null);
            // Действие
             RedirectToRouteResult result = controller.AddToCard(card, 1, "myUrl");

            // Утвержение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Card_Conteins()
        {
            // Организация
            Card card = new Card();

            CardController target = new CardController(null, null);
            // Действие
            CardIndexVM result = (CardIndexVM)target.Index(card,"myUrl").ViewData.Model;

            // Утвержение
            Assert.AreEqual(result.Card, card);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Card()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Card card = new Card();
            ShippingDetails shippingDetails = new ShippingDetails();

            CardController controller = new CardController(null, mock.Object);

            ViewResult result = controller.Checkout(card, shippingDetails);
            mock.Verify(m => m.ProcessOrder(It.IsAny<Card>(), It.IsAny<ShippingDetails>()), Times.Never);

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);

        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Card card = new Card();
            card.AddItem(new Book(), 1);

            CardController controller = new CardController(null, mock.Object);
            controller.ModelState.AddModelError("error","error");

            ViewResult result = controller.Checkout(card, new ShippingDetails());
            mock.Verify(m => m.ProcessOrder(It.IsAny<Card>(), It.IsAny<ShippingDetails>()), Times.Never);

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);

        }

        [TestMethod]
        public void Cannot_Checkout_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Card card = new Card();
            card.AddItem(new Book(), 1);

            CardController controller = new CardController(null, mock.Object);

            ViewResult result = controller.Checkout(card, new ShippingDetails());
            mock.Verify(m => m.ProcessOrder(It.IsAny<Card>(), It.IsAny<ShippingDetails>()), Times.Once());

            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);

        }
    }
}
