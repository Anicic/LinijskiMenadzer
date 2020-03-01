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
    public class ObavjestenjaController : Controller
    {
        [HttpGet]
        [CustomAutorizeAttribute("admin,menadžer,radnik,tim lider")]
        public ActionResult Index()
        {
            using (var context = new LMContext())
            {
                var tipObavjestenja = new List<SelectListItem>()
                {
                    new SelectListItem(){Text = "Status", Value = "0"},
                    new SelectListItem(){Text = "Novo", Value = "1"},
                    new SelectListItem(){Text = "Odobreno", Value = "2"},
                    new SelectListItem(){Text = "Odbijeno", Value = "3"},
                    new SelectListItem(){Text = "Na cekanju", Value = "4"},
                    new SelectListItem(){Text = "Informacija", Value = "5"},
                };

                ViewBag.TipObavjestenja = tipObavjestenja;
            }
            return View();   
        }

        [HttpGet]
        public PartialViewResult _KreirajNoviZahtjev()
        {
            return PartialView("_KreirajNoviZahtjev");
        }

        [ValidateInput(false)]
        [HttpPost]
        public PartialViewResult _KreirajNoviZahtjev(SadrzajViewModel viewModel)
        {
            using (var context = new LMContext())
            {
                var user = context.RadnikUsers.FirstOrDefault(x => x.Radnik.DomenskoIme == User.Identity.Name)?.RadnikID;

                var noviZahtjev = new Obavjestenje
                {
                    PosiljalacID = (short)user,
                    TipObavjestenjaID = viewModel.TipID,
                    DatumObavjestenja = DateTime.Now,
                    Odobreno = null,
                    Pregledano = false,
                    PrimalacID = viewModel.PrimalacID
                };

                var SadrzajObavjestenja = new SadrzajObavjestenja();
                
                if (noviZahtjev.TipObavjestenjaID == 1)
                {

                    SadrzajObavjestenja = new SadrzajObavjestenja
                    {
                        DatumOd = viewModel.DatumOd,
                        TextObavjestenja = viewModel.TextObavjestenja
                    };

                }else if(noviZahtjev.TipObavjestenjaID == 2 || noviZahtjev.TipObavjestenjaID == 3)
                {
                    SadrzajObavjestenja = new SadrzajObavjestenja
                    {
                        DatumOd = viewModel.DatumOd,
                        DatumDo = viewModel.DatumDo,
                        RadnikID = viewModel.RadnikID,
                        TextObavjestenja = viewModel.TextObavjestenja
                    };
                }

                noviZahtjev.SadrzajObavjestenja = SadrzajObavjestenja;

                context.Obavjestenjes.Add(noviZahtjev);
                context.SaveChanges();

            }
            return null;
        }

        public PartialViewResult _VratiObavjestenja(bool IspisiSvaObavjestenja, byte StatusID)
        {
            using (var context = new LMContext())
            {
                var RadnikID = Controllers.PocetnaController.PronadjiIDUlogovanogRadnika();

                var Obavjestenja = context.Obavjestenjes.Where(m => m.PrimalacID == RadnikID && (IspisiSvaObavjestenja == true || (IspisiSvaObavjestenja == false && (m.Pregledano == false || m.Pregledano == null)))).Select(m1 => new ObavjestenjeViewModel
                {
                    ObavjestenjeID = m1.ObavjestenjeID,
                    ObavjestenjeNaziv = m1.TipObavjestenja.Naziv,
                    Odobreno = m1.Odobreno,
                    PosiljalacIme = m1.Posiljalac.Ime + " " + m1.Posiljalac.Prezime,
                    PrimalacIme = m1.Primalac.Ime + " " + m1.Primalac.Prezime,
                    Pregledano = m1.Pregledano,
                    PosiljalacID = m1.PosiljalacID,
                    PrimalacID = m1.PrimalacID,
                    SadrzajObavjestenja = m1.SadrzajObavjestenja.TextObavjestenja,
                    TipObavjestenjaID = m1.TipObavjestenjaID,
                    DatumObavjestenja = m1.DatumObavjestenja.Value,             
                    DatumOdKad = m1.SadrzajObavjestenja.DatumOd,
                    DatumDoKad = m1.SadrzajObavjestenja.DatumDo,
                    OpisObavjestenja = m1.TipObavjestenja.OpisObavjestenja,
                    ImeIPrezimeRadnika = (m1.SadrzajObavjestenja.Radnik == null ? "" : m1.SadrzajObavjestenja.Radnik.Ime + " " + m1.SadrzajObavjestenja.Radnik.Prezime)
                }).OrderByDescending(n => n.ObavjestenjeID).ToList();

                if(StatusID != 0)
                {
                    Obavjestenja = Obavjestenja.Where(ob => (StatusID == 1 && ob.Pregledano != true) || (StatusID == 2 && ob.Odobreno == true) || (StatusID == 3 && ob.Odobreno == false) ||
                                    (StatusID == 4 && ob.Odobreno == null && ob.Pregledano == true) || (StatusID == 5 && ob.Pregledano != true)).ToList();
                }

                foreach (var obavjestenje in Obavjestenja)
                {
                    obavjestenje.OpisObavjestenja = IzmijeniTekst(obavjestenje);
                }

                return PartialView("_VratiObavjestenja", Obavjestenja);
            }
        }

        public JsonResult Last5notifications()
        {
            var RadnikID = Controllers.PocetnaController.PronadjiIDUlogovanogRadnika();
            using (var context = new LMContext())
            {
                var Obavjestenja = context.Obavjestenjes.Where(m => m.PrimalacID == RadnikID).Select(m1 => new ObavjestenjeViewModel
                {
                    ObavjestenjeID = m1.ObavjestenjeID,
                    ObavjestenjeNaziv = m1.TipObavjestenja.Naziv,
                    Odobreno = m1.Odobreno,
                    PosiljalacIme = m1.Posiljalac.Ime + " " + m1.Posiljalac.Prezime,
                    PrimalacIme = m1.Primalac.Ime + " " + m1.Primalac.Prezime,
                    Pregledano = m1.Pregledano,
                    PosiljalacID = m1.PosiljalacID,
                    PrimalacID = m1.PrimalacID,
                    SadrzajObavjestenja = m1.SadrzajObavjestenja.TextObavjestenja,
                    TipObavjestenjaID = m1.TipObavjestenjaID,
                    DatumObavjestenja = m1.DatumObavjestenja.Value,             // m1.DatumObavjestenja kada se doda u bazu

                }).OrderByDescending(n => n.ObavjestenjeID).Take(5).ToList();
                foreach (var item in Obavjestenja)
                {
                    item.DatumObavjestenjaString = item.DatumObavjestenja.ToString("dd.MM.yyyy");
                }

                return Json(Obavjestenja, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult VratiObavjestenje(int id)
        {
            using (var context = new LMContext())
            {
                var Obavjestenje = context.Obavjestenjes.Find(id);
                var ObavjestenjeViewModel = new ObavjestenjeViewModel
                {
                    ObavjestenjeID = Obavjestenje.ObavjestenjeID,
                    ObavjestenjeNaziv = Obavjestenje.TipObavjestenja.Naziv,
                    Odobreno = Obavjestenje.Odobreno,
                    PosiljalacIme = Obavjestenje.Posiljalac.Ime + " " + Obavjestenje.Posiljalac.Prezime,
                    PrimalacIme = Obavjestenje.Primalac.Ime + " " + Obavjestenje.Primalac.Ime,
                    Pregledano = Obavjestenje.Pregledano,
                    PosiljalacID = Obavjestenje.PosiljalacID,
                    DatumObavjestenja = Obavjestenje.DatumObavjestenja ?? DateTime.Now,
                    PrimalacID = Obavjestenje.PrimalacID,
                    SadrzajObavjestenja = Obavjestenje.SadrzajObavjestenja.TextObavjestenja,
                    TipObavjestenjaID = Obavjestenje.TipObavjestenjaID
                };
                ObavjestenjeViewModel.DatumObavjestenjaString = ObavjestenjeViewModel.DatumObavjestenja.ToString("dd.MM.yyyy");

                return Json(ObavjestenjeViewModel, JsonRequestBehavior.AllowGet);
            }
        }

        private string IzmijeniTekst(ObavjestenjeViewModel viewModel)
        {
            using (var context = new LMContext())
            {
                string PromijenjenoObavjestenje = "";

                string[] obavjestenje = viewModel.OpisObavjestenja.Split(' ');

                foreach (var rijec in obavjestenje)
                {
                    string novaRijec = rijec;

                    switch (rijec)
                    {
                        case "@posiljaoc":
                            novaRijec = "<span style=" + "color:#00537a;font-weight:bold;" + ">" + viewModel.PosiljalacIme + "</span>";
                            break;
                        case "@primalac":
                            novaRijec = "<span style=" + "color:#00537a;font-weight:bold;" + ">" + viewModel.PrimalacIme + "</span>";
                            break;
                        case "@datum":
                            novaRijec = "<span style=" + "color:#00537a;font-weight:bold;" + ">" + String.Format("{0:dd/MM/yyyy}", viewModel.DatumOdKad) + "</span>";
                            break;
                        case "@period":
                            novaRijec = "od " + "<span style=" + "color:#00537a;font-weight:bold;" + ">" + String.Format("{0:dd/MM/yyyy}", viewModel.DatumOdKad) + "</span>" + " do " +
                                        "<span style=" + "color:#00537a;font-weight:bold;" + ">" + String.Format("{0:dd/MM/yyyy}", viewModel.DatumDoKad) + "</span>";
                            break;
                        case "@radnik":
                            novaRijec = "<span style=" + "color:#00537a;font-weight:bold;" + ">" + viewModel.ImeIPrezimeRadnika + "</span>";
                            break;
                    }
                    PromijenjenoObavjestenje += novaRijec + " ";
                }

                return PromijenjenoObavjestenje;
            }
        }

        public PartialViewResult _PregledObavjestenja(int obavjestenjeID)
        {
            using (var context = new LMContext())
            {

                var obavjestenje = context.Obavjestenjes.Find(obavjestenjeID);
                var obavjestenjeViewModel = new ObavjestenjeViewModel()
                {
                    ObavjestenjeID = obavjestenje.ObavjestenjeID,
                    ObavjestenjeNaziv = obavjestenje.TipObavjestenja.Naziv,
                    Odobreno = obavjestenje.Odobreno,
                    PosiljalacIme = obavjestenje.Posiljalac.Ime + " " + obavjestenje.Posiljalac.Prezime,
                    PrimalacIme = obavjestenje.Primalac.Ime + " " + obavjestenje.Primalac.Prezime,
                    Pregledano = obavjestenje.Pregledano,
                    PosiljalacID = obavjestenje.PosiljalacID,
                    PrimalacID = obavjestenje.PrimalacID,
                    SadrzajObavjestenja = obavjestenje.SadrzajObavjestenja.TextObavjestenja,
                    TipObavjestenjaID = obavjestenje.TipObavjestenjaID,
                    DatumObavjestenja = DateTime.Now,             // m1.DatumObavjestenja kada se doda u bazu
                    DatumOdKad = obavjestenje.SadrzajObavjestenja.DatumOd,
                    DatumDoKad = obavjestenje.SadrzajObavjestenja.DatumDo,
                    OpisObavjestenja = obavjestenje.TipObavjestenja.OpisObavjestenja,
                    ImeIPrezimeRadnika = (obavjestenje.SadrzajObavjestenja.Radnik == null ? "" : obavjestenje.SadrzajObavjestenja.Radnik.Ime + " " + obavjestenje.SadrzajObavjestenja.Radnik.Prezime),
                    Email = obavjestenje.Posiljalac.EmailAdresa,
                    BrojTelefona = obavjestenje.Posiljalac.BrojTelefona
                };

                obavjestenjeViewModel.OpisObavjestenja = IzmijeniTekst(obavjestenjeViewModel);

                return PartialView("_PregledObavjestenja", obavjestenjeViewModel);
            }
        }

        /// <summary>
        /// Kada korisnik otvori obavjestenje ono automatski postaje procitano
        /// </summary>
        /// <param name="obavjestenjeID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ProcitanoObavjestenje(int obavjestenjeID)
        {
            using (var context = new LMContext())
            {
                var obavjestenje = context.Obavjestenjes.Find(obavjestenjeID);

                if (obavjestenje.Pregledano == false || obavjestenje.Pregledano == null)
                {
                    obavjestenje.Pregledano = true;
                    context.SaveChanges();
                    return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { Result = "ERROR" }, JsonRequestBehavior.AllowGet);

            }
        }

        /// <summary>
        /// metoda koja kupi informacije sa Obrazlozenje i cuva ga u bazu
        /// </summary>
        /// <param name="obavjestenjeID"></param>
        /// <param name="odobri"></param>
        /// <param name="imaObrazlozenje"></param>
        /// <param name="obrazlozenje"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpGet]
        public JsonResult RjesavanjeZahtjeva(int obavjestenjeID, bool odobri, bool imaObrazlozenje, SadrzajObavjestenja sadrzajObavjestenja)
        {
            try
            {
                using (var context = new LMContext())
                {
                    var obavjestenje = context.Obavjestenjes.Find(obavjestenjeID);

                    obavjestenje.Odobreno = odobri;

                    if (imaObrazlozenje)
                       obavjestenje.Odgovor = sadrzajObavjestenja.TextObavjestenja;

                    Obavjestenje novoObavjestenje = new Obavjestenje()
                    {
                        DatumObavjestenja = DateTime.Now,
                        PosiljalacID = obavjestenje.PrimalacID,
                        PrimalacID = obavjestenje.PosiljalacID,
                        TipObavjestenjaID = 4,
                        SadrzajObavjestenja = new SadrzajObavjestenja()
                        {
                            TextObavjestenja = sadrzajObavjestenja.TextObavjestenja
                        },
                        Odobreno = odobri
                    };
                    context.Obavjestenjes.Add(novoObavjestenje);

                    context.SaveChanges();

                    return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metoda koja nakon obrazlozenja salje povratnu poruku posiljaocu o odobrenju/odbijanju njegovog zahtjeva
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UzvratiPoruku()
        {
            try
            {
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult VratiBrojNeprocitanihObavjestenja()
        {
            using(var context = new LMContext())
            {

                var RadnikID = Controllers.PocetnaController.PronadjiIDUlogovanogRadnika();

                var radnik = context.Radniks.Find(RadnikID);

                var brojNeprocitanihObavjestenja = radnik.PrimljenaObavjestenje?.Where(o => o.Pregledano != true).Count();
                //var brojNeprocitanihObavjestenja = 0;
                return Json(new { brojNeprocitanihObavjestenja }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metoda koja vrsi pretragu radnika on keyUp
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


    }

}