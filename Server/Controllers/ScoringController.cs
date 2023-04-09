using BlazorCRUDApp.Server.Models;
using BlazorCRUDApp.Server.Services;
using BlazorCRUDApp.Shared;
using eamuse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorCRUDApp.Server.Controllers
{
    [Route("webui/api/[controller]")]
    [ApiController]
    public class ScoringController : ControllerBase
    {
        [HttpGet("GetAllScore")]
        public List<Score> GetAllScore()
        {
            return MSSQLConnection.GetMaxScores();
            //return await MongoDBConnector.GetAllCard(pcbid);
        }
    }
}
