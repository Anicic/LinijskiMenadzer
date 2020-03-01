using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Controllers;
using WebApplication.Models;

namespace WebApplication.HelperClass
{
    public class ImeIPrezimeLogovanogRadnika
    {
       public static string ImeRadnika()
       {
            using(var context =new LMContext())
            {
                string UserName = System.Web.HttpContext.Current.User.Identity.Name;
                UserName = UserName.Replace("LANACO\\", "");
                var UserID = context.Users.FirstOrDefault(m => m.UserName == UserName).UserID;
                var RadnikID = context.RadnikUsers.FirstOrDefault(m => m.UserID == UserID).RadnikID;
                var ImeRadnika = context.RadnikUsers.Where(r => r.RadnikID == RadnikID).Select(r => new
                {
                    ImeIPrezime = r.Radnik.Ime + " " + r.Radnik.Prezime
                }).ToList();
                string Ime = Convert.ToString(ImeRadnika[0].ImeIPrezime);

                return Ime;
            }    
       }
    }
}