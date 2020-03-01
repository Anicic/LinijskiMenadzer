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
    public class GrupaRadnik1Controller : Controller
    {

        [HttpGet]
        [CustomAutorizeAttribute("admin,menadžer")]
        public ActionResult Index()
        {

            ViewBag.TipGrupe = new List<SelectListItem> {
                new SelectListItem(){Text="Organizaciona", Value="1"},
                 new SelectListItem(){Text="Projektna", Value="2"}
             };
            return View();
        }

        /// <summary>
        /// Metoda za izlistavanje dropdown liste za sve grupe u bazi
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetGrupe(string Pretraga)
        {
            try
            {
                using (var context = new LMContext())
                {
                    var grupe = context.Grupes.Where(grupa => grupa.Naziv.Contains(Pretraga) || Pretraga == null)
                        .Select(c =>
                            new GrupaRadioButtonViewModel
                            {
                                NazivGrupe = c.Naziv,
                                GrupaID = c.GrupaID
                            }).ToList();

                    return Json(new { Data = grupe, Result = "OK" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metoda koja vraca listu radnika za selektovanu grupu
        /// </summary>
        /// <param name="GrupaID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult IspisiRadnikeGrupe(int GrupaID)
        {
            try
            {
                using (var context = new LMContext())
                {
                    var listaRadnika = context.GrupaRadniks.Where(m => m.GrupaID == GrupaID && m.DatumDo == null).Select(m1 => new RadnikViewModel
                    {
                        Ime = m1.Radnik.Ime,
                        Prezime = m1.Radnik.Prezime,
                        EmailAdresa = m1.Radnik.EmailAdresa,
                        RadnikID = m1.RadnikID
                    }).ToList();

                    return Json(new { Data = listaRadnika, Result = "OK" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// Metoda za izlistavanje dropdown liste za sve radnike u bazi
        /// </summary>
        /// <param name="Pretraga"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult PretraziRadnike(string Pretraga)
        {
            try
            {
                using (var context = new LMContext())
                {
                    var listaRadnika = context.Radniks.Where(radnik => ((radnik.Ime + " " + radnik.Prezime).Contains(Pretraga) || Pretraga == null)).Select(
                        r => new RadnikViewModel
                        {
                            Ime = r.Ime,
                            Prezime = r.Prezime,
                            EmailAdresa = r.EmailAdresa,
                            RadnikID = r.RadnikID
                        }).ToList();
                    return Json(new { Data = listaRadnika, Result = "OK" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Selektovanog radnika sa searchable dropdown liste dodaj u cekiranu grupu
        /// </summary>
        /// <param name="RadnikID"></param>
        /// <param name="GrupaID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult IspisiListu(int RadnikID, int GrupaID)
        {
            try
            {
                using (var context = new LMContext())
                {

                    if (!context.GrupaRadniks.Where(g => g.GrupaID == GrupaID && g.Radnik.RadnikID == RadnikID).Any(g1 => g1.DatumDo == null) )
                    {
                        var radnik = context.Radniks.Find(RadnikID);

                        var viewModel = new RadnikViewModel
                        {
                            Ime = radnik.Ime,
                            Prezime = radnik.Prezime,
                            EmailAdresa = radnik.EmailAdresa,
                            RadnikID = radnik.RadnikID
                        };

                        var grupaRadnik = new GrupaRadnik
                        {
                            GrupaID = (short)GrupaID,
                            RadnikID = (short)RadnikID,
                            DatumOd = DateTime.Today,
                            DatumDo = null,
                            Vođa = false
                        };
                        context.GrupaRadniks.Add(grupaRadnik);
                        context.SaveChanges();

                        return Json(new { Data = viewModel, Result = "OK" }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { Result = "ERROR" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult CreateNaziv(string naziv, string tip, DateTime? DatumKraj)
        {
            using (var context = new LMContext())
            {

                if (context.Grupes.Any(g => g.Naziv == naziv) || naziv.Trim() == "")
                {
                    return Json(new { Result = "ERROR", Message = "Unos nije validan!" }, JsonRequestBehavior.AllowGet);
                }

                context.Grupes.Add(new Grupe
                {
                    Naziv = naziv,
                    DatumOd = DateTime.Today,
                    DatumDo = DatumKraj,
                    TipGrupeID = context.TipGrupes.FirstOrDefault(tg => tg.Naziv == tip).TipGrupeID

                });
                context.SaveChanges();
            }
            return Json(new { Result = "OK", Message = "Uspjesno ste dodali grupu!" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Metoda koja uklanja radnika iz selektovane grupe
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult UkloniRadnika(short RadnikId, short GrupaId)
        {
            try
            {
                using (var context = new LMContext())
                {
                    var grupaRadnik = context.GrupaRadniks.FirstOrDefault(g => g.GrupaID == GrupaId && g.RadnikID == RadnikId && g.DatumDo==null);
                    grupaRadnik.DatumDo = DateTime.Today;
                    context.SaveChanges();
                }
                return Json(new { Result = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}