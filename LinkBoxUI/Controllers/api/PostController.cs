using DataAccessLayer.Class;
using LinkBoxUI.Context;
using LinkBoxUI.Helpers;
using LinkBoxUI.Services;
using DomainLayer.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using System.Net.Http;
using LinkBoxUI.SwaggerDocuments;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Text;
using LinkBoxUI.Properties;
using DomainLayer.Models;
using Microsoft.Owin.Security.Provider;
using NPOI.SS.Formula.Functions;

namespace LinkBoxUI.Controllers.api
{
    [RoutePrefix("Linkbox")]
    public class PostController : ApiController
    {
        public SqlHelpers sql = new SqlHelpers();
        public SAPAccess sapAces = new SAPAccess();
        LinkboxDb _context = new LinkboxDb();
        GlobalServices globalServices = new GlobalServices();
        EmailHelpers eml = new EmailHelpers();
        [HttpPost]
        [Route("Post")]
        public bool Post()
        {
            try
            {
                //Properties.Settings prop = new Properties.Settings();
                Settings prop = new Settings();
                foreach (var task in TaskSchedulerHelpers.getRunningTask(prop.ServerName, prop.User, prop.Domain, prop.Password))
                {

                    var EmailCreds = globalServices.GetEmailCredentials(task);


                    if (EmailCreds.EmailCreds != null)
                    {
                        if (eml.Send(EmailCreds).ToLower().Contains("success"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        [SwaggerOperation(Tags = new[] { "MOOMasterData" })]
        [HttpPost]
        [Route("moo/post/masterdata")]
        public string MOOMasterData(string taskname)
        {
            string ret = "";
            var _apimap = globalServices.MasterDataUploadSAPtoAPI(taskname);
            return ret;
        }

        [HttpPost]
        [Route("ManualPost")]
        public string ManualPost(string Code)
        {
            try
            {

                var EmailCreds = globalServices.GetEmailCredentials(Code);
                if (EmailCreds.EmailCreds != null)
                {
                    return eml.Send(EmailCreds);
                }
                return "No Data";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        [SwaggerOperation(Tags = new[] { "LoginToSAPDB" })]
        [HttpPost]
        [Route("sap/login")]
        public bool Login(AuthenticationCredViewModel auth)
        {
            bool result = false;

            if (auth != null)
            {
                sapAces.SaveCredentials(auth);
                result = SAPAccess.LoginAction();
            }

            return result;
        }

        [SwaggerOperation(Tags = new[] { "LoginService" })]
        [HttpPost]
        [Route("sap/post")]
        public string PostSL(AuthenticationCredViewModel auth)
        {
            string ret = "";
            bool blnLogin = false;
            sapAces.SaveCredentials(auth);
            blnLogin = SAPAccess.LoginAction();

            if (blnLogin == true)
            {
                ret = sapAces.SendSLData(auth);
            }
            return ret;
        }

        [SwaggerOperation(Tags = new[] { "UploadDataSAPToAddon" })]
        [HttpPost]
        [Route("sap/upload/documents")]
        public string SapPostDocument(string taskname)
        {
            string ret = "";
            _context.SystemLogs.Add(new DomainLayer.Models.SystemLog { Task = "API_POSTDOCS", CreateDate = DateTime.Now, ApiUrl = $@"", Json = "", ErrorMsg = "THIS IS INFORMATION ONLY" });
            _context.SaveChanges();
            var _apimap = globalServices.GetApiOrderPostSAP(taskname);

            return ret;
        }

        [SwaggerOperation(Tags = new[] { "UploadDataSAPToAddon" })]
        [HttpPost]
        [Route("sap/upload/documents")]
        public string ScheduleSapUploadDocument(string Code)
        {

            string ret = "";
            var mapcodes = _context.Schedules.Where(x => x.SchedCode == Code)
                                .Join(_context.Process, sc => sc.Process, pr => pr.ProcessCode, (sc, pr) => new { sc, pr})
                                .Join(_context.ProcessMap, scpr => scpr.pr.ProcessId, pm => pm.ProcessId, (scpr, pm) => new { scpr, pm })
                                .Join(_context.FieldMappings, scprpm => scprpm.pm.MapId, fm => fm.MapId, (scprpm, fm) => new
                                {
                                    MapCode = fm.MapCode,
                                }).ToList();
            string[] modules = mapcodes.Select(x => x.MapCode).ToArray();

            //Add Function
            _context.SystemLogs.Add(new DomainLayer.Models.SystemLog { Task = "SAP_POSTDOCS", CreateDate = DateTime.Now, ApiUrl = $@"", Json = "", ErrorMsg = "THIS IS INFORMATION ONLY" });
            _context.SaveChanges();
            var _apimap = globalServices.GetSAPPostUploadtoAddon(modules);
            return ret;
        }

        [SwaggerOperation(Tags = new[] { "UploadDataSAPToAddon" })]
        [HttpPost]
        [Route("sap/upload/documents")]
        public string SapUploadDocument(string[] modules)
        {

            string ret = "";
            //Add Function
            _context.SystemLogs.Add(new DomainLayer.Models.SystemLog { Task = "SAP_POSTDOCS", CreateDate = DateTime.Now, ApiUrl = $@"", Json = "", ErrorMsg = "THIS IS INFORMATION ONLY" });
            _context.SaveChanges();
            var _apimap = globalServices.GetSAPPostUploadtoAddon(modules);
            //var _apimap = globalServices.UploadOPSFiletoSap(modules);

            
            return ret;
        }

        [SwaggerOperation(Tags = new[] { "UploadDataSAPToAddon" })]
        [HttpPost]
        [Route("sap/upload/data")]
        //public string SaptoSapUploadDocument(string[] modules)
        public string SaptoSapUploadDocument(string modules)
        {
            string[] test = new string[1];
            test[0] = modules;
            string ret = "";
            //Add Function
            _context.SystemLogs.Add(new DomainLayer.Models.SystemLog { Task = "SAP_POSTDOCS", CreateDate = DateTime.Now, ApiUrl = $@"", Json = "", ErrorMsg = "THIS IS INFORMATION ONLY" });
            _context.SaveChanges();
            var _apimap = globalServices.GetSAPPostUploadtoSAP(test);
            //var _apimap = globalServices.GetSAPPostUploadtoSAP(modules);
            return ret;
        }

        [SwaggerOperation(Tags = new[] { "ShopifyProducts" })]
        [HttpPost]
        [Route("shopify/post/items")]
        public string ShopifyPostProducts(string taskname)
        {
            string ret = "";
            var _apimap = globalServices.GetSAPPostShopiAPI(taskname);
            return ret;
        }

        [SwaggerOperation(Tags = new[] { "ZendeskTiket" })]
        [HttpPost]
        [Route("zendesk/post/tickets")]
        public string ZendeskPostTickets(string taskname)
        {
            string ret = "";
            var _apimap = globalServices.ZendeskPostTicket(taskname);
            return ret;
        }

        [SwaggerOperation(Tags = new[] { "MOOSalesOrder" })]
        [Authorize]
        [HttpPost]
        [Route("linkbox/post/apitoken/so")]
        public IHttpActionResult LinkboxAPI([FromBody] JObject jObj)
        {
            //dynamic newRetObj = new JObject();
            string res = "";
            try
            {
                if(jObj == null)
                {
                    var dict = new Dictionary<string, string>();
                    dict.Add("error", "Bad format");
                    string JSONresult = JsonConvert.SerializeObject(dict);
                    var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(JSONresult, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }

                /// Multiple JObject Posting
                res = globalServices.PostJArraySO(jObj.ToString());
            }
            catch (Exception exc)
            {
                string msg = exc.Message;
                string exValid = "Unexpected JSON token";
                if (msg.Trim().ToLower().Contains(exValid.Trim().ToLower()))
                {
                    try
                    {
                        /// Single JObject Posting
                        res = globalServices.PostJObjectSO(jObj.ToString());
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }
                else
                {
                    return InternalServerError(exc);
                }
            }
            //return newRetObj;
            return Ok(res);
        }

        [SwaggerOperation(Tags = new[] { "ShopifyToSapBPCatalog" })]
        [HttpPost]
        [Route("sap/post/bpcatalog")]
        public string SapPostCatalog(string taskname)
        {
            string ret = "";
            var _apimap = globalServices.PostSapCatalog(taskname);
            return ret;
        }

        [SwaggerOperation(Tags = new[] { "ShopifyInventory" })]
        [HttpPost]
        [Route("shopify/post/inventory")]
        public string ShopifyPostInventory(string taskname)
        {
            string ret = "";
            _context.SystemLogs.Add(new DomainLayer.Models.SystemLog { Task = "API_INVTY", CreateDate = DateTime.Now, ApiUrl = $@"", Json = "", ErrorMsg = "THIS IS INFORMATION ONLY" });
            _context.SaveChanges();
            var _apimap = globalServices.PostAPIInventory(taskname);
            return ret;
        }

        [SwaggerOperation(Tags = new[] { "UploadDataSAPToAddon" })]
        [HttpPost]
        [Route("sap2api/upload")] 
        public string SaptoSapUpload(string code)
        {
            string ret = "";
            //Add Function
            var mapcodes = _context.Schedules.Where(x => x.SchedCode == code)
                    .Join(_context.Process, sc => sc.Process, pr => pr.ProcessCode, (sc, pr) => new { sc, pr })
                    .Join(_context.ProcessMap, scpr => scpr.pr.ProcessId, pm => pm.ProcessId, (scpr, pm) => new { scpr, pm })
                    .Join(_context.FieldMappings, scprpm => scprpm.pm.MapId, fm => fm.MapId, (scprpm, fm) => new
                    {
                        MapCode = fm.MapCode,
                    }).ToList();
            string[] modules = mapcodes.Select(x => x.MapCode).ToArray();
            _context.SystemLogs.Add(new DomainLayer.Models.SystemLog { Task = "SAP_POSTDOCS", CreateDate = DateTime.Now, ApiUrl = $@"", Json = "", ErrorMsg = "THIS IS INFORMATION ONLY" });
            _context.SaveChanges();
            var _apimap = globalServices.GetSAPPostUploadtoSAP(modules);
            return ret;
        }

        [SwaggerOperation(Tags = new[] { "UploadFileToSap" })]
        [HttpPost]
        [Route("filetosap/upload")]
        public string FileUploadToSap(string Code)
        {
            string ret = "";
            var mapcodes = _context.Schedules.Where(x => x.SchedCode == Code)
                                .Join(_context.Process, sc => sc.Process, pr => pr.ProcessCode, (sc, pr) => new { sc, pr })
                                .Join(_context.ProcessMap, scpr => scpr.pr.ProcessId, pm => pm.ProcessId, (scpr, pm) => new { scpr, pm })
                                .Join(_context.FieldMappings, scprpm => scprpm.pm.MapId, fm => fm.MapId, (scprpm, fm) => new
                                {
                                    MapCode = fm.MapCode,
                                }).ToList();
            string[] modules = mapcodes.Select(x => x.MapCode).ToArray();

            //Add Function
            _context.SystemLogs.Add(new DomainLayer.Models.SystemLog { Task = "SAP_POSTDOCS", CreateDate = DateTime.Now, ApiUrl = $@"", Json = "", ErrorMsg = "THIS IS INFORMATION ONLY" });
            _context.SaveChanges();
            var _apimap = globalServices.UploadFiletoSap(modules);
            return ret;
        }

        [SwaggerOperation(Tags = new[] { "UploadOPSFileToSap" })]
        [HttpPost]
        [Route("opsupload")]
        public string OPSFileUploadToSap(string Code)
        {
            string ret = "";

            var proc = _context.Schedules.Where(x => x.SchedCode == Code).FirstOrDefault();

            //if (_context.Schedules.Where(sel => sel.SchedCode == Code && sel.ForceStop == true).Any())
            //{ return "Stopping Process"; }

            if (proc.RunTime == null)
            {
                proc.RunTime = DateTime.Now.AddMinutes(-8);
                _context.SaveChanges();
            }

            var runntime = _context.Schedules.AsEnumerable().Where(sel => sel.SchedCode == Code).Select(x => x.RunTime).FirstOrDefault();
            var currtime = DateTime.Now;

            if ((currtime.Minute - runntime.Value.Minute) > 4 || currtime.Hour != runntime.Value.Hour)
            {
                _context.SaveChanges();
                proc.IsRunning = false;
            }
            var isrunproc = _context.Schedules.Where(sel => sel.SchedCode == Code).Select(sel => sel.IsRunning).FirstOrDefault();
        
            if (isrunproc == false)
            {

                var mapcodes = _context.Schedules.Where(x => x.SchedCode == Code)
                  .Join(_context.Process, sc => sc.Process, pr => pr.ProcessCode, (sc, pr) => new { sc, pr })
                  .Join(_context.ProcessMap, scpr => scpr.pr.ProcessId, pm => pm.ProcessId, (scpr, pm) => new { scpr, pm })
                  .Join(_context.OPSFieldMappings, scprpm => scprpm.pm.MapId, fm => fm.MapId, (scprpm, fm) => new
                  {
                      MapCode = fm.MapCode,
                      VisOrder = scprpm.pm.VisOrder,
                   
                  }).ToList().OrderBy(x => x.VisOrder);

                string[] modules = mapcodes.Select(x => x.MapCode).ToArray();


                //Add Function
                //var mapcodes = _context.Schedules.Where(x => x.SchedCode == Code)
                //    .Join(_context.Process, sc => sc.Process, pr => pr.ProcessCode, (sc, pr) => new { sc, pr })
                //    .Join(_context.ProcessMap, scpr => scpr.pr.ProcessId, pm => pm.ProcessId, (scpr, pm) => new { scpr, pm })
                //    .Join(_context.OPSFieldMappings, scprpm => scprpm.pm.MapId, fm => fm.MapId, (scprpm, fm) => new
                //    {
                //        MapCode = fm.MapCode,
                //        VisOrder = scprpm.pm.VisOrder,
                       
                //    }).ToList().OrderBy(x => x.MapCode);
                //string[] modules = mapcodes.Select(x => x.MapCode).ToArray();


                proc.IsRunning = true;
                proc.RunTime = DateTime.Now;
                _context.SaveChanges();
                //Add Function
                //_context.SystemLogs.Add(new DomainLayer.Models.SystemLog { Task = "SAP_POSTDOCS", CreateDate = DateTime.Now, ApiUrl = $@"", Json = "", ErrorMsg = "THIS IS INFORMATION ONLY" });
                //_context.SaveChanges();

                try
                { 
                    var _apimap = globalServices.UploadOPSFiletoSap(modules, Code);
                    ret = "Success!";
                }
                catch (Exception ex)
                {
                    ret = ex.Message;
                    var errmsg = $@"{ex.Message} | {ex.StackTrace}";
                    var _currDate = DateTime.Now.Date;
                    if (!ex.Message.ToLower().Contains("this method cannot be called until the send method has been called"))
                    {
                        _context.SystemLogs.Add(new DomainLayer.Models.SystemLog { Task = "SAP_SERVICE_EROR", CreateDate = DateTime.Now, ApiUrl = $@"", ErrorMsg = $@"{ex.Message} | {ex.StackTrace}", Json = "", Database = "", Table = "", Module = "" });
                        _context.SaveChanges();
                    }
                }

                proc.IsRunning = false;
                _context.SaveChanges();

                var lastmsg = _context.SystemLogs.OrderByDescending(x => x.Id).Select(x => x.Task).FirstOrDefault();
                if (lastmsg.ToLower().Contains("upload_end"))
                {
                    _context.SystemLogs.Add(new DomainLayer.Models.SystemLog { Task = "UPLOAD_END", CreateDate = DateTime.Now, ApiUrl = $@"", Json = "", ErrorMsg = "THIS IS INFORMATION ONLY" });
                    _context.SaveChanges();
                }

            }//end of if

            return ret;


        }

        [HttpPost]
        [Route("sap/crexport")]
        public string CrystalExtract(int id)
            {
      
            string ret = "error";
            var crystalExtract = _context.CrystalExtractSetup
                         .FirstOrDefault(x => x.Id == id);

            if (crystalExtract != null)
            {
                string name = crystalExtract.Name;

                var proc = _context.Schedules.Where(x => x.Process == name).FirstOrDefault();

                if (proc.RunTime == null)
                {
                    proc.RunTime = DateTime.Now.AddMinutes(-8);
                    _context.SaveChanges();
                }

                var runntime = _context.Schedules.AsEnumerable().Where(sel => sel.Process == name).Select(x => x.RunTime).FirstOrDefault();
                var currtime = DateTime.Now;

                if ((currtime.Minute - runntime.Value.Minute) > 4 || currtime.Hour != runntime.Value.Hour)
                {
                    _context.SaveChanges();
                    proc.IsRunning = false;
                }

                var isrunproc = _context.Schedules.Where(sel => sel.Process == name).Select(sel => sel.IsRunning).FirstOrDefault();
             
                if (isrunproc == false)
                {

                    proc.IsRunning = true;
                    proc.RunTime = DateTime.Now;
                    _context.SaveChanges();


                    try
                    {
                        globalServices.ExtractCrystalReport(name);
                        ret = "Success!";
                    }
                    catch (Exception ex)
                    {
                        ret = ex.Message;
                        var errmsg = $@"{ex.Message} | {ex.StackTrace}";
                        var _currDate = DateTime.Now.Date;
                        if (!ex.Message.ToLower().Contains("this method cannot be called until the send method has been called"))
                        {
                            _context.SystemLogs.Add(new DomainLayer.Models.SystemLog { Task = "CRYSTAL_REPORT_EROR", CreateDate = DateTime.Now, ApiUrl = $@"", ErrorMsg = $@"{ex.Message} | {ex.StackTrace}", Json = "", Database = "", Table = "", Module = "" });
                            _context.SaveChanges();
                        }
                    }

                    proc.IsRunning = false;
                    _context.SaveChanges();

                    var lastmsg = _context.SystemLogs.OrderByDescending(x => x.Id).Select(x => x.Task).FirstOrDefault();
                    if (lastmsg.ToLower().Contains("upload_end"))
                    {
                        _context.SystemLogs.Add(new DomainLayer.Models.SystemLog { Task = "Crystal Extract End", CreateDate = DateTime.Now, ApiUrl = $@"", Json = "", ErrorMsg = "THIS IS INFORMATION ONLY" });
                        _context.SaveChanges();
                    }

                }

            }
            return ret;
        }

     }
}
