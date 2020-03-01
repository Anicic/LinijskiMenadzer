using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestOfWebServices;
using WebApplication.Models;
using System.Linq.Dynamic;


namespace WebApplication.Controllers
{
    public class RadnikRolaController : Controller
    {
        /// <summary>
        /// Metoda za prikaz View Modela
        /// </summary>
        /// <returns></returns>
        [CustomAutorizeAttribute("admin,tim lider")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Metoda za izlistavanje svih RadnikRola iz baze
        /// </summary>
        /// <param name="jtStartIndex"></param>
        /// <param name="jtPageSize"></param>
        /// <param name="jtSorting"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var context = new LMContext())
                {

                    var role = context.RadnikRolas.Select(r => new
                    {
                        RadnikRolaID = r.RadnikRolaID,
                        RadnikID = r.RadnikID,
                        RolaID = r.RolaID
                    });
                    var count = role.Count();
                    var records = role.OrderBy(jtSorting).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = count });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Metoda za kreiranje novih RadnikRola u bazi
        /// </summary>
        /// <param name="rola"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult Create(RadnikRolaViewModel rola)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                using (var context = new LMContext())
                {
                    context.RadnikRolas.Add(new RadnikRola
                    {
                        RadnikRolaID = rola.RadnikRolaID,
                        RadnikID = rola.RadnikID,
                        RolaID = rola.RolaID
                    });

                    context.SaveChanges();

                    return Json(new { Result = "OK", Record = rola });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Metoda za izmjene RadnikRola u bazi
        /// </summary>
        /// <param name="rola"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(RadnikRolaViewModel rola)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                using (var context = new LMContext())
                {
                    var rolaPom = context.RadnikRolas.Find(rola.RadnikRolaID);

                    rolaPom.RadnikRolaID = rola.RadnikRolaID;
                    rolaPom.RadnikID = rola.RadnikID;
                    rolaPom.RolaID = rola.RolaID;

                    context.SaveChanges();
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Metoda za brisanje RadnikRola iz baze
        /// </summary>
        /// <param name="radnikRolaId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int radnikRolaId)
        {
            try
            {
                using (var context = new LMContext())
                {
                    context.RadnikRolas.Remove(context.RadnikRolas.Find(radnikRolaId));
                    context.SaveChanges();
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Metoda za izlistavanje dropdown liste za sve role u bazi
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRola()
        {
            try
            {
                using (var context = new LMContext())
                {
                    var role = context.Rolas.Select(r => new { DisplayText = r.Naziv, Value = r.RolaID }).ToList();
                    return Json(new { Result = "OK", Options = role });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Metoda za izlistavanje dropdown liste za sve radnike u bazi
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRadnik()
        {
            try
            {
                using (var context = new LMContext())
                {
                    var radnici = context.Radniks.Select(c => new { DisplayText = c.Ime + " " + c.Prezime, Value = c.RadnikID }).ToList();
                    return Json(new { Result = "OK", Options = radnici });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}