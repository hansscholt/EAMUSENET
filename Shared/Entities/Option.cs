using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace eamuse.Models
{
    [BsonIgnoreExtraElements]
    public class Option
    {
        [BsonId]
        [BsonElement(elementName: "_id")]
        public ObjectId _id { get; set; }

        [BsonElement(elementName: "id")]
        public int id { get; set; }

        [BsonElement(elementName: "refid")]
        public string refid { get; set; }

        [BsonElement(elementName: "common")]
        public string common { get; set; }

        [BsonElement(elementName: "option")]
        public string option { get; set; }

        [BsonElement(elementName: "last")]
        public string last { get; set; }

        [BsonElement(elementName: "rival")]
        public string rival { get; set; }
    }
}
