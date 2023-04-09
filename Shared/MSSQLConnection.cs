using eamuse;
using eamuse.Models;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCRUDApp.Shared
{
    public class MSSQLConnection
    {
        public static string ConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "SQL8005.site4now.net";
            builder.UserID = "db_a9734b_dbeamuse_admin";
            builder.Password = "zKbE12oToAbPfQFu";
            builder.InitialCatalog = "db_a9734b_dbeamuse";
            return builder.ConnectionString;
        }

        public static Option GetOption(string refid)
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();
                string query = string.Format("select top 1 [option].* from [option] " +
                    "inner join [card] on [card].card_optionid = [option].option_id where [card].card_refid = '{0}'", refid);

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            Option o = new Option();
                            result.Read();
                            o.id = (int)result["option_id"];
                            o.common = result["option_common"].ToString();
                            o.option = result["option_option"].ToString();
                            o.last = result["option_last"].ToString();
                            o.rival = result["option_rival"].ToString();

                            return o;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }

        public static string[][] GetOptionAsArray(string refid)
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();
                string query = string.Format("select top 1 [option].* from [option] " +
                    "inner join [card] on [card].card_optionid = [option].option_id where [card].card_refid = '{0}'", refid);

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            result.Read();

                            Option o = new Option();
                            o.id = (int)result["option_id"];
                            o.common = result["option_common"].ToString();
                            o.option = result["option_option"].ToString();
                            o.last = result["option_last"].ToString();
                            o.rival = result["option_rival"].ToString();

                            string[][] allOption = new string[4][];
                            byte[] bData = Convert.FromBase64String(o.common);
                            string[] decodedString = Encoding.UTF8.GetString(bData).Split(',');
                            allOption[0] = decodedString;
                            bData = Convert.FromBase64String(o.option);
                            decodedString = Encoding.UTF8.GetString(bData).Split(',');
                            allOption[1] = decodedString;
                            bData = Convert.FromBase64String(o.last);
                            decodedString = Encoding.UTF8.GetString(bData).Split(',');
                            allOption[2] = decodedString;
                            bData = Convert.FromBase64String(o.rival);
                            decodedString = Encoding.UTF8.GetString(bData).Split(',');
                            allOption[3] = decodedString;
                            return allOption;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }

        public static Card GetCardByRefID(string refid)
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();
                string query = string.Format("select top 1 [card].* from [card] " +
                    "where card_refid = '{0}'", refid);

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            Card c = new Card();
                            result.Read();
                            c.id = (int)result["card_id"];
                            c.refid = result["card_refid"].ToString();
                            c.cardid = result["card_cardid"].ToString();
                            //c.area = result["card_area"].ToString();
                            c.code = result["card_code"].ToString();
                            c.name = result["card_name"].ToString();
                            c.pass = result["card_pin"].ToString();
                            c.single_grade = (int)result["card_leaguesingle"];
                            c.double_grade = (int)result["card_leaguedouble"];
                            c.golden_class = (int)result["card_leagueclass"];
                            c.golden_count = (int)result["card_leaguecount"];
                            c.golden_score = (int)result["card_leaguescore"];
                            c.date_init = (DateTime)result["card_dateinit"];
                            c.date_last = (DateTime)result["card_dateend"];
                            c.playerid = (int)result["card_playerid"];
                            c.optionid = (int)result["card_optionid"];

                            return c;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }

        public static int[] GetSongCounts(string refid)
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();
                string query = string.Format("select count(distinct concat(score_mcode, score_notetype)) as 'total' from score " +
                    "inner join [card] on [card].card_id = score.score_cardid where [card].card_refid = '{0}' " +
                    "union all " +
                    "select count(concat(score_mcode, score_notetype)) from score " +
                    "inner join [card] on [card].card_id = score.score_cardid where [card].card_refid = '{0}'", refid);

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            List<int> countlist = new List<int>(); 
                            while (result.Read())                            
                                countlist.Add((int)result["total"]);
                            return countlist.ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }

        public static void UpdateProfile(string refid, Dictionary<string, object> jsonUpdate)
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

            Option option = GetOption(refid);
            //Option option = optionsCollection.Find(Builders<Option>.Filter.Eq(s => s.refid, refid)).FirstOrDefault();
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

            UpdateOption(option);
            //await MongoDBConnector.optionsCollection.ReplaceOneAsync(Builders<Option>.Filter.Eq(s => s.refid, refid), option);

            
            
            //Card card = cardsCollection.Find(Builders<Card>.Filter.Eq(s => s.refid, refid)).FirstOrDefault();
            Card card = GetCardByRefID(refid);
            if (spass != "XXXX")
                card.pass = spass;
            card.name = sname;
            UpdateCard(card);
            //await MongoDBConnector.cardsCollection.ReplaceOneAsync(Builders<Card>.Filter.Eq(s => s.refid, refid), card);
            
        }

        public static List<Card> GetCardByPCBID(string pcbid)
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();
                string query = string.Format("select [card].* from [card] inner join player on player.player_id = [card].card_playerid " +
                    "where player.player_pcbid = '{0}'", pcbid);

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            List<Card> cards = new List<Card>();
                            while (result.Read())
                            {
                                Card c = new Card()
                                {
                                    id = (int)result["card_id"],
                                    refid = result["card_refid"].ToString(),
                                    cardid = result["card_cardid"].ToString(),
                                    //c.area = result["card_area"].ToString();
                                    code = result["card_code"].ToString(),
                                    name = result["card_name"].ToString(),
                                    pass = result["card_pin"].ToString(),
                                    single_grade = (int)result["card_leaguesingle"],
                                    double_grade = (int)result["card_leaguedouble"],
                                    golden_class = (int)result["card_leagueclass"],
                                    golden_count = (int)result["card_leaguecount"],
                                    golden_score = (int)result["card_leaguescore"],
                                    date_init = (DateTime)result["card_dateinit"],
                                    date_last = (DateTime)result["card_dateend"],
                                    playerid = (int)result["card_playerid"],
                                    optionid = (int)result["card_optionid"]
                                };
                                cards.Add(c);
                            }


                            return cards;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }

        public static Card GetCardByCode(int code)
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();
                string query = string.Format("select top 1 [card].* from [card] " +
                    "where card_code = {0}", code);

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            Card c = new Card();
                            result.Read();
                            c.id = (int)result["card_id"];
                            c.refid = result["card_refid"].ToString();
                            c.cardid = result["card_cardid"].ToString();
                            //c.area = result["card_area"].ToString();
                            c.code = result["card_code"].ToString();
                            c.name = result["card_name"].ToString();
                            c.pass = result["card_pin"].ToString();
                            c.single_grade = (int)result["card_leaguesingle"];
                            c.double_grade = (int)result["card_leaguedouble"];
                            c.golden_class = (int)result["card_leagueclass"];
                            c.golden_count = (int)result["card_leaguecount"];
                            c.golden_score = (int)result["card_leaguescore"];
                            c.date_init = (DateTime)result["card_dateinit"];
                            c.date_last = (DateTime)result["card_dateend"];
                            c.playerid = (int)result["card_playerid"];
                            c.optionid = (int)result["card_optionid"];

                            return c;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }

        public static Card GetCard(string cardid, string pcbid) 
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {                
                conn.Open();
                string query = string.Format("select top 1 [card].* from [card] " +
                    "inner join player on player.player_id = [card].card_playerid " +
                    "where [card].card_cardid = '{0}' and player.player_pcbid = '{1}'", cardid, pcbid);

                if (string.IsNullOrEmpty(pcbid))                
                    query = string.Format("select top 1 [card].* from [card] where card.card_cardid = '{0}'", cardid);
                

                using (SqlCommand command = new SqlCommand(query, conn))
                {                    
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            Card c = new Card();
                            result.Read();                            
                            c.id = (int)result["card_id"];
                            c.refid = result["card_refid"].ToString();
                            c.cardid = result["card_cardid"].ToString();
                            //c.area = result["card_area"].ToString();
                            c.code = result["card_code"].ToString();
                            c.name = result["card_name"].ToString();
                            c.pass = result["card_pin"].ToString();
                            c.single_grade = (int)result["card_leaguesingle"];
                            c.double_grade = (int)result["card_leaguedouble"];
                            c.golden_class = (int)result["card_leagueclass"];
                            c.golden_count = (int)result["card_leaguecount"];
                            c.golden_score = (int)result["card_leaguescore"];
                            c.date_init = (DateTime)result["card_dateinit"];
                            c.date_last = (DateTime)result["card_dateend"];
                            c.playerid = (int)result["card_playerid"];
                            c.optionid = (int)result["card_optionid"];

                            return c;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();
                        
                    }
                    return null;
                }
            }
        }
        public static Player GetPlayer(string pcbid)
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(string.Format("select * from player where player_pcbid = '{0}'", pcbid), conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            Player p = new Player();
                            result.Read();
                            p.id = (int)result["player_id"];
                            p.name = result["player_name"].ToString();
                            p.pcbid = result["player_pcbid"].ToString();
                            return p;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }

       
        public static Ghost GetGhost(int ghostid)
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(string.Format("select * from ghost where ghost_id = {0}", ghostid), conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            result.Read();
                            Ghost g = new Ghost();                           
                            g.id = (int)result["ghost_id"];
                            g.ghostsize = (int)result["ghost_size"];
                            g.ghost = result["ghost_data"].ToString();
                            g.mcode = (int)result["ghost_mcode"];
                            g.notetype = (int)result["ghost_notetype"];
                            return g;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }
        public static List<Score> GetMaxScores()
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(string.Format("select * " +
                    "from( select score_id, score_mcode, score_clearkind, score_count, score_ghostid, score_score, score_exscore, score_maxcombo, score_notetype, " +
                    "score_marvelous, score_perfect, score_great, score_good, score_boo, score_miss, score_ok, score_ng, score_fast, score_slow, [card].card_name, score_date, ROW_NUMBER() " +
                    "OVER(PARTITION BY score_mcode, score_notetype ORDER BY score_score desc) as rn " +
                    "from score inner join [card] on [card].card_id = score.score_cardid ) as a " +
                    "where rn = 1"), conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            List<Score> scores = new List<Score>();
                            while (result.Read())
                            {
                                Score s = new Score()
                                {
                                    id = (int)result["score_id"],
                                    mcode = (int)result["score_mcode"],
                                    clearkind = (int)result["score_clearkind"],
                                    count = (int)result["score_count"],
                                    ghostid = (int)result["score_ghostid"],
                                    score = (int)result["score_score"],
                                    exscore = (int)result["score_exscore"],
                                    maxcombo = (int)result["score_maxcombo"],
                                    notetype = (int)result["score_notetype"],
                                    judge_marvelous = (int)result["score_marvelous"],
                                    judge_perfect = (int)result["score_perfect"],
                                    judge_great = (int)result["score_great"],
                                    judge_good = (int)result["score_good"],
                                    judge_boo = (int)result["score_boo"],
                                    judge_miss = (int)result["score_miss"],
                                    judge_ok = (int)result["score_ok"],
                                    judge_ng = (int)result["score_ng"],
                                    fastcount = (int)result["score_fast"],
                                    slowcount = (int)result["score_slow"],
                                    date = (DateTime)result["score_date"],
                                    name = result["card_name"].ToString()
                                };
                                scores.Add(s); 
                            }
                            return scores;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }
        public static List<Score> GetMaxScoresByArea(int area)
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(string.Format("select * " +
                    "from( select score_id, score_mcode, score_clearkind, score_count, score_ghostid, score_score, score_exscore, score_maxcombo, score_notetype, " +
                    "score_marvelous, score_perfect, score_great, score_good, score_boo, score_miss, score_ok, score_ng, score_fast, score_slow, [card].card_name, score_date, ROW_NUMBER() " +
                    "OVER(PARTITION BY score_mcode, score_notetype ORDER BY score_score desc) as rn " +
                    "from score inner join [card] on [card].card_id = score.score_cardid inner join player on player.player_id = [card].card_playerid where player.player_area = {0}) as a " +
                    "where rn = 1", area), conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            List<Score> scores = new List<Score>();
                            while (result.Read())
                            {
                                Score s = new Score()
                                {
                                    id = (int)result["score_id"],
                                    mcode = (int)result["score_mcode"],
                                    clearkind = (int)result["score_clearkind"],
                                    count = (int)result["score_count"],
                                    ghostid = (int)result["score_ghostid"],
                                    score = (int)result["score_score"],
                                    exscore = (int)result["score_exscore"],
                                    maxcombo = (int)result["score_maxcombo"],
                                    notetype = (int)result["score_notetype"],
                                    judge_marvelous = (int)result["score_marvelous"],
                                    judge_perfect = (int)result["score_perfect"],
                                    judge_great = (int)result["score_great"],
                                    judge_good = (int)result["score_good"],
                                    judge_boo = (int)result["score_boo"],
                                    judge_miss = (int)result["score_miss"],
                                    judge_ok = (int)result["score_ok"],
                                    judge_ng = (int)result["score_ng"],
                                    fastcount = (int)result["score_fast"],
                                    slowcount = (int)result["score_slow"],
                                    date = (DateTime)result["score_date"],
                                    name = result["card_name"].ToString()
                                };
                                scores.Add(s);
                            }
                            return scores;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }

        public static List<Score> GetMaxScoresByCode(int code)
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(string.Format("select * " +
                    "from( select score_id, score_mcode, score_clearkind, score_count, score_ghostid, score_score, score_exscore, score_maxcombo, score_notetype, " +
                    "score_marvelous, score_perfect, score_great, score_good, score_boo, score_miss, score_ok, score_ng, score_fast, score_slow, [card].card_name, score_date, ROW_NUMBER() " +
                    "OVER(PARTITION BY score_mcode, score_notetype ORDER BY score_score desc) as rn " +
                    "from score inner join [card] on [card].card_id = score.score_cardid where [card].card_code = {0}) as a " +
                    "where rn = 1", code), conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            List<Score> scores = new List<Score>();
                            while (result.Read())
                            {
                                Score s = new Score()
                                {
                                    id = (int)result["score_id"],
                                    mcode = (int)result["score_mcode"],
                                    clearkind = (int)result["score_clearkind"],
                                    count = (int)result["score_count"],
                                    ghostid = (int)result["score_ghostid"],
                                    score = (int)result["score_score"],
                                    exscore = (int)result["score_exscore"],
                                    maxcombo = (int)result["score_maxcombo"],
                                    notetype = (int)result["score_notetype"],
                                    judge_marvelous = (int)result["score_marvelous"],
                                    judge_perfect = (int)result["score_perfect"],
                                    judge_great = (int)result["score_great"],
                                    judge_good = (int)result["score_good"],
                                    judge_boo = (int)result["score_boo"],
                                    judge_miss = (int)result["score_miss"],
                                    judge_ok = (int)result["score_ok"],
                                    judge_ng = (int)result["score_ng"],
                                    fastcount = (int)result["score_fast"],
                                    slowcount = (int)result["score_slow"],
                                    date = (DateTime)result["score_date"],
                                    name = result["card_name"].ToString()
                                };
                                scores.Add(s);
                            }
                            return scores;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }

        public static List<Score> GetMaxScoresByPCBID(string pcbid)
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(string.Format("select * " +
                    "from( select score_id, score_mcode, score_clearkind, score_count, score_ghostid, score_score, score_exscore, score_maxcombo, score_notetype, " +
                    "score_marvelous, score_perfect, score_great, score_good, score_boo, score_miss, score_ok, score_ng, score_fast, score_slow, [card].card_name, score_date, ROW_NUMBER() " +
                    "OVER(PARTITION BY score_mcode, score_notetype ORDER BY score_score desc) as rn " +
                    "from score inner join [card] on [card].card_id = score.score_cardid inner join player on player.player_id = [card].card_playerid where player.player_pcbid = '{0}') as a " +
                    "where rn = 1", pcbid), conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            List<Score> scores = new List<Score>();
                            while (result.Read())
                            {
                                Score s = new Score()
                                {
                                    id = (int)result["score_id"],
                                    mcode = (int)result["score_mcode"],
                                    clearkind = (int)result["score_clearkind"],
                                    count = (int)result["score_count"],
                                    ghostid = (int)result["score_ghostid"],
                                    score = (int)result["score_score"],
                                    exscore = (int)result["score_exscore"],
                                    maxcombo = (int)result["score_maxcombo"],
                                    notetype = (int)result["score_notetype"],
                                    judge_marvelous = (int)result["score_marvelous"],
                                    judge_perfect = (int)result["score_perfect"],
                                    judge_great = (int)result["score_great"],
                                    judge_good = (int)result["score_good"],
                                    judge_boo = (int)result["score_boo"],
                                    judge_miss = (int)result["score_miss"],
                                    judge_ok = (int)result["score_ok"],
                                    judge_ng = (int)result["score_ng"],
                                    fastcount = (int)result["score_fast"],
                                    slowcount = (int)result["score_slow"],
                                    date = (DateTime)result["score_date"],
                                    name = result["card_name"].ToString()
                                };
                                scores.Add(s);
                            }
                            return scores;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }
        

        public static List<Score> GetMaxScoresByRefID(string refid)
        {
            string connsString = ConnectionString();
            using (SqlConnection conn = new SqlConnection(connsString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(string.Format("select * " +
                    "from( select score_id, score_mcode, score_clearkind, score_count, score_ghostid, score_score, score_exscore, score_maxcombo, score_notetype, " +
                    "score_marvelous, score_perfect, score_great, score_good, score_boo, score_miss, score_ok, score_ng, score_fast, score_slow, [card].card_name, score_date, ROW_NUMBER() " +
                    "OVER(PARTITION BY score_mcode, score_notetype ORDER BY score_score desc) as rn " +
                    "from score inner join [card] on [card].card_id = score.score_cardid where [card].card_refid = '{0}') as a " +
                    "where rn = 1", refid), conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            List<Score> scores = new List<Score>();
                            while (result.Read())
                            {
                                Score s = new Score() 
                                {
                                    id = (int)result["score_id"],
                                    mcode = (int)result["score_mcode"],
                                    clearkind = (int)result["score_clearkind"],
                                    count = (int)result["score_count"],
                                    ghostid = (int)result["score_ghostid"],
                                    score = (int)result["score_score"],
                                    exscore = (int)result["score_exscore"],
                                    maxcombo = (int)result["score_maxcombo"],
                                    notetype = (int)result["score_notetype"],
                                    judge_marvelous = (int)result["score_marvelous"],
                                    judge_perfect = (int)result["score_perfect"],
                                    judge_great = (int)result["score_great"],
                                    judge_good = (int)result["score_good"],
                                    judge_boo = (int)result["score_boo"],
                                    judge_miss = (int)result["score_miss"],
                                    judge_ok = (int)result["score_ok"],
                                    judge_ng = (int)result["score_ng"],
                                    fastcount = (int)result["score_fast"],
                                    slowcount = (int)result["score_slow"],
                                    date = (DateTime)result["score_date"],
                                    name = result["card_name"].ToString()
                                };
                                scores.Add(s);
                            }
                            return scores;

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();

                    }
                    return null;
                }
            }
        }

        public static int InsertOption(Option option)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(
                    string.Format("insert into [option] (option_common, option_option, option_last, option_rival) " +
                    "OUTPUT inserted.option_id values('{0}','{1}','{2}','{3}')", option.common, option.option, option.last, option.rival),
                    conn))
                {
                    try
                    {
                        var result = command.ExecuteScalar();
                        return (int)result;
                    }
                    catch (Exception ex)
                    {
                    }
                    finally 
                    {
                        conn.Close();
                    }
                    return 0;
                }
            }
        }

        public static int InsertGhost(Ghost ghost)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(
                    string.Format("insert into ghost (ghost_size ,ghost_data, ghost_mcode, ghost_notetype) " +
                    "OUTPUT inserted.ghost_id values({0},'{1}',{2},{3})", ghost.ghostsize, ghost.ghost, ghost.mcode, ghost.notetype),
                    conn))
                {
                    try
                    {
                        var result = command.ExecuteScalar();
                        return (int)result;
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        conn.Close();
                    }
                    return 0;
                }
            }
        }

        public static void InsertScore(Score score)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using ( SqlCommand command = new SqlCommand(
                    string.Format("INSERT INTO score (score_mcode ,score_clearkind ,score_count ,score_ghostid ,score_score ,score_exscore, score_maxcombo " +
                    ",score_notetype ,score_marvelous ,score_perfect ,score_great ,score_good ,score_boo ,score_miss ,score_ok ,score_ng ,score_fast " +
                    ",score_slow ,score_cardid, score_date) VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},'{19}')",
                    score.mcode, score.clearkind, score.count, score.ghostid, score.score, score.exscore, score.maxcombo, 
                    score.notetype, score.judge_marvelous, score.judge_perfect, score.judge_great, score.judge_good, score.judge_boo, score.judge_miss, score.judge_ok, score.judge_ng, score.fastcount,
                    score.slowcount, score.cardid, score.date),
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
        public static void UpdateCard(Card card)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(
                    string.Format("UPDATE [card] set card_refid = '{0}', card_cardid = '{1}',card_code = {2} ,card_name = '{3}' ,card_pin = '{4}', " +
                    "card_leaguesingle = {5},card_leaguedouble = {6}, card_leagueclass = {7} ,card_leaguecount = {8}, card_leaguescore = {9}, " +
                    "card_dateinit = '{10}',card_dateend = '{11}',card_playerid = {12},card_optionid = {13} where card_id = {14} ",
                    card.refid, card.cardid, card.code, card.name, card.pass, card.single_grade, card.double_grade,
                    card.golden_class, card.golden_count, card.golden_score, card.date_init, card.date_last, card.playerid, card.optionid, card.id),
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

        public static void UpdateOption(Option option)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(
                    string.Format("update [option] set option_common = '{0}', option_option = '{1}', " +
                    "option_last = '{2}', option_rival = '{3}' where option_id = {4}",
                    option.common, option.option, option.last, option.rival, option.id),
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

        public static void InsertCard(Card card)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(
                    string.Format("INSERT INTO [card] (card_refid ,card_cardid ,card_code ,card_name ,card_pin ,card_leaguesingle , " +
                    "card_leaguedouble ,card_leagueclass ,card_leaguecount ,card_leaguescore ,card_dateinit ,card_dateend , " +
                    "card_playerid ,card_optionid) VALUES ('{0}', '{1}', {2}, '{3}','{4}', {5},{6},{7},{8},{9}, '{10}','{11}', {12}, {13})",
                    card.refid, card.cardid, card.codeint, card.name, card.pass, card.single_grade,
                    card.double_grade, card.golden_class, card.golden_count, card.golden_score, card.date_init, card.date_last,
                    card.playerid, card.optionid),
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
    }
}
