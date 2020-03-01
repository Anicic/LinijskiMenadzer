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
    public class GrupaRadnikController : Controller
    {
        /// <summary>
        /// Metoda za glavni view u kome se nalazi GrupaRadnik jTable
        /// </summary>
        /// <returns></returns>
        [CustomAutorizeAttribute("admin,tim lider")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Metoda za izlistavanje svih grupa radnika
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

                    var list = context.GrupaRadniks.Select(m => new GrupaRadnikViewModel
                    {
                        GrupaRadnikID = m.GrupaRadnikID,
                        GrupaID = m.GrupaID,
                        RadnikID = m.RadnikID
                        
                    });
                    int broj = list.Count();

                    var records = list.OrderBy(jtSorting).Skip(jtStartIndex).Take(jtPageSize).ToList();

                    return Json(new { Result = "OK", Records = records, TotalRecordCount = broj });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Metoda za kreiranje grupa radnika u jTable-u
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create(GrupaRadnikViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Korisnicko ime je zauzeto!" });
                }

                using (var context = new LMContext())
                {
                    var model = new GrupaRadnik
                    {
                        GrupaRadnikID = viewModel.GrupaRadnikID,
                        RadnikID = viewModel.RadnikID,
                        GrupaID = viewModel.GrupaID
                    };
                    context.GrupaRadniks.Add(model);
                    context.SaveChanges();
                    return Json(new { Result = "OK", Record = model });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        /// <summary>
        /// Metoda za izmjenu podataka o grupama radnika u jTable-u
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(GrupaRadnikViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                using (var context = new LMContext())
                {
                    var model = context.GrupaRadniks.Find(viewModel.GrupaRadnikID);
                    model.GrupaID = viewModel.GrupaID;
                    model.RadnikID = viewModel.RadnikID;
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
        /// Metoda za brisanje veze izmedju grupa i radnika
        /// </summary>
        /// <param name="grupaRadnikId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int grupaRadnikId)
        {
            try
            {
                using (var context = new LMContext())
                {
                    var model = context.GrupaRadniks.Find(grupaRadnikId);
                    context.GrupaRadniks.Remove(model);
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
        /// Metoda za izlistavanje dropdown liste za sve grupe u bazi
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetGrupe()
        {
            try
            {
                using (var context = new LMContext())
                {
                    var grupe = context.Grupes.Select(c => new { DisplayText = c.Naziv, Value = c.GrupaID }).ToList();
                    return Json(new { Result = "OK", Options = grupe });
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
        public JsonResult GetRadnike()
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