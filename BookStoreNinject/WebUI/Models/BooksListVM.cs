using System.Collections.Generic;
using Domain.Entities;

namespace WebUI.Models
{
    public class BooksListVM
    {
        public IEnumerable<Book> Books { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentGenre { get; set; }
    }
}