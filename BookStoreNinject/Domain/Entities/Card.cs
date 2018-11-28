using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Card
    {
        private List<CardLine> lineColection = new List<CardLine>();

        public IEnumerable<CardLine> Lines
        {
            get { return lineColection; }
        }

        public void AddItem(Book book, int quantity)
        {
            CardLine line = lineColection
                .Where(b => b.Book.BookId == book.BookId)
                .FirstOrDefault();

            if (line == null) // Если корзина пуста, то создать новый товар
            {
                lineColection.Add(new CardLine(){ Book = book, Quantity = quantity });
            }
            else // Если нет то прибавить у уже существующему товару
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveItem(Book book)
        {
            lineColection.RemoveAll(l => l.Book.BookId == book.BookId);
        }

        public decimal ComputeTotalValue()
        {
            return lineColection.Sum(e => e.Book.Price * e.Quantity);
        }

        public void Clear()
        {
            lineColection.Clear();
        }

    }

    public class CardLine
    {
        public Book Book { get; set; } // Товар
        public int  Quantity { get; set; } // Кол-во товара
    }
}