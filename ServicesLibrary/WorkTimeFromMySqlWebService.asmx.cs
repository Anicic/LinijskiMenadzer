//using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebApplication.Models;

namespace MyWebService
{
    /// <summary>
    /// Summary description for UserFromMySqlWebService
    /// </summary>
    [WebService(Namespace = "http://tempuril.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UserFromMySqlWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// Metoda GetAllWorkTimes kupi sva radna vremena 
        /// </summary>
       [WebMethod]
        public List<WorkTimeViewModel > GetAllWorkTimes()
        {
            //using (var context = new KontrolaProlazaContext()) {

            //    var works = context.WorkTimes.Select(w => new WorkTimeViewModel
            //    {
            //        WorkTimeID = w.WorkTimeID,
            //        UserID = w.UserID,
            //        InTime = w.InTime,
            //        OutTime = w.OutTime,
            //        InTimeText = w.InTimeText,
            //        OutTimeText = w.OutTimeText,
            //        StartDate = w.StartDate
            //    }).ToList();

            //    return works;
            // }
            return null;
        }
    }
}
