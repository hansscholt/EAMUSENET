using BlazorCRUDApp.Shared;
using eamuse.KBin;
using eamuse.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace eamuse.Controllers
{
    
    [ApiController]
    public class Home : Controller
    {
        [HttpPost]
        [Route("core/{*url}")]
        public void Post()
        {
            string mo = Request.Query["module"];
            string me = Request.Query["method"];

            if (mo == "services")
                Services(me);
            else if (mo == "pcbtracker")
                PCBTracker(me);
            else if (mo == "package")
                Package(me);
            else if (mo == "message")
                Message(me);
            else if (mo == "facility")
                Facility(me);
            else if (mo == "pcbevent")
                PCBEvent(me);
            else if (mo == "dlstatus")
                DLStatus(me);
            else if (mo == "eventlog_2")
                EventLog2(me);
            else if (mo == "tax")
                Tax(me);
            else if (mo == "cardmng")
                CardMNG(me);
            else if (mo == "system_2")
                System2(me);
            else if (mo == "playerdata_2")
                PlayerData2(me);
            else
                Console.WriteLine("not mapped: " + mo);

        }
        void Services(string method)
        {
            string amuse = HttpContext.Request.Headers["X-Eamuse-Info"];
            string compress = HttpContext.Request.Headers["X-Compress"];

            XDocument outputDocument = new XDocument();

            if (method == "get")
            {                
                byte[] byteData = Util.ExtractRequest(Request, amuse, compress);
                KBinXML inputDocument = new KBinXML(byteData);
                string pcbid = inputDocument.Document.Element("call").Attribute("srcid").Value;
                
                XElement servicesElement = new XElement("services");


                string url = "http://199.102.48.162/core";
                url = "http://localhost:5091/core";


                servicesElement.Add(new XElement("item", new XAttribute("name", "cardmng"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "facility"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "message"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "numbering"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "package"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "pcbevent"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "pcbtracker"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "pkglist"), new XAttribute("url", url )));
                servicesElement.Add(new XElement("item", new XAttribute("name", "posevent"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "userdata"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "userid"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "eacoin"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "local"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "local2"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "lobby"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "lobby2"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "dlstatus"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "netlog"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "sidmgr"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "globby"), new XAttribute("url", url)));
                servicesElement.Add(new XElement("item", new XAttribute("name", "ntp"), new XAttribute("url", "ntp://pool.ntp.org/")));
                servicesElement.Add(new XElement("item", new XAttribute("name", "keepalive"), new XAttribute("url", "http://199.102.48.162/keepalive?pa=199.102.48.162&ia=199.102.48.162&ga=199.102.48.162&ma=199.102.48.162&t1=2&t2=10")));
                outputDocument = new XDocument(new XElement("response", servicesElement));
                Console.WriteLine("pcb correct: " + pcbid);
                
            }
            else
                Console.WriteLine("system :" + method);

            string algo = "lz77";
            byte[] resData = Util.CompressRequest(outputDocument, amuse, algo);

            Response.StatusCode = 200;
            Response.Headers.Add("X-Powered-By", "HANS");
            if (amuse != null)
                Response.Headers.Add("X-Eamuse-Info", amuse);
            Response.Headers.Add("X-Compress", algo);
            Response.ContentType = "application/octet-stream";
            Response.ContentLength = resData.Length;
            Response.Body.Write(resData, 0, resData.Length);
        }
        void PCBTracker(string method)
        {
            string amuse = HttpContext.Request.Headers["X-Eamuse-Info"];
            string compress = HttpContext.Request.Headers["X-Compress"];

            XDocument outputDocument = new XDocument();
            if (method == "alive")
            {
                byte[] byteData = Util.ExtractRequest(Request, amuse, compress);
                KBinXML inputDocument = new KBinXML(byteData);
                string hardid = inputDocument.Document.Element("call").Element("pcbtracker").Attribute("hardid").Value.ToUpper();
                if (hardid != "5BAE39118A6927555FC9")
                {
                    Response.StatusCode = 405;
                    return;
                }
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                ulong secondsSinceEpoch = (ulong)t.TotalMilliseconds;

                outputDocument = new XDocument(new XElement("response", new XElement("pcbtracker",
                    new XAttribute("ecenable", 1), new XAttribute("eclimit", 0), new XAttribute("expire", 1200), new XAttribute("limit", 0), new XAttribute("status", 0),
                    new XAttribute("time", secondsSinceEpoch)
                )));
            }
            else
                Console.WriteLine("pcbtracker: " + method);            

            string algo = "lz77";
            byte[] resData = Util.CompressRequest(outputDocument, amuse, algo);

            Response.StatusCode = 200;
            Response.Headers.Add("X-Powered-By", "HANS");
            if (amuse != null)
                Response.Headers.Add("X-Eamuse-Info", amuse);
            Response.Headers.Add("X-Compress", algo);
            Response.ContentType = "application/octet-stream";
            Response.ContentLength = resData.Length;
            Response.Body.Write(resData, 0, resData.Length);
        }
        void Package(string method)
        {
            string amuse = HttpContext.Request.Headers["X-Eamuse-Info"];
            string compress = HttpContext.Request.Headers["X-Compress"];

            XDocument data = new XDocument();
            if (method == "list")
                data = new XDocument(new XElement("response", new XElement("package", new XAttribute("expire", 1200), new XAttribute("status", 0))));
            else
                Console.WriteLine("package: " + method);

            string algo = "lz77";
            byte[] resData = Util.CompressRequest(data, amuse, algo);

            Response.StatusCode = 200;
            Response.Headers.Add("X-Powered-By", "HANS");
            if (amuse != null)
                Response.Headers.Add("X-Eamuse-Info", amuse);
            Response.Headers.Add("X-Compress", algo);
            Response.ContentType = "application/octet-stream";
            Response.ContentLength = resData.Length;
            Response.Body.Write(resData, 0, resData.Length);
        }
        void Message(string method)
        {
            string amuse = HttpContext.Request.Headers["X-Eamuse-Info"];

            XDocument outputDocument = new XDocument();

            if (method == "get")
            {
                outputDocument = new XDocument(new XElement("response", new XElement("message", 
                new XAttribute("expire", 300), new XAttribute("status", 0))));
            }
            else
                Console.WriteLine("message: " + method);

            string algo = "lz77";
            byte[] resData = Util.CompressRequest(outputDocument, amuse, algo);

            Response.StatusCode = 200;
            Response.Headers.Add("X-Powered-By", "HANS");
            if (amuse != null)
                Response.Headers.Add("X-Eamuse-Info", amuse);
            Response.Headers.Add("X-Compress", algo);
            Response.ContentType = "application/octet-stream";
            Response.ContentLength = resData.Length;
            Response.Body.Write(resData, 0, resData.Length);
        }
        void Facility(string method)
        {
            string amuse = HttpContext.Request.Headers["X-Eamuse-Info"];
            string compress = HttpContext.Request.Headers["X-Compress"];

            XDocument outputDocument = new XDocument();

            if (method == "get")
            {
                outputDocument = new XDocument(new XElement("response", new XElement("facility", new XAttribute("status", "0"),
                new XElement("location",
                    new KStr("id", "ea"),
                    new KStr("country", "SA"),
                    new KStr("region", "1"),
                    new KStr("name", "DDR HEVEN EAMUSENET"),
                    new KU8("type", 0),
                    new KStr("countryname", "SOUTHLA"),
                    new KStr("countryjname", ""),
                    new KStr("regionname", "HOME"),
                    new KStr("regionjname", ""),
                    new KStr("customercode", "X000000001"),
                    new KStr("companycode", "X000000001"),
                    new KS32("latitude", 6666),
                    new KS32("longitude", 6666),
                    new KU8("accuracy", 0)
                ),
                new XElement("line",
                    new KStr("id", "."),
                    new KU8("class", 0)
                ),
                new XElement("portfw",
                    new KIP4("globalip", IPAddress.Parse("127.0.0.1")),
                    new KU16("globalport", 5700),
                    new KU16("privateport", 5700)
                ),
                new XElement("public",
                    new KU8("flag", 1),
                    new KStr("name", "DDR HEVEN EAMUSENET"),
                    new KStr("latitude", "0"),
                    new KStr("longitude", "0")
                ),
                new XElement("share",
                    new XElement("eacoin",
                        new KS32("notchamount", 0),
                        new KS32("notchcount", 0),
                        new KS32("supplylimit", 100000)
                    ),
                    new XElement("url",
                        new KStr("eapass", "DDR HEVEN EAMUSENET"),
                        new KStr("arcadefan", "DDR HEVEN EAMUSENET"),
                        new KStr("konaminetdx", "DDR HEVEN EAMUSENET"),
                        new KStr("konamiid", "DDR HEVEN EAMUSENET"),
                        new KStr("eagate", "DDR HEVEN EAMUSENET")
                    )
                ))));
            }
            else
                Console.WriteLine("facility: " + method);

            string algo = "lz77";
            byte[] resData = Util.CompressRequest(outputDocument, amuse, algo);

            Response.StatusCode = 200;
            Response.Headers.Add("X-Powered-By", "HANS");
            if (amuse != null)
                Response.Headers.Add("X-Eamuse-Info", amuse);
            Response.Headers.Add("X-Compress", algo);
            Response.ContentType = "application/octet-stream";
            Response.ContentLength = resData.Length;
            Response.Body.Write(resData, 0, resData.Length);
        }
        void PCBEvent(string method)
        {
            string amuse = HttpContext.Request.Headers["X-Eamuse-Info"];
            string compress = HttpContext.Request.Headers["X-Compress"];

            XDocument outputDocument = new XDocument();

            if (method == "put")
                outputDocument = new XDocument(new XElement("response", new XElement("pcbevent", new XAttribute("status", 0))));
            else
                Console.WriteLine("pcbevent: " + method);

            string algo = "lz77";
            byte[] resData = Util.CompressRequest(outputDocument, amuse, algo);

            Response.StatusCode = 200;
            Response.Headers.Add("X-Powered-By", "HANS");
            if (amuse != null)
                Response.Headers.Add("X-Eamuse-Info", amuse);
            Response.Headers.Add("X-Compress", algo);
            Response.ContentType = "application/octet-stream";
            Response.ContentLength = resData.Length;
            Response.Body.Write(resData, 0, resData.Length);
        }
        void DLStatus(string method)
        {
            string amuse = HttpContext.Request.Headers["X-Eamuse-Info"];

            XDocument outputDocument = new XDocument();

            if (method == "progress")
                outputDocument = new XDocument(new XElement("response", new XElement("dlstatus", new XAttribute("status", 0))));
            else
                Console.WriteLine("dlstatus: " + method);

            string algo = "lz77";
            byte[] resData = Util.CompressRequest(outputDocument, amuse, algo);

            Response.StatusCode = 200;
            Response.Headers.Add("X-Powered-By", "HANS");
            if (amuse != null)
                Response.Headers.Add("X-Eamuse-Info", amuse);
            Response.Headers.Add("X-Compress", algo);
            Response.ContentType = "application/octet-stream";
            Response.ContentLength = resData.Length;
            Response.Body.Write(resData, 0, resData.Length);
        }
        void EventLog2(string method)
        {
            string amuse = HttpContext.Request.Headers["X-Eamuse-Info"];

            XDocument outputDocument = new XDocument();
            if (method == "write")
                outputDocument = new XDocument(new XElement("response", 
                    new XElement("eventlog_2", new XAttribute("status", 0),
                        new KS64("gamesession", 1),
                        new KS32("logsendflg", 0),
                        new KS32("logerrlevel", 0),
                        new KS32("evtidnosendflg", 0)
                    )));
            else
                Console.WriteLine("eventlog2: " + method);

            string algo = "lz77";
            byte[] resData = Util.CompressRequest(outputDocument, amuse, algo);

            Response.StatusCode = 200;
            Response.Headers.Add("X-Powered-By", "HANS");
            if (amuse != null)
                Response.Headers.Add("X-Eamuse-Info", amuse);
            Response.Headers.Add("X-Compress", algo);
            Response.ContentType = "application/octet-stream";
            Response.ContentLength = resData.Length;
            Response.Body.Write(resData, 0, resData.Length);
        }
        void Tax(string method)
        {
            string amuse = HttpContext.Request.Headers["X-Eamuse-Info"];

            XDocument outputDocument = new XDocument();

            if (method == "get_phase")
                outputDocument = new XDocument(new XElement("response", new XElement("tax"), new XAttribute("status", 0), new KS32("phase", 0)));
            else
                Console.WriteLine("tax: " + method);

            string algo = "lz77";
            byte[] resData = Util.CompressRequest(outputDocument, amuse, algo);

            Response.StatusCode = 200;
            Response.Headers.Add("X-Powered-By", "HANS");
            if (amuse != null)
                Response.Headers.Add("X-Eamuse-Info", amuse);
            Response.Headers.Add("X-Compress", algo);
            Response.ContentType = "application/octet-stream";
            Response.ContentLength = resData.Length;
            Response.Body.Write(resData, 0, resData.Length);
        }
        void CardMNG(string method)
        {
            string compress = HttpContext.Request.Headers["X-Compress"];
            string amuse = HttpContext.Request.Headers["X-Eamuse-Info"];
            byte[] byteData = Util.ExtractRequest(Request, amuse, compress);

            KBinXML inputDocument = new KBinXML(byteData);
            string pcbid = inputDocument.Document.Element("call").Attribute("srcid").Value.ToUpper();

            XDocument outputDocument = new XDocument();
            if (method == "inquire")
            {
                XElement cardmng = inputDocument.Document.Element("call").Element("cardmng");
                string cardid = cardmng.Attribute("cardid").Value.ToUpper();
                int update = (int)cardmng.Attribute("update");

                Card card = MSSQLConnection.GetCard(cardid, pcbid);

                if (card != null)
                {
                    if (update == 0)                    
                        Console.WriteLine("card found: " + card.cardid);
                    else
                        Console.WriteLine("card loaded: " + card.cardid);

                    outputDocument = new XDocument(new XElement("response", new XElement("cardmng",
                        new XAttribute("binded", "1"),
                        new XAttribute("dataid", card.refid),
                        new XAttribute("ecflag", "1"),
                        new XAttribute("newflag", "0"),
                        new XAttribute("expired", "0"),
                        new XAttribute("refid", card.refid),
                        new XAttribute("status", 0)
                    )));
                }
                else
                {
                    outputDocument = new XDocument(new XElement("response", new XElement("cardmng",
                        new XAttribute("status", 112)
                    )));
                }
            }
            else if (method == "authpass")
            {
                XElement cardmng = inputDocument.Document.Element("call").Element("cardmng");
                string pass = cardmng.Attribute("pass").Value;
                string refid = cardmng.Attribute("refid").Value.ToUpper();

                Card card = MSSQLConnection.GetCardByRefID(refid);

                if (card != null && card.pass == pass)
                {
                    Console.WriteLine("card pass ok");
                    outputDocument = new XDocument(new XElement("response", new XElement("cardmng",
                        new XAttribute("status", 0)
                    )));
                }
                else 
                {
                    Console.WriteLine("card pass wrong");
                    outputDocument = new XDocument(new XElement("response", new XElement("cardmng",
                            new XAttribute("status", 116)
                        )));
                }
            }
            else if (method == "getrefid")
            {
                Console.WriteLine("cardmng new refid: " + Util.randomCard);

                XElement cardmng = inputDocument.Document.Element("call").Element("cardmng");
                string cardid = cardmng.Attribute("cardid").Value.ToUpper();
                string passwd = cardmng.Attribute("passwd").Value;

                Player player = MSSQLConnection.GetPlayer(pcbid);

                Option option = new Option()
                {
                    refid = Util.randomCard,
                    common = "MSwwLDQ5MTJlOCwxLDAsMCwwLDAsMCxmZmZmZmZmZmZmZmZmZmZmLDAsMCwwLDAsMCwwLDAsMC4wMDAwMDAsMC4wMDAwMDAsMC4wMDAwMDAsMC4wMDAwMDAsMC4wMDAwMDAsMC4wMDAwMDAsMC4wMDAwMDAsMC4wMDAwMDAsLDA0NzgtODk2OCwsLCws",
                    option = "MSwzLDAsMCwwLDAsMCwzLDAsMCwwLDAsMSwyLDAsMCwwLDEwLjAwMDAwMCwxMC4wMDAwMDAsMTAuMDAwMDAwLDEwLjAwMDAwMCwwLjAwMDAwMCwwLjAwMDAwMCwwLjAwMDAwMCwwLjAwMDAwMCwsLCwsLCw=",
                    last = "MSwwLDAsMCwwLDAsMCwwLDAsMCwwLDAsMCwwLDAsMCwwLDAuMDAwMDAwLDAuMDAwMDAwLDAuMDAwMDAwLDAuMDAwMDAwLDAuMDAwMDAwLDAuMDAwMDAwLDAuMDAwMDAwLDAuMDAwMDAwLCwsLCwsLA==",
                    rival = "MCwwLDAsMCwwLDAsMCwwLDAsMCwwLDAsMCwwLDAsMCwwLDAuMDAwMDAwLDAuMDAwMDAwLDAuMDAwMDAwLDAuMDAwMDAwLDAuMDAwMDAwLDAuMDAwMDAwLDAuMDAwMDAwLDAuMDAwMDAwLCwsLCwsLA=="
                };
                int lastoptionid = MSSQLConnection.InsertOption(option);

                Card card = new Card()
                {
                    cardid = cardid,
                    dataid = Util.randomCard,
                    refid = Util.randomCard,
                    pass = passwd,
                    code = Util.randomCode,
                    codeint = int.Parse(Util.randomCode.Replace("-", string.Empty)),
                    pcbid = pcbid,
                    name = "",
                    single_grade = 0,
                    double_grade = 0,
                    golden_id = 0,
                    golden_score = 0,
                    golden_class = 0,
                    golden_count = 0,
                    date_init = DateTime.Now,
                    date_last = DateTime.Now,
                    //area = 1,
                    optionid = lastoptionid,
                    playerid = player.id
                };

                MSSQLConnection.InsertCard(card);
                outputDocument = new XDocument(new XElement("response", new XElement("cardmng",
                            new XAttribute("dataid", Util.randomCard), new XAttribute("refid", Util.randomCard)
                        )));
            }
            else            
                Console.WriteLine("cardmng: " + method);
            
            string algo = "lz77";
            byte[] resData = Util.CompressRequest(outputDocument, amuse, algo);

            Response.StatusCode = 200;
            Response.Headers.Add("X-Powered-By", "HANS");
            if (amuse != null)
                Response.Headers.Add("X-Eamuse-Info", amuse);
            Response.Headers.Add("X-Compress", algo);
            Response.ContentType = "application/octet-stream";
            Response.ContentLength = resData.Length;
            Response.Body.Write(resData, 0, resData.Length);
        }
        void System2(string method)
        {
            string compress = HttpContext.Request.Headers["X-Compress"];
            string amuse = HttpContext.Request.Headers["X-Eamuse-Info"];            
            XDocument outputDocument = new XDocument();

            if (method == "convcardnumber")
            {
                byte[] byteData = Util.ExtractRequest(Request, amuse, compress);
                KBinXML inputDocument = new KBinXML(byteData);
                string pcbid = inputDocument.Document.Element("call").Attribute("srcid").Value.ToUpper();
                string cardid = inputDocument.Document.Element("call").Element("system_2").Element("data").Element("card_id").Value;
                Card card = MSSQLConnection.GetCard(cardid, string.Empty);

                
                if (card != null)
                {
                    outputDocument = new XDocument(new XElement("response", new XElement("system_2", new XAttribute("status", "0"),
                    new KS32("result", 0),
                    new XElement("data", new KStr("card_number", card.dataid)
                    ))));
                }
                else
                {
                    Util.RandomCardString();
                    outputDocument = new XDocument(new XElement("response", new XElement("system_2", new XAttribute("status", "0"),
                    new KS32("result", 0),
                    new XElement("data", new KStr("card_number", Util.randomCard)
                    ))));

                    string seqCode1 = Util.RandomSeqCode();
                    string seqCode2 = Util.RandomSeqCode();
                    string seqCode = seqCode1 + "-" + seqCode2;
                    Util.randomCode = seqCode;
                    Console.WriteLine("refid not found convcard event: " + Util.randomCard);
                }
            }
            else
                Console.WriteLine("system_2: " + method);

            string algo = "lz77";
            byte[] resData = Util.CompressRequest(outputDocument, amuse, algo);

            Response.StatusCode = 200;
            Response.Headers.Add("X-Powered-By", "HANS");
            if (amuse != null)
                Response.Headers.Add("X-Eamuse-Info", amuse);
            Response.Headers.Add("X-Compress", algo);
            Response.ContentType = "application/octet-stream";
            Response.ContentLength = resData.Length;
            Response.Body.Write(resData, 0, resData.Length);
        }
        void PlayerData2(string me)
        {
            string compress = HttpContext.Request.Headers["X-Compress"];
            string amuse = HttpContext.Request.Headers["X-Eamuse-Info"];
            byte[] byteData = Util.ExtractRequest(Request, amuse, compress);
            KBinXML inputDocument = new KBinXML(byteData);

            XDocument outputData = new XDocument();
            if (me == "usergamedata_advanced")
            {
                string pbcid = inputDocument.Document.Element("call").Attribute("srcid").Value;

                XElement playerdataCall = inputDocument.Document.Element("call").Element("playerdata_2");
                string refid = playerdataCall.Element("data").Element("refid").Value.ToUpper();
                string mode = playerdataCall.Element("data").Element("mode").Value.ToUpper();


                XElement playerdata = new XElement("playerdata_2");
                if (mode == "USERLOAD")
                {
                    playerdata = new XElement("playerdata_2");
                    playerdata.Add(new KS32("result", 0), new KBool("is_new", false), new KBool("is_refid_locked", false)
                        , new KS16("eventdata_count_all", 1)
                        );
                    if (refid != "X0000000000000000000000000000001")    //new card
                    {
                        Console.WriteLine("load scores");

                        List <Score> listScores = MSSQLConnection.GetMaxScoresByRefID(refid);

                        foreach (var s in listScores)
                        {
                            var musicNode = new XElement("music", new KU32("mcode", (uint)s.mcode));
                            for (int i = 0; i < 9; i++)
                            {
                                Score score = listScores.Find(x => x.notetype == i && x.mcode == s.mcode);
                                if (score != null)
                                {
                                    musicNode.Add(new XElement("note",
                                        new KU16("count", (ushort)score.count),
                                        new KU8("rank", (byte)score.rank),
                                        new KU8("clearkind", (byte)score.clearkind),
                                        new KS32("score", score.score),
                                        new KS32("ghostid", score.ghostid)));
                                }
                                else
                                    musicNode.Add(new XElement("note"));
                            }
                            playerdata.Add(new XElement(musicNode));
                        }

                        for (int i = 1; i < 100; i++)
                        {
                            if (i == 4 || i == 6 || i == 7 || i == 8 || i == 14 || i == 47)
                                continue;
                            XElement e = new XElement("eventdata");
                            e.Add(new KU32("eventid", (uint)i),
                                new KS32("eventtype", 9999),
                                new KU32("eventno", 0),
                                new KS64("condition", 0),
                                new KU32("reward", 0),
                                new KS32("comptime", 1),
                                new KS64("savedata", 0));
                            playerdata.Add(e);
                        }

                        Card currentCard = MSSQLConnection.GetCardByRefID(refid);

                        playerdata.Add(new XElement("grade", new KU32("single_grade", (uint)currentCard.single_grade), new KU32("double_grade", (uint)currentCard.double_grade)));

                        XElement current = new XElement("current",
                            new KS32("id", currentCard.golden_id),
                            new KStr("league_name_base64", ""),
                            new KU64("start_time", 0),
                            new KU64("end_time", 0),
                            new KU64("summary_time", 0),
                            new KS32("league_status", 1),
                            new KS32("league_class", currentCard.golden_class),
                            new KS32("league_class_result", 0),
                            new KS32("ranking_number", 0),
                            new KS32("total_exscore", currentCard.golden_score),
                            new KS32("total_play_count", currentCard.golden_count),
                            new KS32("join_number", 0),
                            new KS32("promotion_ranking_number", 0),
                            new KS32("demotion_ranking_number", 0),
                            new KS32("promotion_exscore", 0),
                            new KS32("demotion_exscore", 0));
                        playerdata.Add(new XElement("golden_league", new KS32("league_class", currentCard.golden_class), current));

                        Console.WriteLine("loaded");
                    }
                }
                else if (mode == "USERNEW")
                {
                    playerdata = new XElement("playerdata_2");
                    playerdata.Add(new KS32("result", 0), new KStr("seq", Util.randomCode), new KS32("code", int.Parse(Util.randomCode.Replace("-", string.Empty))), new KStr("shoparea", "1"));
                }
                else if (mode == "INHERITANCE")
                {
                    playerdata = new XElement("playerdata_2");
                    playerdata.Add(new KS32("result", 0), new KS32("InheritanceStatus", 1));
                }
                else if (mode == "USERSAVE")  //scores
                {
                    playerdata = new XElement("playerdata_2");
                    IEnumerable<XElement> noteData = playerdataCall.Element("data").Elements("note");
                    string shoparea = playerdataCall.Element("data").Element("shoparea").Value;

                    Card currentCard = MSSQLConnection.GetCardByRefID(refid);

                    int isGameOver = (int)playerdataCall.Element("data").Element("isgameover");

                    if (isGameOver == 0)    //save here or on gameover?
                    {
                        Console.WriteLine("save score");
                        foreach (var note in noteData)
                        {
                            int stagenum = (int)note.Element("stagenum");
                            if (stagenum == 0)
                                continue;

                            int mcode = (int)note.Element("mcode");
                            int notetype = (int)note.Element("notetype");

                            if (Util.FindMusicBymcode(mcode) == null)//by list
                                continue;
                            
                            Ghost ghost = new Ghost()
                            {
                                ghostsize = (int)note.Element("ghostsize"),
                                ghost = note.Element("ghost").Value,
                                mcode = mcode,
                                notetype = notetype
                            };
                            int ghostid = MSSQLConnection.InsertGhost(ghost);

                            Score score = new Score();
                            score = new Score();
                            score.mcode = mcode;
                            score.clearkind = (int)note.Element("clearkind");
                            score.count = 1;
                            score.ghostid = ghostid;
                            score.rank = (int)note.Element("rank");
                            score.score = (int)note.Element("score");
                            score.exscore = (int)note.Element("exscore");
                            score.maxcombo = (int)note.Element("maxcombo");
                            score.notetype = notetype;
                            score.judge_marvelous = (int)note.Element("judge_marvelous");
                            score.judge_perfect = (int)note.Element("judge_perfect");
                            score.judge_great = (int)note.Element("judge_great");
                            score.judge_good = (int)note.Element("judge_good");
                            score.judge_boo = (int)note.Element("judge_boo");
                            score.judge_miss = (int)note.Element("judge_miss");
                            score.judge_ok = (int)note.Element("judge_ok");
                            score.judge_ng = (int)note.Element("judge_ng");
                            score.fastcount = (int)note.Element("fastcount");
                            score.slowcount = (int)note.Element("slowcount");
                            score.cardid = currentCard.id;
                            score.date = DateTime.Now;

                            MSSQLConnection.InsertScore(score);                            
                        }
                    }
                    else
                    {
                        XElement grade = playerdataCall.Element("data").Element("grade");
                        if (grade != null)
                        {
                            currentCard.single_grade = (int)grade.Element("single_grade");
                            currentCard.double_grade = (int)grade.Element("double_grade");
                        }

                        XElement golden_league = playerdataCall.Element("data").Element("golden_league");
                        if (golden_league != null)
                        {
                            currentCard.name = playerdataCall.Element("data").Element("name").Value.ToUpper();
                            currentCard.golden_class = (int)golden_league.Element("current").Element("league_class");
                            currentCard.golden_count = (int)golden_league.Element("current").Element("total_play_count");
                            currentCard.golden_id = (int)golden_league.Element("current").Element("id");
                            currentCard.golden_score = (int)golden_league.Element("current").Element("total_exscore");
                        }
                        
                        currentCard.date_last = DateTime.Now;

                        MSSQLConnection.UpdateCard(currentCard);
                    }
                    playerdata.Add(new KS32("result", 0));
                }
                else if (mode == "GHOSTLOAD")
                {
                    playerdata = new XElement("playerdata_2");
                    playerdata.Add(new KS32("result", 0));

                    int ghostid = (int)playerdataCall.Element("data").Element("ghostid");
                    Ghost ghost = MSSQLConnection.GetGhost(ghostid);
                    if (ghost != null)
                    {
                        playerdata.Add(new XElement("ghostdata", new KS32("code", 0), new KU32("mcode", (uint)ghost.mcode), new KU8("notetype", (byte)ghost.notetype),
                        new KS32("ghostsize", ghost.ghostsize), new KStr("ghost", ghost.ghost)));
                    }
                }
                else if (mode == "RIVALLOAD")
                {
                    playerdata = new XElement("playerdata_2");

                    var dataNode = playerdataCall.Element("data");
                    int loadflag = (int)dataNode.Element("loadflag");
                    int shoparea = (int)dataNode.Element("shoparea");
                    int rivalddrcode = (int)dataNode.Element("ddrcode");

                    playerdata.Add(new KS32("result", 0));
                    XElement outputDataNode = new XElement("data");

                    if (loadflag == 1)  //machine
                    {
                        Console.WriteLine("machine load");
                        List<Score> listScores = MSSQLConnection.GetMaxScoresByPCBID(pbcid);

                        foreach (var record in listScores)
                        {
                            string[] csvarray = new string[]
                            {
                                record.mcode.ToString(),
                                record.notetype.ToString(),
                                record.rank.ToString(),
                                record.clearkind.ToString(),
                                record.name,
                                record.area.ToString(),
                                record.code.ToString(),
                                record.score.ToString(),
                                record.ghostid.ToString()
                            };
                            string toEncode = string.Join(",", csvarray);
                            var recordNode = new XElement("record", new KStr("record_csv", Convert.ToBase64String(Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(toEncode)))));
                            outputDataNode.Add(recordNode);
                        }
                    }
                    else if (loadflag == 2)  //area
                    {
                        Console.WriteLine("area load");

                        List<Score> listScores = MSSQLConnection.GetMaxScoresByArea(shoparea);

                        foreach (var record in listScores)
                        {
                            string[] csvarray = new string[]
                            {
                                record.mcode.ToString(),
                                record.notetype.ToString(),
                                record.rank.ToString(),
                                record.clearkind.ToString(),
                                record.name,
                                record.area.ToString(),
                                record.code.ToString(),
                                record.score.ToString(),
                                record.ghostid.ToString()
                            };
                            string toEncode = string.Join(",", csvarray);
                            var recordNode = new XElement("record", new KStr("record_csv", Convert.ToBase64String(Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(toEncode)))));
                            outputDataNode.Add(recordNode);
                        }
                    }
                    else if (loadflag == 4)  //all
                    {
                        Console.WriteLine("global load");
                        List<Score> listScores = MSSQLConnection.GetScores(true);

                        foreach (var record in listScores)
                        {
                            string[] csvarray = new string[]
                            {
                                record.mcode.ToString(),
                                record.notetype.ToString(),
                                record.rank.ToString(),
                                record.clearkind.ToString(),
                                record.name,
                                record.area.ToString(),
                                record.code.ToString(),
                                record.score.ToString(),
                                record.ghostid.ToString()
                            };
                            string toEncode = string.Join(",", csvarray);
                            var recordNode = new XElement("record", new KStr("record_csv", Convert.ToBase64String(Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(toEncode)))));
                            outputDataNode.Add(recordNode);
                        }
                    }
                    else if (loadflag == 8 || loadflag == 16 || loadflag == 32)  //rival
                    {
                        Console.WriteLine("rival load");
                        Card rivalCard = MSSQLConnection.GetCardByCode(rivalddrcode);

                        if (rivalCard == null)
                            Console.WriteLine("rival no found: " + rivalddrcode);
                        else
                        {
                            List<Score> listScores = MSSQLConnection.GetMaxScoresByCode(rivalddrcode);

                            foreach (var record in listScores)
                            {
                                string[] csvarray = new string[]
                                {
                                record.mcode.ToString(),
                                record.notetype.ToString(),
                                record.rank.ToString(),
                                record.clearkind.ToString(),
                                record.name,
                                record.area.ToString(),
                                record.code.ToString(),
                                record.score.ToString(),
                                record.ghostid.ToString()
                                };
                                string toEncode = string.Join(",", csvarray);
                                var recordNode = new XElement("record", new KStr("record_csv", Convert.ToBase64String(Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(toEncode)))));
                                outputDataNode.Add(recordNode);

                            }
                        }
                    }
                    else
                        Console.WriteLine("rival loadflag: " + loadflag);

                    playerdata.Add(outputDataNode);
                }
                else
                    Console.WriteLine("usergamedata_advanced usermode: " + mode);

                outputData = new XDocument(new XElement("response", playerdata));
            }
            else if (me == "usergamedata_recv")
            {
                XElement playerdataCall = inputDocument.Document.Element("call").Element("playerdata_2");
                string refid = playerdataCall.Element("data").Element("refid").Value;
                Option option = MSSQLConnection.GetOption(refid);

                outputData = new XDocument(new XElement("response",
                    new XElement("playerdata_2", new XAttribute("status", 0),
                    new KS32("result", 0),
                    new XElement("player", new XElement("record",
                    new KStr("d", option.common),
                    new KStr("d", option.option),
                    new KStr("d", option.last),
                    new KStr("d", option.rival)),
                    new KU32("record_num", 4)))));
            }
            else if (me == "usergamedata_send")
            {
                XElement playerdataCall = inputDocument.Document.Element("call").Element("playerdata_2");
                string refid = playerdataCall.Element("data").Element("refid").Value;
                int numData = (int)playerdataCall.Element("data").Element("datanum");
                var dataChain = playerdataCall.Element("data").Element("record").Elements();

                if (numData > 0)   //update
                {
                    Option option = MSSQLConnection.GetOption(refid);

                    foreach (var sChain in dataChain)
                    {
                        byte[] bData = Convert.FromBase64String(sChain.Value);
                        string[] decodedString = Encoding.UTF8.GetString(bData).Split(',');
                        var optionList = new List<string>(Encoding.UTF8.GetString(bData).Split(','));
                        optionList.RemoveRange(0, 2);
                        if (decodedString[1].ToUpper() == "COMMON")
                        {
                            string sCommon = String.Join(",", optionList);
                            option.common = Convert.ToBase64String(Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(sCommon)));

                        }
                        else if (decodedString[1].ToUpper() == "OPTION")
                        {
                            string sOption = String.Join(",", optionList);
                            option.option = Convert.ToBase64String(Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(sOption)));

                        }
                        else if (decodedString[1].ToUpper() == "LAST")
                        {
                            string sLast = String.Join(",", optionList);
                            option.last = Convert.ToBase64String(Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(sLast)));

                        }
                        else if (decodedString[1].ToUpper() == "RIVAL")
                        {
                            string sRival = String.Join(",", optionList);
                            option.rival = Convert.ToBase64String(Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(sRival)));
                        }
                        else
                        {
                            Console.WriteLine(decodedString[1].ToUpper());
                            Console.WriteLine(sChain.Value);
                        }
                    }
                    Console.WriteLine("save option");

                    MSSQLConnection.UpdateOption(option);
                }

                outputData = new XDocument(new XElement("response",
                new XElement("playerdata_2", new XAttribute("status", 0), new KS32("result", 0))));
            }
            else
                Console.WriteLine("playerdata_2: " + me);

            string algo = "lz77";
            byte[] resData = Util.CompressRequest(outputData, amuse, algo);

            Response.StatusCode = 200;
            Response.Headers.Add("X-Powered-By", "HANS");
            if (amuse != null)
                Response.Headers.Add("X-Eamuse-Info", amuse);
            Response.Headers.Add("X-Compress", algo);
            Response.ContentType = "application/octet-stream";
            Response.ContentLength = resData.Length;
            Response.Body.Write(resData, 0, resData.Length);
        }
    }
}