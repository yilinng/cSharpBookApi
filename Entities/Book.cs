using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace TodoApi.Models
{
    //https://stackoverflow.com/questions/65157992/immutable-field-id-was-found-to-have-been-altered-c-sharp-mongo-driver
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        [BsonElement("Name")]
        [JsonProperty("Name")]
        public string BookName { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }
    }
}
