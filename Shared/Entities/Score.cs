using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace eamuse.Models
{
    [BsonIgnoreExtraElements]
    public class Score
    {
        [BsonId]
        [BsonElement(elementName: "_id")]
        public ObjectId _id { get; set; }

        [BsonId]
        [BsonElement(elementName: "id")]
        public int id { get; set; }

        [BsonElement(elementName: "mcode")]//songid
        public int mcode { get; set; }

        [BsonElement(elementName: "clearkind")]
        public int clearkind { get; set; }

        [BsonElement(elementName: "count")]
        public int count { get; set; }

        [BsonElement(elementName: "ghostid")]
        public int ghostid { get; set; }

        [BsonElement(elementName: "rank")]
        public int rank { get; set; }

        [BsonElement(elementName: "score")]
        public int score { get; set; }

        [BsonElement(elementName: "exscore")]
        public int exscore { get; set; }

        [BsonElement(elementName: "maxcombo")]
        public int maxcombo { get; set; }

        [BsonElement(elementName: "notetype")]
        public int notetype { get; set; }

        //[BsonElement(elementName: "player")]
        //public ObjectId player { get; set; }

        [BsonElement(elementName: "judge_marvelous")]
        public int judge_marvelous { get; set; }

        [BsonElement(elementName: "judge_perfect")]
        public int judge_perfect { get; set; }

        [BsonElement(elementName: "judge_great")]
        public int judge_great { get; set; }

        [BsonElement(elementName: "judge_good")]
        public int judge_good { get; set; }

        [BsonElement(elementName: "judge_boo")]
        public int judge_boo { get; set; }

        [BsonElement(elementName: "judge_miss")]
        public int judge_miss { get; set; }

        [BsonElement(elementName: "judge_ok")]
        public int judge_ok { get; set; }

        [BsonElement(elementName: "judge_ng")]
        public int judge_ng { get; set; }

        [BsonElement(elementName: "fastcount")]
        public int fastcount { get; set; }

        [BsonElement(elementName: "slowcount")]
        public int slowcount { get; set; }

        [BsonElement(elementName: "area")]
        public int area { get; set; }

        [BsonElement(elementName: "code")]
        public int code { get; set; }

        [BsonElement(elementName: "name")]
        public string name { get; set; }

        [BsonElement(elementName: "pcbid")]
        public string pcbid { get; set; }

        [BsonElement(elementName: "refid")]
        public string refid { get; set; }

        [BsonElement(elementName: "shoparea")]
        public string shoparea { get; set; }


        [BsonElement(elementName: "scoreid")]
        public int scoreid { get; set; }

        [BsonElement(elementName: "cardid")]
        public int cardid { get; set; }

    }

}
