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
        [JsonProperty("editora")]
        public string Publisher { get; set; }
        [JsonProperty("anoPub")]
        public string Year { get; set; }
        [JsonProperty("localPub")]
        public string Local { get; set; }
    }
}
