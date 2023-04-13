using BlazorCRUDApp.Server.Models;
using BlazorCRUDApp.Server.Services;
using BlazorCRUDApp.Shared;
using eamuse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace BlazorCRUDApp.Server.Controllers
{
    [Route("webui/api/[controller]")]
    [ApiController]
    public class ScoringController : ControllerBase
    {
        [HttpGet("GetAllScore")]
        public List<Score> GetAllScore()
        {
            var listScore = MSSQLConnection.GetScores(false);
            for (int i = listScore.Count - 1; i >= 0; i--)
            {
                XElement element = Util.FindMusicBymcode(listScore[i].mcode);
                if (element != null)//we are removing here the dan courses
                {
                    listScore[i].songtitle = element.Element("title").Value;
                    listScore[i].artist = element.Element("artist").Value;
                    listScore[i].series = (int)element.Element("series");
                    listScore[i].difficultynumber = int.Parse(element.Element("diffLv").Value.Split(" ")[listScore[i].notetype].ToString());
                }
                else
                    listScore.RemoveAt(i);
            }
            return listScore;
        }

        [HttpGet("GetMaxScore")]
        public List<Score> GetMaxScore()
        {
            var listScore = MSSQLConnection.GetScores(true);
            for (int i = listScore.Count - 1; i >= 0; i--)
            {
                XElement element = Util.FindMusicBymcode(listScore[i].mcode);
                if (element != null)//we are removing here the dan courses
                {
                    listScore[i].songtitle = element.Element("title").Value;
                    listScore[i].artist = element.Element("artist").Value;
                    listScore[i].series = (int)element.Element("series");
                    listScore[i].difficultynumber = int.Parse(element.Element("diffLv").Value.Split(" ")[listScore[i].notetype].ToString());
                }
                else
                    listScore.RemoveAt(i);
            }
            return listScore;
        }
    }
}
