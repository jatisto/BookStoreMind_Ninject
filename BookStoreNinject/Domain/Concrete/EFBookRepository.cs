using System.Collections.Generic;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFBookRepository : IBookRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Book> Books
        {
            get { return context.Books;}
        }
    }
}