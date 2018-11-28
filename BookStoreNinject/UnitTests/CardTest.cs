using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
