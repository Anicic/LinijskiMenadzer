using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    class CustomAutorizeAttribute : AuthorizeAttribute
    {
        string[] role;
        public CustomAutorizeAttribute(string rola)
        {
            role = rola.Split(',');
        }
        public CustomAutorizeAttribute()
        {

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            using (var context = new LMContext())
            {
                string UserName = httpContext.User.Identity.Name;
                UserName = UserName.Replace("LANACO\\", "");
                List<string> roleRadnika = new List<string>();
                bool PostojiRolaRadnikaURolama = false;
                try
                {
                    var UserID = context.Users.FirstOrDefault(m => m.UserName == UserName).UserID;
                    //var UserID = 5;
                    var RadnikID = context.RadnikUsers.FirstOrDefault(m => m.UserID == UserID).RadnikID;
                    //var RadnikID = 6;
                    roleRadnika = context.RadnikRolas.Where(m => m.RadnikID == RadnikID).Select(m1 => m1.Rola.Naziv).ToList();
                    if (roleRadnika.Count == 0)
                        return false;
                }
                catch
                {
                    return false;
                }

                if (role == null)
                {
                    if (roleRadnika.Count() != 1)
                    {
                        var role = context.Rolas.Select(m => m.Naziv).ToList();

                         PostojiRolaRadnikaURolama = role.Any(r => roleRadnika.Contains(r));
                        if (PostojiRolaRadnikaURolama)
                            return true;
                        return false;
                    }
                    else
                    {
                        var role = context.Rolas.Any(m => m.Naziv == roleRadnika.ElementAt(0));
                        if (role)
                            return true;

                        return false;
                    }

                }
                 PostojiRolaRadnikaURolama = role.Any(r => roleRadnika.Contains(r));
                if (PostojiRolaRadnikaURolama)
                    return true;
                return false;
            }

        }
    }
}
