using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestOfWebServices;
using System.Linq.Dynamic;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class SektorController : Controller
    {
        /// <summary>
        /// Metoda koja prikazuje glavni view i sam JTable
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///   Metoda za izlistavanje svih sektora u bazi
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
                //podaci iz baze
                using (var context = new LMContext())
                {
                    var sektor = context.Sektors.Select(x => new
                    {
                        x.SektorID,
                        x.Naziv
                    });
                    var count = sektor.Count();
                    var records = sektor.OrderBy(jtSorting).Skip(jtStartIndex).Take(jtPageSize).ToList();


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
        /// Metoda za dodavanje novih sektora u bazu
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create(SektorViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                using (var context = new LMContext())
                {
                    var noviSektor = new Sektor
                    {
                        Naziv = viewModel.Naziv
                    };

                    context.Sektors.Add(noviSektor);
                    context.SaveChanges();

                    return Json(new { Result = "OK", Record = noviSektor });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        /// <summary>
        /// Metoda za izmjenu postojecih sektora u bazi
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(SektorViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                using (var context = new LMContext())
                {
                    var sektorEdit = context.Sektors.Find(viewModel.SektorID);
                    sektorEdit.SektorID = viewModel.SektorID;
                    sektorEdit.Naziv = viewModel.Naziv;


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
        /// Metoda za brisanje odabranih sektora iz baze
        /// </summary>
        /// <param name="SektorID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int SektorID)
        {
            try
            {
                using (var context = new LMContext())
                {
                    var brisaniSektor = context.Sektors.Find(SektorID);
                    context.Sektors.Remove(brisaniSektor);
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
        /// Metoda za dobijanje dropdown liste za sektor
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSektorOptions()
        {
            try
            {
                using (var db = new LMContext())
                {
                    var sektor = db.Sektors.Select(c => new { DisplayText = c.Naziv, Value = c.SektorID }).ToList();
                    return Json(new { Result = "OK", Options = sektor });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}