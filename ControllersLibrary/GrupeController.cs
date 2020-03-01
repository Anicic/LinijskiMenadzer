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
    public class GrupeController : Controller
    {
        [CustomAutorizeAttribute("admin,tim lider")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lista grupa iz baze
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
                //Get data from database
                using (var context = new LMContext())
                {
                    var grupe = context.Grupes.Select(o => new GrupeViewModel
                    {
                        GrupaID = o.GrupaID,
                        Naziv = o.Naziv
                    });
                    var count = grupe.Count();
                    var records = grupe.OrderBy(jtSorting).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new
                    {
                        Result = "OK",
                        Records = records,
                        TotalRecordCount = count
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Kreiranje nove grupe
        /// </summary>
        /// <param name="grupe"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create(GrupeViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                using (var context = new LMContext())
                {
                    Grupe model = new Grupe
                    {
                        Naziv = viewModel.Naziv,

                    };
                    context.Grupes.Add(model);
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
        /// Update grupe
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(GrupeViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                using (var context = new LMContext())
                {
                    Grupe model = context.Grupes.Find(viewModel.GrupaID);
                    model.Naziv = viewModel.Naziv;
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
        /// Delete grupe
        /// </summary>
        /// <param name="GrupaID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int GrupaID)
        {
            try
            {
                using (var context = new LMContext())
                {
                    Grupe model = context.Grupes.Find(GrupaID);
                    context.Grupes.Remove(model);
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
        /// Dobijanje tim lidera iz baze
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetLider()
        {
            try
            {
                using (var context = new LMContext())
                {
                    var lideri = context.Radniks.Select(r => new
                    {
                        Value = r.RadnikID,
                        DisplayText = r.Ime + " " + r.Prezime
                    }).ToList();

                    return Json(new { Result = "OK", Options = lideri });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}