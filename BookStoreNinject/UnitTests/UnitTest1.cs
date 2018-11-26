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
                new Book() {BookId = 1, Name = "Book1"},
                new Book() {BookId = 2, Name = "Book2"},
                new Book() {BookId = 3, Name = "Book3"},
                new Book() {BookId = 4, Name = "Book4"},
                new Book() {BookId = 5, Name = "Book5"}
            });

            // Действие (act)
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            BooksListVM result = (BooksListVM) controller.List(null, 2).Model;

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
                new Book() {BookId = 1, Name = "Book1"},
                new Book() {BookId = 2, Name = "Book2"},
                new Book() {BookId = 3, Name = "Book3"},
                new Book() {BookId = 4, Name = "Book4"},
                new Book() {BookId = 5, Name = "Book5"}
            });

            // Действие (act)
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            BooksListVM result = (BooksListVM) controller.List(null, 2).Model;

            // Утверждение
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPage, 2);
        }

        [TestMethod]
        public void Can_Filter_Genre()
        {
            // Организация (Arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>()
            {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1"},
                new Book {BookId = 2, Name = "Book2", Genre = "Genre2"},
                new Book {BookId = 3, Name = "Book3", Genre = "Genre1"},
                new Book {BookId = 4, Name = "Book4", Genre = "Genre3"},
                new Book {BookId = 5, Name = "Book5", Genre = "Genre2"}
            });

            // Действие (act)
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            List<Book> result = ((BooksListVM) controller.List("Genre2", 1).Model).Books.ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Book2" && result[0].Genre == "Genre2");
            Assert.IsTrue(result[1].Name == "Book5" && result[1].Genre == "Genre2");
        }

        [TestMethod]
        public void Can_Create_Category()
        {
            // Организация (Arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>()
            {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1"},
                new Book {BookId = 2, Name = "Book2", Genre = "Genre2"},
                new Book {BookId = 3, Name = "Book3", Genre = "Genre1"},
                new Book {BookId = 4, Name = "Book4", Genre = "Genre3"},
                new Book {BookId = 5, Name = "Book5", Genre = "Genre2"}
            });

            // Действие (act)
            NavController target = new NavController(mock.Object);

            List<string> result = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual(result[0], "Genre1");
            Assert.AreEqual(result[1], "Genre2");
            Assert.AreEqual(result[2], "Genre3");
        }

        [TestMethod]
        public void Can_Indicator_SelectGenre()
        {
            // Организация (Arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>()
            {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1"},
                new Book {BookId = 2, Name = "Book2", Genre = "Genre2"},
                new Book {BookId = 3, Name = "Book3", Genre = "Genre1"},
                new Book {BookId = 4, Name = "Book4", Genre = "Genre3"},
                new Book {BookId = 5, Name = "Book5", Genre = "Genre2"}
            });

            // Действие (act)
            NavController target = new NavController(mock.Object);

            string genreToSelect = "Genre2";
            string result = target.Menu(genreToSelect).ViewBag.SelecteGenre;

            // Утверждение
            Assert.AreEqual(genreToSelect, result);
        }

        [TestMethod]
        public void Generate_Genre_Spesific_Boo_Count()
        {
            // Организация (Arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>()
            {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1"},
                new Book {BookId = 2, Name = "Book2", Genre = "Genre2"},
                new Book {BookId = 3, Name = "Book3", Genre = "Genre1"},
                new Book {BookId = 4, Name = "Book4", Genre = "Genre3"},
                new Book {BookId = 5, Name = "Book5", Genre = "Genre2"}
            });

            // Действие (act)
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            int res1 = ((BooksListVM)controller.List("Genre1").Model).PagingInfo.TotalItems;
            int res2 = ((BooksListVM)controller.List("Genre2").Model).PagingInfo.TotalItems;
            int res3 = ((BooksListVM)controller.List("Genre3").Model).PagingInfo.TotalItems;
            int resAll = ((BooksListVM)controller.List(null).Model).PagingInfo.TotalItems;
            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}