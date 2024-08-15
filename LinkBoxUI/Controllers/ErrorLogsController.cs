using DocumentFormat.OpenXml.EMMA;
using DomainLayer.ViewModels;
using LinkBoxUI.Context;
using LinkBoxUI.SessionChecker;
using StructureMap.Query;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace LinkBoxUI.Controllers
{
    [SessionCheck]
    public class ErrorLogsController : Controller
    {
        // GET: ErrorLogs
        LinkboxDb db = new LinkboxDb();
        public ActionResult Index()
        {
            ErrorLogsViewModel model = new ErrorLogsViewModel();
            //x.CreateDate > DateTime.Today

            model.ErrorList = db.SystemLogs.Where(x=>x.Task.ToLower().Contains("UPLOAD_ERROR"))
            //model.ErrorList = db.SystemLogs.Where(x => x.ErrorMsg.ToLower().Contains($@"""error"" :") || x.ErrorMsg.ToLower().Contains($@"error :") || x.ErrorMsg.ToLower().Contains("error:"))
                .AsEnumerable().Select(x => new ErrorLogsViewModel.ErrorsViewModel
                {
                    id = x.Id,
                    Task = x.Task,
                    ErrorMsg = x.ErrorMsg,
                    Database = x.Database,
                    Module = x.Module,
                    Table = x.Table,
                    CreateDate = x.CreateDate,


                }).OrderByDescending(x => x.id).ThenByDescending(x => x.id).ToList();
            //RemoveString(model);

            return View(model);
        }
        //public ErrorLogsViewModel RemoveString(ErrorLogsViewModel modelz)
        //{
            

        //    foreach (var ErrorList in modelz.ErrorList)
        //    {
        //        //ErrorLogsViewModel modelz = new ErrorLogsViewModel;

        //        string errorMessage = ErrorList.ErrorMsg.ToString();
                
        //        string pattern = @"\[([^]]+)\]\[line: (\d+)\] , '([^']+)\'";

        //        // Using regular expression to find the match
        //        Match match = Regex.Match(errorMessage, pattern);


        //        if (match.Success)
        //        {
        //            string matchedString = match.Value; // Extract the entire matched substring

        //            ErrorList.ErrorMsg = matchedString;

        //            //modelz.E(tabledata).State = EntityState.Modified;
        //            //_context.SaveChanges();
        //            //Console.WriteLine("Matched String: " + matchedString);
        //        }
        //        else
        //        {
        //            Console.WriteLine("No match found.");
        //        }


        //    }

        //    return modelz;
        //}

    }


}