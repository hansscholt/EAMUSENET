using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace eamuse
{
    [BsonIgnoreExtraElements]
    public class Misc
    {
        [BsonId]
        [BsonElement(elementName: "_id")]
        public ObjectId _id { get; set; }

        [BsonElement(elementName: "lastinsertedtid")] 
        public int lastinsertedtid { get; set; }
    }
}
