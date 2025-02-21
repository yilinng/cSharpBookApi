using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace TodoApi.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

      //  [JsonIgnore]
        public string Password { get; set; }
    }
}
