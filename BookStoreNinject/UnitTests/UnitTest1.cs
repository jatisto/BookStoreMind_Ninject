using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.HtmlHelpers;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (Arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>()
            {
                new Book(){BookId = 1, Name = "Book1"},
                new Book(){BookId = 2, Name = "Book2"},
                new Book(){BookId = 3, Name = "Book3"},
                new Book(){BookId = 4, Name = "Book4"},
                new Book(){BookId = 5, Name = "Book5"}
            });

            // Действие (act)
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            BooksListVM result = (BooksListVM)controller.List(2).Model;

            // Утверждение
            List<Book> books = result.Books.ToList();
            Assert.IsTrue(books.Count == 2);
            Assert.AreEqual(books[0].Name, "Book4");
            Assert.AreEqual(books[1].Name, "Book5");

        }

        [TestMethod]
        public void Can_Page_Generate_Links()
        {
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo()
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>"
                , result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (Arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>()
            {
                new Book(){BookId = 1, Name = "Book1"},
                new Book(){BookId = 2, Name = "Book2"},
                new Book(){BookId = 3, Name = "Book3"},
                new Book(){BookId = 4, Name = "Book4"},
                new Book(){BookId = 5, Name = "Book5"}
            });

            // Действие (act)
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            BooksListVM result = (BooksListVM)controller.List(2).Model;

            // Утверждение
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPage, 2);
        }

    }
}
