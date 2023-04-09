using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Xml.Linq;

namespace eamuse
{
    public static class MongoDBConnector
    {
        //public MongoDBConnector()
        //{
        //    LoadCollections();
        //    ///UPDATE GHOSTS
        //    ///GIVE EACH GHOST A UNIQUE ID
        //}
        public static IMongoCollection<Card> cardsCollection { get; set; }
        public static IMongoCollection<Player> playersCollection { get; set; }
        public static IMongoCollection<Score> scoreCollection { get; set; }
        public static IMongoCollection<Ghost> ghostCollection { get; set; }
        public static IMongoCollection<Option> optionsCollection { get; set; }
        public static IMongoDatabase database { get; set; }

        public static void LoadCollections()
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString("mongodb+srv://hansscholt:zKbE12oToAbPfQFu@eamusenet.2tkp4pj.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            MongoClient client = new MongoClient(settings);
            database = client.GetDatabase("ddr");
            cardsCollection = database.GetCollection<Card>("cards");
            playersCollection = database.GetCollection<Player>("players");
            scoreCollection = database.GetCollection<Score>("scores");
            ghostCollection = database.GetCollection<Ghost>("ghosts");
            optionsCollection = database.GetCollection<Option>("options");
        }

        public static async Task<string[][]> GetCardOption(string refid)
        {
            LoadCollections();
            string[][] allOption = new string[4][];
            var filterOption = Builders<Option>.Filter.Eq(o => o.refid, refid);
            var myOption = MongoDBConnector.optionsCollection.Find(filterOption).FirstOrDefault();
            byte[] bData = Convert.FromBase64String(myOption.common);
            string[] decodedString = Encoding.UTF8.GetString(bData).Split(',');
            allOption[0] = decodedString;
            bData = Convert.FromBase64String(myOption.option);
            decodedString = Encoding.UTF8.GetString(bData).Split(',');
            allOption[1] = decodedString;
            bData = Convert.FromBase64String(myOption.last);
            decodedString = Encoding.UTF8.GetString(bData).Split(',');
            allOption[2] = decodedString;
            bData = Convert.FromBase64String(myOption.rival);
            decodedString = Encoding.UTF8.GetString(bData).Split(',');
            allOption[3] = decodedString;

            return allOption;
        }
        public static async Task<List<Card>> GetAllCard(string pcbid)
        {
            LoadCollections();
            var filterCard = Builders<Card>.Filter.Eq(c => c.pcbid, pcbid);
            //return await(await MongoDBConnector.cardsCollection.FindAsync(filterCard)).ToListAsync();
            return await (await MongoDBConnector.cardsCollection.FindAsync(filterCard)).ToListAsync();            
        }

        public static async Task<int> GetUniqueSongCount(string refid)
        {
            LoadCollections();
            var filterRefid = Builders<Score>.Filter.Eq(s => s.refid, refid);
            var listScores = (await (await MongoDBConnector.scoreCollection.FindAsync(filterRefid)).ToListAsync()).Select(x => x.mcode).Distinct().ToList();

            return listScores.Count;
        }

        public static async Task<int> GetTotalPlaysCount(string refid)
        {
            LoadCollections();
            var filterRefid = Builders<Score>.Filter.Eq(s => s.refid, refid);
            var listScores = await (await scoreCollection.FindAsync(filterRefid)).ToListAsync();

            int iTotal = 0;
            foreach (var item in listScores)
            {
                iTotal += item.count;
            }
            return iTotal;
        }

        public static async void UpdateProfile(string refid, Dictionary<string, object> jsonUpdate)
        {
            int icombo = int.Parse(jsonUpdate["combo"].ToString());
            string spass = jsonUpdate["pass"].ToString();
            float fweight = float.Parse(jsonUpdate["weight"].ToString());
            int ifilter = int.Parse(jsonUpdate["filter"].ToString());
            int iguidelines = int.Parse(jsonUpdate["guidelines"].ToString());
            int ifastslow = int.Parse(jsonUpdate["fastslow"].ToString());
            int iarrow = int.Parse(jsonUpdate["arrow"].ToString());
            string sname = jsonUpdate["name"].ToString();
            int icharacter = int.Parse(jsonUpdate["character"].ToString());

            Option option = optionsCollection.Find(Builders<Option>.Filter.Eq(s => s.refid, refid)).FirstOrDefault();

            byte[] bData = Convert.FromBase64String(option.common);
            var optionList = new List<string>(Encoding.UTF8.GetString(bData).Split(','));
            optionList[4] = icharacter.ToString("X");
            optionList[17] = fweight.ToString();
            optionList[25] = sname;
            if (fweight == 0)
                optionList[3] = "0";
            else
                optionList[3] = "1";
            string sCommon = String.Join(",", optionList);
            option.common = Convert.ToBase64String(Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(sCommon)));
           

            bData = Convert.FromBase64String(option.option);
            optionList = new List<string>(Encoding.UTF8.GetString(bData).Split(','));
            optionList[11] = iarrow.ToString();
            optionList[12] = ifilter.ToString();
            optionList[13] = iguidelines.ToString();
            optionList[15] = icombo.ToString();
            optionList[16] = ifastslow.ToString();

            

            string sOption = String.Join(",", optionList);
            option.option = Convert.ToBase64String(Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(sOption)));

            await MongoDBConnector.optionsCollection.ReplaceOneAsync(Builders<Option>.Filter.Eq(s => s.refid, refid), option);

            if (spass != "XXXX")
            {
                Card card = cardsCollection.Find(Builders<Card>.Filter.Eq(s => s.refid, refid)).FirstOrDefault();
                card.pass = spass;
                await MongoDBConnector.cardsCollection.ReplaceOneAsync(Builders<Card>.Filter.Eq(s => s.refid, refid), card);
            }

            
        }
    }
}
