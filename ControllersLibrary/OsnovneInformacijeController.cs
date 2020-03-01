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
    public class OsnovneInformacijeController : Controller
    {
        /// <summary>
        /// Metoda za glavni view u kome se nalazi GrupaRadnik jTable
        /// </summary>
        /// <returns></returns>
        [CustomAutorizeAttribute("admin,menadžer,radnik,tim lider")]
        public ActionResult Index()
        {
            ViewBag.Tipovi = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Jednodnevno", Value = "1"},
                new SelectListItem(){ Text = "Visednevno", Value = "2"}
            };
            return View();
        }

        public PartialViewResult _VratiJednodnevniIzgled()
        {
            return PartialView("_VratiJednodnevniIzgled");
        }

        public PartialViewResult _VratiVisednevniIzgled()
        {
            return PartialView("_VratiVisednevniIzgled");
        }

        [HttpGet]
        public JsonResult VratiRadnoVrijeme(DateTime Datum)
        {
            using (LMContext context = new LMContext())
            {

                string UserName = User.Identity.Name;
                UserName = UserName.Replace("LANACO\\", "");
                var UserID = context.Users.FirstOrDefault(m => m.UserName == UserName).UserID;
                var RadnikID = context.RadnikUsers.FirstOrDefault(m => m.UserID == UserID).RadnikID;

                var AktivnostiUJednomDanu = context.WorkTimes.Where(m => m.UserID == UserID && m.StartDate == Datum).ToList();
                var RadnoVrijeme = AktivnostiUJednomDanu.FirstOrDefault(m => m.IzlazID == 4) as WorkTime;

                double totalSecends = 0;
                TimeSpan? span = new TimeSpan();

                if (RadnoVrijeme == null)
                {
                    //nije se uopste ulogovao na posao
                    span = TimeSpan.Parse("00:00:00");
                }
                else
                {
                    if (RadnoVrijeme.OutTime == null)
                    {
                        TimeSpan trenutnoVrijeme = DateTime.Now.TimeOfDay;
                        span = trenutnoVrijeme - RadnoVrijeme.InTime;
                    }
                    else
                    {
                        span = RadnoVrijeme.OutTime - RadnoVrijeme.InTime;
                    }

                    PocetnaController pocetnaController = new PocetnaController();

                    TimeSpan? privatnoOdsustvo = pocetnaController.UkupnoVrijemeZaPrivatnoOdsustvo(Datum, Convert.ToInt32(RadnikID));
                    if (privatnoOdsustvo != null)
                        span -= privatnoOdsustvo;

                }

                totalSecends += ((TimeSpan)span).TotalSeconds;

                TimeSpan t = TimeSpan.FromSeconds(totalSecends);



                string VrijemeKojeJeRadioUlogovaniKorisnik = string.Format("{0:D2}:{1:D2}:{2:D2}",
                t.Hours,
                t.Minutes,
                t.Seconds);
                int ProcenatRedovnogRada = (int)Math.Round((double)(100 * totalSecends) / 28800);
                if (ProcenatRedovnogRada > 100)
                    ProcenatRedovnogRada = 100;
                return Json(ProcenatRedovnogRada, JsonRequestBehavior.AllowGet);
            }
        }
    }
}