using Newtonsoft.Json;

namespace DoutrinaAgil.Service.Dto
{
    public class BookDto
    {
        [JsonProperty("book_id")]
        public int Id { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
