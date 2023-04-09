using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace eamuse
{
    [BsonIgnoreExtraElements]
    public class Card
    {
        [BsonId]
        [BsonElement(elementName: "_id")]
        public ObjectId _id { get; set; }

        [BsonId]
        [BsonElement(elementName: "id")]
        public int id { get; set; }

        [BsonElement(elementName: "cardid")] 
        public string cardid { get; set; }

        //[BsonElement(elementName: "area")]
        //public string area { get; set; }

        [BsonElement(elementName: "dataid")] 
        public string dataid { get; set; }

        [BsonElement(elementName: "refid")]
        public string refid { get; set; }

        [BsonElement(elementName: "code")]
        public string code { get; set; }

        [BsonElement(elementName: "codeint")]
        public int codeint { get; set; }

        [BsonElement(elementName: "pcbid")]
        public string pcbid { get; set; }

        //[BsonElement(elementName: "player")]
        //public ObjectId player { get; set; }

        [BsonElement(elementName: "pass")]
        public string pass { get; set; }

        [BsonElement(elementName: "single_grade")]
        public int single_grade { get; set; }

        [BsonElement(elementName: "double_grade")]
        public int double_grade { get; set; }

        [BsonElement(elementName: "name")]
        public string name { get; set; }

        [BsonElement(elementName: "golden_class")]
        public int golden_class { get; set; }

        [BsonElement(elementName: "golden_count")]
        public int golden_count { get; set; }

        [BsonElement(elementName: "golden_id")]
        public int golden_id { get; set; }

        [BsonElement(elementName: "golden_score")]
        public int golden_score { get; set; }

        [BsonElement(elementName: "date_init")]
        public DateTime date_init { get; set; }

        [BsonElement(elementName: "date_last")]
        public DateTime date_last { get; set; }

        [BsonElement(elementName: "playerid")]
        public int playerid { get; set; }

        [BsonElement(elementName: "optionid")]
        public int optionid { get; set; }
        
    }
}
