//using BlazorCRUDApp.Server.Models;
using BlazorCRUDApp.Shared;
using eamuse;
using eamuse.KBin;
using eamuse.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MudBlazor.Charts;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
//using System.Text.Json;

namespace eamusenet.Server.Controllers
{
    [Route("webui/api/[controller]")]
    [ApiController]
    public class OptionController : Controller
    {
        // GET: OptionController
        //[HttpGet]
        //public async Task<string[][]> Get(string refid)
        //{
        //    return MongoDBConnector.GetAllOption(refid);
        //}

        [HttpGet("GetAllCard/{pcbid}")]
        public List<Card> GetCard(string pcbid)
        {
            return MSSQLConnection.GetCardByPCBID(pcbid);
            //return await MongoDBConnector.GetAllCard(pcbid);
        }

        [HttpGet("GetCardOption/{refid}")]
        public async Task<string[][]> GetCardOption(string refid)
        {
            return MSSQLConnection.GetOptionAsArray(refid);
            //return await MongoDBConnector.GetCardOption(refid);
        }

        //[HttpGet("GetAllOption/{refid}")]
        //public string[][] GetOption(string refid)
        //{
        //    return MongoDBConnector.GetAllOption(refid);
        //}
        [HttpGet("GetSongCounts/{refid}")]
        public async Task<int[]> GetSongCounts(string refid)
        {
            return MSSQLConnection.GetSongCounts(refid);
            //return await MongoDBConnector.GetUniqueSongCount(refid);
        }

        [HttpGet("GetCardByCode/{ddrcode}")]
        public async Task<Card> GetCardByCode(string ddrcode)
        {            
            Card c = MSSQLConnection.GetCardByCode(int.Parse(ddrcode));
            if (c == null)            
                Response.StatusCode = 204;            
            else
                Response.StatusCode = 200;
            return c;
            //return await MongoDBConnector.GetUniqueSongCount(refid);
        }

        //[HttpGet("GetUniqueSongCount/{refid}")]
        //public async Task<int> GetUniqueSongCount(string refid)
        //{
        //    return MSSQLConnection.GetUniqueSongCount();
        //    return await MongoDBConnector.GetUniqueSongCount(refid);
        //}
        //[HttpGet("GetTotalPlaysCount/{refid}")]
        //public async Task<int> GetTotalPlaysCount(string refid)
        //{
        //    return await MongoDBConnector.GetTotalPlaysCount(refid);
        //}

        [HttpPut("UpdateProfile/{refid}")]
        public async void UpdateProfile(string refid, object content)
        {
            Dictionary<string, object> jsonUpdate = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(content.ToString());
            MSSQLConnection.UpdateProfile(refid, jsonUpdate);
            //MongoDBConnector.UpdateProfile(refid, jsonUpdate);
        }

        [HttpPut("UpdateRival/{refid}")]
        public async void UpdateRival(string refid, object content)
        {
            Dictionary<string, object> jsonUpdate = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(content.ToString());
            MSSQLConnection.UpdateRival(refid, jsonUpdate);
        }
        //// GET: OptionController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: OptionController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: OptionController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: OptionController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: OptionController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: OptionController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: OptionController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
