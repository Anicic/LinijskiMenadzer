//using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebApplication.Models;

namespace TestOfWebServices
{

    /// <summary>
    /// Summary description for UserFromMySqlWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
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
        /// Funkcija koja uzima sve korisnike iz baze
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<UserViewModel> GetAllUsers()
        {
            //using(var context = new KontrolaProlazaContext())
            //{
            //    var userList = context.Users.Select(x => new UserViewModel
            //    {
            //        UserID = x.UserID,
            //        UserName = x.UserName,
            //        Password = x.Password

            //    }).ToList();

            //    return userList;
            //}
            return null;
        }


        /*public DataTable GetUsers()
        {
            string connectionString = @"Server = localhost;Database = demo; Uid = savke; Pwd = 123;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM `users`", conn);
                mySqlCommand.CommandType = CommandType.Text;

                DataTable dataTable = new DataTable();
                DataView dataView = new DataView();
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                dataTable.Load(mySqlDataReader);

                conn.Close();

                return dataTable;
            }

        }*/

    }
}
