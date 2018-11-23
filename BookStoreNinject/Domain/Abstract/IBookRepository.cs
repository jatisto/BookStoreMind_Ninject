using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IBookRepository
    {
        IEnumerable<Book> Books { get; }
    }
}