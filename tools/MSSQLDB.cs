using eamuse;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace BlazorCRUDApp.Shared
{
    public static class MSSQLDB
    {
        public static XElement FindMusicBymcode(int mcode)
        {
            var filesPath = Directory.GetCurrentDirectory() + @"\musicdb.xml";
            XDocument xml = XDocument.Load(filesPath);
            XElement mdb = xml.Document.Element("mdb");
            IEnumerable<XElement> music = mdb.Elements("music");
            return music.FirstOrDefault(x => (int)x.Element("mcode") == mcode);

        }

        public static void Player()
        {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "SQL8005.site4now.net";
            builder.UserID = "db_a9734b_dbeamuse_admin";
            builder.Password = "zKbE12oToAbPfQFu";
            builder.InitialCatalog = "db_a9734b_dbeamuse";


            var filesPath = Directory.GetCurrentDirectory() + @"\heven.xml";
            XDocument xml = XDocument.Load(filesPath);
            XElement playerdata = xml.Document.Element("response").Element("playerdata");
            IEnumerable<XElement> music = playerdata.Elements("music");
            int toinsert = 0;
            int toskip = 0;
            foreach (var m in music)
            {
                int imcode = (int)m.Element("mcode");
                var f = FindMusicBymcode(imcode);

                if (f == null)
                {
                    toskip++;
                }
                else
                {
                    toinsert++;

                    var notes = m.Elements("note");
                    int nt = 0;
                    foreach (var n in notes)
                    {
                        var cou = n.Element("count");

                        if (cou != null)
                        {
                            var ra = (int)n.Element("rank");
                            var ck = (int)n.Element("clearkind");
                            var sc = (int)n.Element("score");
                            Ghost gh = new Ghost();
                            gh.ghostsize = 0;
                            gh.ghost = "";
                            gh.mcode = imcode;
                            gh.notetype = nt;

                            int lastinsertedghost = 0;
                            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
                            {
                                conn.Open();
                                using (SqlCommand command = new SqlCommand(
                                    string.Format("insert into ghost (ghost_size ,ghost_data, ghost_mcode, ghost_notetype) " +
                                        "output inserted.ghost_id values({0},'{1}',{2},{3})", gh.ghostsize, gh.ghost, gh.mcode, gh.notetype),
                                        conn))
                                {
                                    try
                                    {
                                        var result = command.ExecuteScalar();
                                        lastinsertedghost = (int)result;
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    finally
                                    {
                                        conn.Close();
                                    }
                                }
                            }

                            Score score = new Score();
                            score.mcode = imcode;
                            score.clearkind = ck;
                            score.count = 1;
                            score.score = sc;
                            score.exscore = 0;
                            score.maxcombo = 0;
                            score.notetype = nt;
                            score.judge_marvelous = 0;
                            score.judge_perfect = 0;
                            score.judge_great = 0;
                            score.judge_good = 0;
                            score.judge_boo = 0;
                            score.judge_miss = 0;
                            score.judge_ok = 0;
                            score.judge_ng = 0;
                            score.fastcount = 0;
                            score.slowcount = 0;
                            score.rank = ra;

                            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
                            {
                                conn.Open();
                                using (SqlCommand command = new SqlCommand(
                                    string.Format("INSERT INTO score (score_mcode ,score_clearkind ,score_count ,score_ghostid ,score_score ,score_exscore, score_maxcombo " +
                                ",score_notetype ,score_marvelous ,score_perfect ,score_great ,score_good ,score_boo ,score_miss ,score_ok ,score_ng ,score_fast " +
                                ",score_slow ,score_cardid, score_date, score_rank) VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},'{19}',{20})",
                                score.mcode, score.clearkind, score.count, lastinsertedghost, score.score, score.exscore, score.maxcombo,
                                score.notetype, score.judge_marvelous, score.judge_perfect, score.judge_great, score.judge_good,
                                score.judge_boo, score.judge_miss, score.judge_ok, score.judge_ng, score.fastcount,
                                score.slowcount, 3/* cambiar*/, DateTime.Now, score.rank),
                                conn))
                                {
                                    try
                                    {
                                        var result = command.ExecuteNonQuery();

                                        if (result > 0)
                                        {
                                            //return true;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw;
                                    }
                                    finally
                                    {
                                        conn.Close();
                                    }
                                }
                            }
                        }
                        nt++;
                    }
                }

            }
            toskip = toskip;
            //MongoDBConnector.LoadCollections();

            //Option option = new Option();

            //using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            //{
            //    conn.Open();
            //    using (SqlCommand command = new SqlCommand(
            //        string.Format("insert into [option] (option_common, option_option, option_last, option_rival) " +
            //        "OUTPUT inserted.option_id values('{0}','{1}','{2}','{3}')", option.common, option.option, option.last, option.rival),
            //        conn))
            //    {

            //        var result = command.ExecuteScalar();


            //        var vv = (int)result;
            //        if ("b" == "a")
            //        {
            //            //return true;
            //        }
            //        conn.Close();
            //    }
            //}




            //List<Player> players = MongoDBConnector.playersCollection.FindAsync(Builders<Player>.Filter.Empty).Result.ToList();
            //foreach (var item in players)
            //{
            //    using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            //    {
            //        conn.Open();
            //        using (SqlCommand command = new SqlCommand(
            //            string.Format("insert into player (player_pcbid, player_name, player_area) values ('{0}', '{1}', {2})", item.pcbid, item.name, 1), conn))
            //        {

            //            var result = command.ExecuteNonQuery();

            //            if (result > 0)
            //            {
            //                //return true;
            //            }
            //            conn.Close();
            //        }
            //    }
            //}

            //List<Card> card = MongoDBConnector.cardsCollection.FindAsync(Builders<Card>.Filter.Empty).Result.ToList();
            //int optionid = 1;
            //foreach (var item in card)
            //{

            //    int playerid = 1;
            //    if (item.pcbid == "018CD3A927F4963524B7")
            //    {
            //        playerid = 1;
            //    }
            //    else if (item.pcbid == "8B065107D051980ACE3B")
            //    {
            //        playerid = 2;
            //    }
            //    using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            //    {
            //        conn.Open();
            //        using (SqlCommand command = new SqlCommand(
            //            string.Format("INSERT INTO card (card_refid ,card_cardid ,card_code ,card_name ,card_pin ,card_leaguesingle ," +
            //            "card_leaguedouble ,card_leagueclass ,card_leaguecount ,card_leaguescore ,card_dateinit ,card_dateend ," +
            //            "card_playerid ,card_optionid) VALUES ('{0}', '{1}', {2}, '{3}','{4}', {5},{6},{7},{8},{9}, '{10}','{11}', {12}, {13})",
            //            item.refid, item.cardid, item.codeint, item.name, item.pass, item.single_grade,
            //            item.double_grade, item.golden_class, item.golden_count, item.golden_score, item.date_init, item.date_last,
            //            playerid, optionid),
            //            conn))
            //        {

            //            var result = command.ExecuteNonQuery();

            //            if (result > 0)
            //            {
            //                //return true;
            //            }
            //            conn.Close();
            //        }
            //    }
            //    optionid++;
            //}

            //List<Option> option = MongoDBConnector.optionsCollection.FindAsync(Builders<Option>.Filter.Empty).Result.ToList();
            //int optionid = 1;
            //foreach (var item in option)
            //{
            //    using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            //    {
            //        conn.Open();
            //        using (SqlCommand command = new SqlCommand(
            //            string.Format("INSERT INTO [option] (option_common, option_option, option_last, option_rival) values ('{0}','{1}','{2}','{3}')",
            //            item.common, item.option, item.last, item.rival),
            //            conn))
            //        {

            //            var result = command.ExecuteNonQuery();

            //            if (result > 0)
            //            {
            //                //return true;
            //            }
            //            conn.Close();
            //        }
            //    }
            //    optionid++;
            //}

            //List<Ghost> ghost = MongoDBConnector.ghostCollection.FindAsync(Builders<Ghost>.Filter.Empty).Result.ToList();
            //foreach (var item in ghost)
            //{
            //    using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            //    {
            //        conn.Open();
            //        using (SqlCommand command = new SqlCommand(
            //            string.Format("INSERT INTO ghost (ghost_size ,ghost_data, ghost_mcode, ghost_notetype) VALUES ({0},'{1}',{2},{3})",
            //            item.ghostsize, item.ghost, item.mcode, item.notetype),
            //            conn))
            //        {

            //            var result = command.ExecuteNonQuery();

            //            if (result > 0)
            //            {
            //                //return true;
            //            }
            //            conn.Close();
            //        }
            //    }
            //}

            //List<Score> score = MongoDBConnector.scoreCollection.FindAsync(Builders<Score>.Filter.Empty).Result.ToList();
            //int ghostid = 1;
            //foreach (var item in score)
            //{
            //    int playerid = 1;
            //    if (item.refid == "0FB825C61330733C")
            //    {
            //        playerid = 1;
            //    }
            //    else if (item.refid == "282DB04B3D00C88E")
            //    {
            //        playerid = 2;
            //    }
            //    else if (item.refid == "B677319C8C7DED43")
            //    {
            //        playerid = 3;
            //    }
            //    using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            //    {
            //        conn.Open();
            //        using (SqlCommand command = new SqlCommand(
            //            string.Format("INSERT INTO [dbo].[score] ([score_mcode] ,[score_clearkind] ,[score_count] ,[score_ghostid] ,[score_score] " +
            //            ",[score_exscore] ,[score_maxcombo] ,[score_notetype] ,[score_marvelous] ,[score_perfect] ,[score_great] ,[score_good] ,[score_boo] " +
            //            ",[score_miss] ,[score_ok] ,[score_ng] ,[score_fast] ,[score_slow] ,[score_cardid]) " +
            //            "VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18})",
            //            item.mcode, item.clearkind, 1, ghostid, item.score,
            //            item.exscore, item.maxcombo, item.notetype, item.judge_marvelous, item.judge_perfect, item.judge_great, item.judge_good, item.judge_boo,
            //            item.judge_miss, item.judge_ok, item.judge_ng, item.fastcount, item.slowcount, playerid),
            //            conn))
            //        {

            //            var result = command.ExecuteNonQuery();

            //            if (result > 0)
            //            {
            //                //return true;
            //            }
            //            conn.Close();
            //        }
            //    }
            //    ghostid++;
            //}
        }

    }
}
