using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApplication.Controllers;

namespace WebApplication.HelperClass
{
    public class WindowsRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            // Provjera rola ulogovanog korisnika
            using (var context = new LMContext())
            {
                string UserName = username;
                UserName = UserName.Replace("LANACO\\", "");
                List<string> rola = new List<string>();
                string[] DozvoljeneRole;
                try
                {
                    var UserID = context.Users.FirstOrDefault(m => m.UserName == UserName).UserID;
                    var RadnikID = context.RadnikUsers.FirstOrDefault(m => m.UserID == UserID).RadnikID;
                    var role = context.RadnikRolas.Where(m => m.RadnikID == RadnikID).Select(m1 => m1.Rola.Naziv).ToList();
                    rola.AddRange(role);
                    DozvoljeneRole = roleName.Split(',');
                }
                catch
                {
                    return false;
                }
                if (DozvoljeneRole.Length == 1 && rola.Count == 1)
                {
                    if (rola.ElementAt(0) == roleName)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    foreach(var item in DozvoljeneRole)
                    {
                        foreach (var value in rola)
                        {
                            if (item == value)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }


        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}