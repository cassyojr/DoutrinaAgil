using System.Collections.Generic;

namespace DoutrinaAgil.Service.Dto
{
    public class DoctrinesDto
    {
        public DoctrinesDto()
        {
        }

        public DoctrinesDto(BookDto book)
        {
            Book = book;
            Contents = new List<ContentDto>();
        }

        public DoctrinesDto(BookDto book, List<ContentDto> contents)
        {
            Book = book;
            Contents = Contents;
        }

        public BookDto Book { get; set; }
        public List<ContentDto> Contents { get; set; }
    }
}
