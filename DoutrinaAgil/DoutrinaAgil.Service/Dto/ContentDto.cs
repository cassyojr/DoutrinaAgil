using Newtonsoft.Json;

namespace DoutrinaAgil.Service.Dto
{
    public class ContentDto
    {
        [JsonProperty("book_id")]
        public int BookId { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("texto")]
        public string Text { get; set; }
    }
}
