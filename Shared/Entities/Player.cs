using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace eamuse
{
    [BsonIgnoreExtraElements]
    public class Player
    {
        [BsonId]
        [BsonElement(elementName: "_id")]
        public ObjectId _id { get; set; }

        [BsonElement(elementName: "id")]
        public int id { get; set; }

        //[BsonElement(elementName: "playerid")]
        //public int playerid { get; set; }              

        [BsonElement(elementName: "pcbid")]
        public string pcbid { get; set; }

        [BsonElement(elementName: "name")]
        public string name { get; set; }
    }
}
