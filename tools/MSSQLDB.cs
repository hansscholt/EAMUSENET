using eamuse;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCRUDApp.Shared
{
    public static class MSSQLDB
    {
        public static void Player()
        {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "SQL8005.site4now.net";
            builder.UserID = "db_a9734b_dbeamuse_admin";
            builder.Password = "zKbE12oToAbPfQFu";
            builder.InitialCatalog = "db_a9734b_dbeamuse";

            MongoDBConnector.LoadCollections();

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

            List<Card> card = MongoDBConnector.cardsCollection.FindAsync(Builders<Card>.Filter.Empty).Result.ToList();
            int optionid = 1;
            foreach (var item in card)
            {

                int playerid = 1;
                if (item.pcbid == "018CD3A927F4963524B7")
                {
                    playerid = 1;
                }
                else if (item.pcbid == "8B065107D051980ACE3B")
                {
                    playerid = 2;
                }
                using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(
                        string.Format("INSERT INTO card (card_refid ,card_cardid ,card_code ,card_name ,card_pin ,card_leaguesingle ," +
                        "card_leaguedouble ,card_leagueclass ,card_leaguecount ,card_leaguescore ,card_dateinit ,card_dateend ," +
                        "card_playerid ,card_optionid) VALUES ('{0}', '{1}', {2}, '{3}','{4}', {5},{6},{7},{8},{9}, '{10}','{11}', {12}, {13})",
                        item.refid, item.cardid, item.codeint, item.name, item.pass, item.single_grade,
                        item.double_grade, item.golden_class, item.golden_count, item.golden_score, item.date_init, item.date_last,
                        playerid, optionid),
                        conn))
                    {

                        var result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            //return true;
                        }
                        conn.Close();
                    }
                }
                optionid++;
            }

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
