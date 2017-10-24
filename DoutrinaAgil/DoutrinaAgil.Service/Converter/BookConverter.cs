using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using DoutrinaAgil.Service.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DoutrinaAgil.Service.Converter
{
    public class BookConverter
    {
        public static string OrganizeBooksContent(HttpResponseMessage httpResponseMessage)
        {
            var result = JObject.Parse(httpResponseMessage.Content.ReadAsStringAsync().Result);

            if (!result.HasValues)
                return DoctrinesToJson(new List<DoctrinesDto>());

            var booksJson = result["livros"].ToString();
            var contentJson = result["doutrinas"].ToString();
            var books = JsonConvert.DeserializeObject<List<BookDto>>(booksJson);
            var contents = JsonConvert.DeserializeObject<List<ContentDto>>(contentJson);

            return DoctrinesToJson(JoinBooksWithDoctrines(books, contents));
        }

        private static IEnumerable<DoctrinesDto> JoinBooksWithDoctrines(IList<BookDto> books, IList<ContentDto> contents)
        {
            if (books == null)
                return null;

            var doctrines = new List<DoctrinesDto>();

            foreach (var book in books)
            {
                var doctrine = new DoctrinesDto(book);

                foreach (var content in contents.Where(x => x.BookId == book.Id))
                    doctrine.Contents.Add(content);

                doctrines.Add(doctrine);
            }

            return doctrines;
        }

        private static string DoctrinesToJson(IEnumerable<DoctrinesDto> doctrines)
        {
            return JsonConvert.SerializeObject(doctrines);
        }
    }
}
