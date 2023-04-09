using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace eamuse
{
    public class Song
    {
        public int id { get; set; }

        public string title_name { get; set; }

        public string artist { get; set; }

        public int series { get; set; }

        public int bpmmin { get; set; }

        public int bpmmax { get; set; }

        public List<int> difficulty { get; set; }
    }

}
