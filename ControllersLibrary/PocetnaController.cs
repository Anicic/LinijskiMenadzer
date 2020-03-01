using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestOfWebServices;
using WebApplication.Models;
using WebApplication;

namespace WebApplication.Controllers
{
    public class PocetnaController : Controller
    {


        public static int? PronadjiIDUlogovanogRadnika()
        {
            using (var context = new LMContext())
            {
                string UserName = System.Web.HttpContext.Current.User.Identity.Name;
                UserName = UserName.Replace("LANACO\\", "");
                var UserID = context.Users.FirstOrDefault(m => m.UserName == UserName).UserID;
                var RadnikID = context.RadnikUsers.FirstOrDefault(m => m.UserID == UserID).RadnikID;
                return RadnikID;
            }
        }

        #region Metode za status radnika 
        /// <summary>
        /// Metoda koja vraca StatusID u listu radnika za grupu -> u metodu _SviRadnici
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="datum"></param>
        /// <returns></returns>
        public int NadjiStatusID(short? UserID, DateTime datum)
        {
            using (var context = new LMContext())
            {
                try
                {
                    if (datum == DateTime.Today.Date)
                    {
                        var StatusID = context.WorkTimes.Where(m => m.UserID == UserID && m.StartDate == datum).Select(m1 => m1).OrderByDescending(m2 => m2.InTime).FirstOrDefault();
                        int izlazID = 4;

                        if (StatusID != null)
                        {
                            izlazID = StatusID.IzlazID == 1 ? 1 : StatusID.IzlazID == 2 ? 2 : StatusID.IzlazID == 3 ? 5 : StatusID.IzlazID == 4 ? 1 : 4;
                        }
                        else
                        {
                            izlazID = context.RadnikUsers.FirstOrDefault(r => r.UserID == UserID).Radnik.EvidencijaRadas.FirstOrDefault(ev => ev.Pocetak <= datum && ev.Kraj >= datum).TipID;
                        }

                        return izlazID;
                    }
                    else if (datum < DateTime.Today.Date)
                    {
                        var RadnikID = context.RadnikUsers.FirstOrDefault(m => m.UserID == UserID).RadnikID;
                        var StatusID = context.EvidencijaRadas.Where(m => m.RadnikID == RadnikID && m.Pocetak == datum).Select(m1 => m1).OrderByDescending(m2 => m2.Kraj).FirstOrDefault();
                        return StatusID.TipID;
                    }
                    return 4;
                }
                catch
                {
                    //Ako ne pronadje statusID u bazi, vraca 'nepoznato'
                    return 4;
                }

            }
        }
        /// <summary>
        /// Metoda koja vraca Naziv statusa u listu radnika za grupu -> u metodu _SviRadnici
        /// </summary>
        /// <param name="StatusID"></param>
        /// <returns></returns>
        public string NadjiNazivStatusa(int? StatusID)
        {
            if (StatusID == null)
            {
                return "Nepoznato";
            }
            using (var context = new LMContext())
            {
                try
                {
                    var NazivStatusa = context.Tips.FirstOrDefault(m => m.TipID == StatusID).Naziv;
                    return NazivStatusa;
                }
                catch
                {
                    return "Nepoznato";
                }
            }
        }
        #endregion

        #region Racunanje vremena po statusu na osnovu radnikID i odredjenog datuma

        public JsonResult VrijemeNaSluzbenomPutu(int? RadnikID, DateTime Datum)
        {
            int? UlogovaniRadnikID;
            if (RadnikID == 0)
                UlogovaniRadnikID = PronadjiIDUlogovanogRadnika();
            else
                UlogovaniRadnikID = RadnikID;
            using (var context = new LMContext())
            {
                var UserID = context.RadnikUsers.FirstOrDefault(m => m.RadnikID == UlogovaniRadnikID).UserID;
                var VrijemeProvedenoNaSluzbenomPutu = context.WorkTimes.Where(wt => wt.UserID == UserID && wt.StartDate == Datum.Date && wt.IzlazID == 3).Select(wt => new
                {
                    wt.InTime,
                    wt.OutTime,
                    wt.StartDate
                }).ToList();

                if (VrijemeProvedenoNaSluzbenomPutu.Count != 0)
                {

                    SluzbeniPutVrijemeViewModel viewModel = new SluzbeniPutVrijemeViewModel();
                    viewModel.ListaLabela = new List<string>();
                    viewModel.ListaMinutaNaSluzbenomPutu = new List<short>();
                    for (int i = 0; i < VrijemeProvedenoNaSluzbenomPutu.Count; i++)
                    {
                        TimeSpan? SluzbeniPutVrijeme = new TimeSpan();
                        if (VrijemeProvedenoNaSluzbenomPutu[i].OutTime != null)
                        {
                            SluzbeniPutVrijeme = VrijemeProvedenoNaSluzbenomPutu[i].OutTime - VrijemeProvedenoNaSluzbenomPutu[i].InTime;
                        }
                        else
                        {
                            DateTime date = DateTime.Now;
                            TimeSpan trenutnoVrijeme = new TimeSpan(date.Hour, date.Minute, date.Second);
                            if (VrijemeProvedenoNaSluzbenomPutu[i].StartDate == DateTime.Now.Date)
                            {
                                SluzbeniPutVrijeme = trenutnoVrijeme - VrijemeProvedenoNaSluzbenomPutu[i].InTime;
                            }
                            else
                            {
                                SluzbeniPutVrijeme = new TimeSpan(8, 0, 0);
                            }
                        }

                        short VrijemeSluzbeniPut = Convert.ToInt16(((TimeSpan)SluzbeniPutVrijeme).TotalMinutes);
                        if (VrijemeSluzbeniPut >= 480)
                        {
                            TimeSpan time = new TimeSpan(8, 0, 0);
                            viewModel.ListaMinutaNaSluzbenomPutu.Add(Convert.ToInt16(((TimeSpan)time).TotalMinutes));
                            viewModel.ListaLabela.Add("Sluzbeni put" + " " + (i + 1));
                        }
                        else
                        {
                            viewModel.ListaMinutaNaSluzbenomPutu.Add(VrijemeSluzbeniPut);
                            viewModel.ListaLabela.Add("Sluzbeni put" + " " + (i + 1));
                        }
                    }
                    return Json(viewModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var sluzbeniPut = context.EvidencijaRadas.FirstOrDefault(m => m.RadnikID == RadnikID && m.TipID == 5 && m.Pocetak <= Datum && m.Kraj >= Datum);
                    if (sluzbeniPut != null)
                    {
                        SluzbeniPutVrijemeViewModel viewModel = new SluzbeniPutVrijemeViewModel();
                        viewModel.ListaLabela = new List<string>();
                        viewModel.ListaMinutaNaSluzbenomPutu = new List<short>();
                        TimeSpan? SluzbeniPutVrijeme = new TimeSpan(8, 0, 0);
                        short VrijemeSluzbeniPut = Convert.ToInt16(((TimeSpan)SluzbeniPutVrijeme).TotalMinutes);
                        viewModel.ListaMinutaNaSluzbenomPutu.Add(VrijemeSluzbeniPut);
                        viewModel.ListaLabela.Add("Sluzbeni put");
                        return Json(viewModel, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new SluzbeniPut(), JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        public JsonResult VrijemeNaPrivatnomOdsustvu(int? RadnikID, DateTime Datum)
        {
            int? UlogovaniRadnikID;
            if (RadnikID == 0)
            {
                UlogovaniRadnikID = PronadjiIDUlogovanogRadnika();
            }
            else
            {
                UlogovaniRadnikID = RadnikID;
            }
            using (var context = new LMContext())
            {
                var UserID = context.RadnikUsers.FirstOrDefault(m => m.RadnikID == UlogovaniRadnikID).UserID;
                var VrijemeProvedenoNaPrivatnom = context.WorkTimes.Where(wt => wt.UserID == UserID && wt.StartDate == Datum && wt.IzlazID == 2).Select(wt => new
                {
                    wt.InTime,
                    wt.OutTime
                }).ToList();
                PrivatnoOdsustvoVrijemeViewModel viewModel = new PrivatnoOdsustvoVrijemeViewModel();
                viewModel.ListaLabela = new List<string>();
                viewModel.ListaMinutaNaPrivatnomOdsustvu = new List<short>();
                for (int i = 0; i < VrijemeProvedenoNaPrivatnom.Count; i++)
                {
                    TimeSpan? PrivatniPutVrijeme = VrijemeProvedenoNaPrivatnom[i].OutTime - VrijemeProvedenoNaPrivatnom[i].InTime;
                    short Vrijeme = Convert.ToInt16(((TimeSpan)PrivatniPutVrijeme).TotalMinutes);
                    viewModel.ListaLabela.Add("Privatno odsustvo" + " " + (i + 1));
                    viewModel.ListaMinutaNaPrivatnomOdsustvu.Add(Vrijeme);
                }
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult VrijemeProvedenoNaPauzi(int? RadnikID, DateTime Datum)
        {
            int? UlogovaniRadnikID;
            if (RadnikID == 0)
                UlogovaniRadnikID = PronadjiIDUlogovanogRadnika();
            else
                UlogovaniRadnikID = RadnikID;
            using (var context = new LMContext())
            {
                var UserID = context.RadnikUsers.FirstOrDefault(m => m.RadnikID == UlogovaniRadnikID).UserID;
                var VrijemeProvedenoNaPauzi = context.WorkTimes.Where(p => p.IzlazID == 1 && p.StartDate == Datum && p.UserID == UserID).Select(p => new
                {
                    p.InTime,
                    p.OutTime
                }).ToList();

                short pauze = 0;
                short pauzeDo = 0;
                short pauzePreko = 0;

                foreach (var item in VrijemeProvedenoNaPauzi)
                {
                    TimeSpan? VrijemeNaPauzi = item.OutTime - item.InTime;
                    pauze = Convert.ToInt16(((TimeSpan)VrijemeNaPauzi).TotalMinutes);
                }
                short DozvoljenoNaPauzi = context.VrijemePauzes.FirstOrDefault().PauzaMinute;
                if (pauze > DozvoljenoNaPauzi)
                {
                    pauzeDo = DozvoljenoNaPauzi;
                    pauzePreko = (short)(pauze - DozvoljenoNaPauzi);
                }
                else
                {
                    pauzeDo = pauze;
                }
                PauzaDijagramViewModel pauzaDijagramViewModel = new PauzaDijagramViewModel();
                pauzaDijagramViewModel.PauzeDo = pauzeDo;
                pauzaDijagramViewModel.PauzePreko = pauzePreko;
                return Json(pauzaDijagramViewModel, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EvidencijaRadaPoDatumu(short? RadnikID)
        {

            using (var context = new LMContext())
            {
                int? UlogovaniRadnikID;
                if (RadnikID == 0 || RadnikID == null)
                    UlogovaniRadnikID = PronadjiIDUlogovanogRadnika();
                else
                    UlogovaniRadnikID = RadnikID;
                var evidencijaRada = context.EvidencijaRadas.Where(r => r.RadnikID == UlogovaniRadnikID).ToList().Select(r => new EvidencijaRadaViewModel
                {
                    tipID = r.TipID,
                    kraj = r.Kraj.ToShortDateString(),
                    pocetak = r.Pocetak.ToShortDateString()
                }).ToList();

                foreach (var item in context.NeradniDanis)
                {
                    evidencijaRada.Add(new EvidencijaRadaViewModel() { tipID = 6, pocetak = item.NeradniDan.ToShortDateString(), kraj = item.NeradniDan.ToShortDateString() });
                }

                return Json(evidencijaRada, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metoda koja vraca ukupno vrijem za privatno odsustvo u toku jednog dana
        /// </summary>
        public TimeSpan? UkupnoVrijemeZaPrivatnoOdsustvo(DateTime Datum, int RadnikID)
        {
            if (RadnikID == 0)
                return null;
            using (var context = new LMContext())
            {

                //var SluzbeniPutID = context.Izlazs.FirstOrDefault(m => m.Naziv == "Privatno").IzlazID;
                var UserID = context.RadnikUsers.FirstOrDefault(m => m.RadnikID == RadnikID).UserID;

                var Vrijeme = context.WorkTimes.Where(r1 => r1.UserID == UserID && r1.IzlazID == 2 && r1.StartDate == Datum).Select(m1 => new
                {
                    inTime = m1.InTime,
                    outTime = m1.OutTime
                }).ToList();

                if (Vrijeme.Count != 0)
                {
                    double totalTime = 0;
                    foreach (var item in Vrijeme)
                    {
                        TimeSpan? span = item.outTime - item.inTime;
                        if (span != null)
                        {
                            totalTime += ((TimeSpan)span).TotalSeconds;
                        }
                    }
                    TimeSpan t = TimeSpan.FromSeconds(totalTime);
                    string VrijemeKojeJeKorisnikProveoNaPrivatnomOdsustvu = string.Format("{0:D2}:{1:D2}:{2:D2}",
                     t.Hours,
                     t.Minutes,
                     t.Seconds);

                    return t;
                }
                else
                    return null;
            }
        }

        private TimeSpan? UkupnoVrijemeNaPauzi(DateTime Datum, short RadnikID)
        {
            if (RadnikID == 0)
                return null;
            using (var context = new LMContext())
            {

                var UserID = context.RadnikUsers.FirstOrDefault(m => m.RadnikID == RadnikID).UserID;

                var Vrijeme = context.WorkTimes.Where(r1 => r1.UserID == UserID && r1.IzlazID == 1 && r1.StartDate == Datum).Select(m1 => new
                {
                    inTime = m1.InTime,
                    outTime = m1.OutTime
                }).ToList();

                if (Vrijeme.Count != 0)
                {
                    double totalTime = 0;
                    foreach (var item in Vrijeme)
                    {
                        TimeSpan? span = item.outTime - item.inTime;
                        if (span != null)
                        {
                            totalTime += ((TimeSpan)span).TotalSeconds;
                        }
                    }
                    TimeSpan t = TimeSpan.FromSeconds(totalTime);
                    string VrijemeKojeJeKorisnikProveoNaPrivatnomOdsustvu = string.Format("{0:D2}:{1:D2}:{2:D2}",
                     t.Hours,
                     t.Minutes,
                     t.Seconds);

                    return t;
                }
                else
                    return null;
            }
        }

        private RadnoVrijemeViewModel UkupnoVrijemeNaPoslu(DateTime Datum, int RadnikID)
        {
            using (var context = new LMContext())
            {
                var UserID = context.RadnikUsers.FirstOrDefault(m => m.RadnikID == RadnikID).UserID;
                var AktivnostiUJednomDanu = context.WorkTimes.Where(m => m.UserID == UserID && m.StartDate == Datum).ToList();
                var RadnoVrijeme = AktivnostiUJednomDanu.FirstOrDefault(m => m.IzlazID == 4 || m.IzlazID == 3) as WorkTime;

                if (AktivnostiUJednomDanu.Count == 0)
                {
                    var SluzbeniPut = context.EvidencijaRadas.FirstOrDefault(m => m.RadnikID == RadnikID && Datum >= m.Pocetak && Datum <= m.Kraj && (m.TipID == 5 || m.TipID == 3)) as EvidencijaRada;
                    if (SluzbeniPut != null)
                    {
                        return new RadnoVrijemeViewModel() { RednovnoVrijemeProvedenoNaPoslu = new TimeSpan(8,0,0)};
                        
                    }
                    else
                    {
                        return new RadnoVrijemeViewModel() { RednovnoVrijemeProvedenoNaPoslu = new TimeSpan(0, 0, 0) };
                    }
                }
                double totalSecends = 0;
                TimeSpan? span = new TimeSpan();

                if (RadnoVrijeme == null)
                {
                    //nije se uopste ulogovao na posao
                    return new RadnoVrijemeViewModel() { RednovnoVrijemeProvedenoNaPoslu = new TimeSpan(0, 0, 0) };
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

                    TimeSpan? privatnoOdsustvo = UkupnoVrijemeZaPrivatnoOdsustvo(Datum,RadnikID);
                    if (privatnoOdsustvo != null)
                        span -= privatnoOdsustvo;
                }

                totalSecends += ((TimeSpan)span).TotalSeconds;

                // ako je korisnik proveo na pauzi vise vremena od predvidjenog , oduzima mu se od radnog vremena
                TimeSpan? vrijemeNaPauzi = UkupnoVrijemeNaPauzi(Datum, Convert.ToInt16(RadnikID));
                if (vrijemeNaPauzi != null)
                {
                    if (((TimeSpan)vrijemeNaPauzi).TotalSeconds > 2700)
                    {
                        totalSecends -= ((TimeSpan)vrijemeNaPauzi).TotalSeconds - 2700;
                    }
                }
                TimeSpan vrijemeProvedenoNaPoslu = new TimeSpan();
                TimeSpan prekovremeno = new TimeSpan(0,0,0);

                if (totalSecends > 28800)
                {
                    int prekovremeneSekunde = (int)totalSecends - 28800;
                    totalSecends = 28800;
                    prekovremeno = TimeSpan.FromSeconds(prekovremeneSekunde);

                }

                vrijemeProvedenoNaPoslu = TimeSpan.FromSeconds(totalSecends);

                return new RadnoVrijemeViewModel() { RednovnoVrijemeProvedenoNaPoslu = vrijemeProvedenoNaPoslu, Prekovremeno = prekovremeno };
            }
        }
        #endregion

        [CustomAutorizeAttribute("admin,tim lider,menadžer")]
        public ActionResult Index()
        {
            //--- Nalaze se u metodama za brisanje "ne koriste se"!
            //IntervalSluzbeniPut(1, new DateTime(2019, 03, 13));
            //IntervalPrivatnoOdsustvo(4, new DateTime(2019, 03, 03));
            //IntervalPauza(4, new DateTime(2019, 03, 06));
            //----------------------------------------------------
            GodisnjiOdmorPreostaloDana(1);
            //VrijemeProvedenoNaPauzi(4, new DateTime(2019, 03, 03));
            //VrijemeNaSluzbenomPutu(3, new DateTime(2019, 03, 03));
            //VrijemeNaPrivatnomOdsustvu(7, new DateTime(2019, 03, 04));
            //VratiAktivnosti(4, new DateTime(2019, 03, 03));

            using (var context = new LMContext())
            {
                var RadnikID = PronadjiIDUlogovanogRadnika();
                var UlogovaniRadnik = context.Radniks.FirstOrDefault(m => m.RadnikID == RadnikID).Ime + " " + context.Radniks.FirstOrDefault(m => m.RadnikID == RadnikID).Prezime;
                //---------------- Metode se nalaze u regiji za brisanje
                //UkupnoVrijemeNaSluzbenomPutu(Convert.ToDateTime("03/03/2019"), 2); // testni podaci
                //UkupnoVrijemeZaPrivatnoOdsustvo(Convert.ToDateTime("03 / 03 / 2019"), 3);  //testni podaci
                //UkupnoVrijemeProvedenoNaPauzi(Convert.ToDateTime("03 / 03 / 2019"), 1);    //testni podaci
                //--------------------------------------------------------
                // Pronalazak svih grupa kojima pripada ulogovani radnik
                var GrupeUKojimaJeLider = context.GrupaRadniks.Where(x => x.RadnikID == RadnikID && x.Vođa == true).Select(m1 => new SelectListItem
                {
                    Value = m1.GrupaID.ToString(),
                    Text = m1.Grupe.Naziv
                }).ToList();

                //ViewBag.GrupeRadnika = GrupeUKojimaJeLider;

                var grupe = context.GrupaRadniks?.Where(x => x.RadnikID == RadnikID && x.Vođa == true).Select(g => new SelectListItem
                {
                    Value = g.GrupaID.ToString(),
                    Text = g.Grupe.Naziv
                }).ToList();
                    ViewBag.Grupe = grupe;

                var status = context.Tips.Select(s => new SelectListItem
                {
                    Value = s.TipID.ToString(),
                    Text = s.Naziv
                }).ToList();

                status.Insert(0, new SelectListItem() { Text = "Status", Value = "0" });

                ViewBag.Status = status;
                return View();
            }

        }

        public PartialViewResult _SviRadnici(int GrupaId, int StatusId, DateTime Datum)
        {
            using (var context = new LMContext())
            {
                var grupa = context.Grupes.FirstOrDefault(x => x.GrupaID == GrupaId);
                #region U sluzaju da nema UserID
                List<SviRadniciViewModel> prazanView = new List<SviRadniciViewModel>(); // u slucaju da ne postoji userID
                string UserName = User.Identity.Name;
                UserName = UserName.Replace("LANACO\\", "");
                var UserID = 0;
                try
                {
                    UserID = context.Users.FirstOrDefault(m => m.UserName == UserName).UserID;
                }
                catch
                {
                    return PartialView(prazanView);
                }
                #endregion

                var listaRadnikaUGrupi = grupa.GrupaRadniks.Where(m => m.GrupaID == grupa.GrupaID && m.DatumDo == null &&
                m.Radnik.VrijemeZaposlenjas.Any(vz => vz.DatumZaposlenja <= Datum && (vz.DatumPrestankaRada >= Datum || vz.DatumPrestankaRada == null)))
                .Select(m => new SviRadniciViewModel
                {
                    Ime = m.Radnik.Ime,
                    Prezime = m.Radnik.Prezime,
                    RadnikID = m.Radnik.RadnikID,
                    UserID = m.Radnik.RadnikUsers.FirstOrDefault(m1 => m1.RadnikID == m.RadnikID).UserID,
                    StatusID = NadjiStatusID(m.Radnik.RadnikUsers.FirstOrDefault(m1 => m1.RadnikID == m.RadnikID).UserID, Datum),
                    EmailAdresa = m.Radnik.EmailAdresa,
                    BrojTelefona = m.Radnik.BrojTelefona
                }).OrderByDescending(m => m.StatusID).ToList();
                foreach (var item in listaRadnikaUGrupi)
                {
                    item.NazivStatusa = NadjiNazivStatusa(item.StatusID);
                }
                if (StatusId != 0)
                {
                    listaRadnikaUGrupi = listaRadnikaUGrupi.Where(m => m.StatusID == StatusId).ToList();

                }

                return PartialView(listaRadnikaUGrupi);
            };

        }

        /// <summary>
        /// Ova metoda vraca profil radnika za dati dan i radnikID
        /// </summary>
        /// <param name="RadnikID"></param>
        /// <param name="Datum"></param>
        /// <returns></returns>
        public PartialViewResult VratiPocetnaProfilRadnika(int RadnikID, DateTime Datum)
        {
            using (var context = new LMContext())
            {

                var prijavljen = context.Radniks.FirstOrDefault(m => m.RadnikID == RadnikID);

                string UserName = User.Identity.Name;
                UserName = UserName.Replace("LANACO\\", "");
                int? UserID;

                if (prijavljen == null)
                {
                    UserID = context.Users.FirstOrDefault(m => m.UserName == UserName).UserID;
                    prijavljen = context.RadnikUsers.FirstOrDefault(ru => ru.UserID == UserID).Radnik;
                }
                else
                {
                    UserID = context.RadnikUsers.FirstOrDefault(u => u.RadnikID == RadnikID).UserID;
                }


                var AktivnostiUJednomDanu = context.WorkTimes.Where(m => m.UserID == UserID && m.StartDate == Datum).ToList();
                var RadnoVrijeme = AktivnostiUJednomDanu.FirstOrDefault(m => m.IzlazID == 4 || m.IzlazID == 3) as WorkTime;

                if (AktivnostiUJednomDanu.Count == 0)
                {
                    var SluzbeniPut = context.EvidencijaRadas.FirstOrDefault(m => m.RadnikID == prijavljen.RadnikID && Datum >= m.Pocetak && Datum <= m.Kraj && (m.TipID == 5 || m.TipID == 3)) as EvidencijaRada;
                    if (SluzbeniPut != null)
                    {
                        var viewModelRadnik = new RadnikViewModel
                        {
                            RadnikID = prijavljen.RadnikID,
                            Ime = prijavljen.Ime,
                            Prezime = prijavljen.Prezime,
                            Lider = (prijavljen.GrupaRadniks.Where(g => g.Vođa == true).Count() > 0) ? true : false,
                            DolazakNaPosao = new TimeSpan(8, 0, 0),
                            OdlazakSaPosla = new TimeSpan(16, 0, 0)
                        };
                        viewModelRadnik.ProcenatProvedenogVremenaNaPoslu = 100;
                        viewModelRadnik.VrijemeProvedenoNaPoslu = "08:00:00";
                        viewModelRadnik.ProcenatPrekovremenogRada = 0;
                        viewModelRadnik.PrekovremenoVrijemeProvedenoNaPoslu = "0";
                        return PartialView("_PocetnaProfilRadnika", viewModelRadnik);
                    }
                }
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

                    TimeSpan? privatnoOdsustvo = UkupnoVrijemeZaPrivatnoOdsustvo(Datum, prijavljen.RadnikID);
                    if (privatnoOdsustvo != null)
                        span -= privatnoOdsustvo;
                }

                totalSecends += ((TimeSpan)span).TotalSeconds;

                // ako je korisnik proveo na pauzi vise vremena od predvidjenog , oduzima mu se od radnog vremena
                TimeSpan? vrijemeNaPauzi = UkupnoVrijemeNaPauzi(Datum, prijavljen.RadnikID);
                if (vrijemeNaPauzi != null)
                {
                    if (((TimeSpan)vrijemeNaPauzi).TotalSeconds > 2700)
                    {
                        totalSecends -= ((TimeSpan)vrijemeNaPauzi).TotalSeconds - 2700;
                    }
                }
                int ProcenatPrekovremenogRada = 0;

                TimeSpan prekovremeno = new TimeSpan();

                if (totalSecends > 28800)
                {
                    int prekovremeneSekunde = (int)totalSecends - 28800;
                    totalSecends = 28800;
                    ProcenatPrekovremenogRada = (int)Math.Round((double)(100 * prekovremeneSekunde) / 28800);

                    prekovremeno = TimeSpan.FromSeconds(prekovremeneSekunde);

                }

                int ProcenatRedovnogRada = (int)Math.Round((double)(100 * totalSecends) / 28800);

                if (ProcenatPrekovremenogRada > 0)
                {
                    ProcenatRedovnogRada = 100 - ProcenatPrekovremenogRada;
                }

                TimeSpan t = TimeSpan.FromSeconds(totalSecends);

                string VrijemeKojeJeRadioUlogovaniKorisnik = string.Format("{0:D2}:{1:D2}:{2:D2}",
                t.Hours,
                t.Minutes,
                t.Seconds);

                string PrekovremenoVrijemeKorisnika = string.Format("{0:D2}:{1:D2}:{2:D2}",
                prekovremeno.Hours,
                prekovremeno.Minutes,
                prekovremeno.Seconds);

                var user = context.Users.FirstOrDefault(u => u.UserID == UserID);
                var dan = user.WorkTimes.FirstOrDefault(d => d.IzlazID == 4 && d.StartDate == Datum);//.InTime;
                TimeSpan? dolazak = new TimeSpan();

                if (dan == null)
                {
                    dolazak = new TimeSpan(7, 30, 0);
                }
                else
                {
                    dolazak = dan.InTime;
                }
                var odlazak = dolazak + TimeSpan.FromHours(8);

                var viewModel = new RadnikViewModel
                {
                    RadnikID = prijavljen.RadnikID,
                    Ime = prijavljen.Ime,
                    Prezime = prijavljen.Prezime,
                    Lider = (prijavljen.GrupaRadniks.Where(g => g.Vođa == true).Count() > 0) ? true : false,
                    DolazakNaPosao = dolazak,
                    OdlazakSaPosla = odlazak
                };

                viewModel.ProcenatProvedenogVremenaNaPoslu = ProcenatRedovnogRada;
                viewModel.VrijemeProvedenoNaPoslu = VrijemeKojeJeRadioUlogovaniKorisnik;
                viewModel.ProcenatPrekovremenogRada = ProcenatPrekovremenogRada;
                viewModel.PrekovremenoVrijemeProvedenoNaPoslu = PrekovremenoVrijemeKorisnika;
            
                var izlaz = user.WorkTimes.FirstOrDefault(d => d.IzlazID == 4)?.OutTime;

                if (izlaz == null)
                {
                    viewModel.OtisaoSaPosla = DateTime.Today.TimeOfDay;
                }
                else
                {
                    viewModel.OtisaoSaPosla = izlaz;
                }
                return PartialView("_PocetnaProfilRadnika", viewModel);
            }
        }



        /// <summary>
        /// Ova metoda vraca partial view sa listom aktivnosti tog radnika za trazeni dan
        /// </summary>
        /// <param name="RadnikID"></param>
        /// <param name="datum"></param>
        /// <returns></returns>
        public PartialViewResult VratiAktivnosti(int RadnikID, DateTime datum)
        {
            using (var context = new LMContext())
            {
                if (RadnikID == 0)
                {
                    RadnikID = (int)PronadjiIDUlogovanogRadnika();
                }
                var UserID = context.RadnikUsers.FirstOrDefault(u => u.RadnikID == RadnikID).UserID;
                var aktivnosti = context.WorkTimes.Where(workTime => workTime.UserID == UserID && workTime.StartDate == datum).Select(workTime => new SveAktivnostiViewModel()
                {
                    Pocetak = workTime.InTime,
                    Kraj = workTime.OutTime,
                    IzlazID = workTime.IzlazID,
                    IzlazNaziv = workTime.IzlazID == 1 ? "Pauza" : workTime.IzlazID == 2 ? "Privatno odsustvo" : workTime.IzlazID == 3 ? "Sluzbeni put" : "Posao"
                }).ToList();

                var rasporedjeneAktivnosti = new List<AktivnostViewModel>();
                foreach (var item in aktivnosti)
                {
                    rasporedjeneAktivnosti.Add(new AktivnostViewModel()
                    {
                        IzlazID = item.IzlazID,
                        Vrijeme = item.Pocetak,
                        IzlazNaziv = item.IzlazNaziv,
                        Odlazak = true
                    });

                    if (item.Kraj != null)
                        rasporedjeneAktivnosti.Add(new AktivnostViewModel()
                        {
                            IzlazID = item.IzlazID,
                            Vrijeme = item.Kraj,
                            IzlazNaziv = item.IzlazNaziv,
                            Odlazak = false
                        });
                }
                var zavrsni = rasporedjeneAktivnosti.OrderByDescending(akt => akt.Vrijeme).ToList();


                return PartialView("_SveAktivnosti", zavrsni);

            }

        }

        public JsonResult PrikaziDetalje(int RadnikID)
        {
            using (var context = new LMContext())
            {
                var radnik = context.Radniks.Where(m => m.RadnikID == RadnikID).Select(m1 => new RadnikViewModel
                {
                    RadnikID = m1.RadnikID,
                    Ime = m1.Ime,
                    Prezime = m1.Prezime,
                    LinkSlike = "https://cdn2.iconfinder.com/data/icons/people-3-2/128/Programmer-Avatar-Backend-Developer-Nerd-512.png",
                    EmailAdresa = m1.EmailAdresa,
                    BrojTelefona = "066/653-812"
                }).ToList();

                return Json(radnik, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        ///  Metoda koja vraca prisutnost radnika u toku jednog mjeseca, ako mjesec nije zavrsio vratice  i preostale dane
        /// </summary>  
        public JsonResult GetPrisutnost(int? RadnikID, int Mjesec, int Godina)
        {

            using (var context = new LMContext())
            {
                /*RadnikID = 4;
                DateTime datum = new DateTime(2019, 03, 03).Date;
                Mjesec = datum.Month;
                Godina = datum.Year;
                */
                //Pronalazi broj radnih dana u mjesecu

                if (RadnikID == 0 || RadnikID == null)
                    RadnikID = PronadjiIDUlogovanogRadnika();

                var RadniSatiUMjesecu = context.RadnoVrijemes.FirstOrDefault(m1 => m1.Mjesec == Mjesec).RadnihSatiUMjesecu;
                var RadnihDanaUMjesecu = RadniSatiUMjesecu / 8;
                var Radnik = context.Radniks.FirstOrDefault(r => r.RadnikID == RadnikID);

                //
                var DaniUMjesecu = Radnik.EvidencijaRadas.Where(r => r.Pocetak.Month == Mjesec && r.Pocetak.Year == Godina)
                    .Select(r => new
                    {
                        r.Pocetak,
                        r.Kraj,
                        r.TipID
                    }).ToList();

                PrisutnostViewModel prisutnostViewModels = new PrisutnostViewModel()
                {
                    RadnikID = RadnikID,
                    Nepoznato = 0,
                    NaPoslu = 0,
                    PreostaloDana = (short)RadnihDanaUMjesecu,
                    PrivatnoOdsutan = 0,
                    SluzbenoOdsutan = 0,
                    VisednevnoOdsutan = 0
                };

                //Lista neradnih dana koja se vuce iz baze
                var NeradniDani = context.NeradniDanis.Select(n => n.NeradniDan).ToList();


                foreach (var dan in DaniUMjesecu)
                {
                    var pocetniDatum = dan.Pocetak;
                    while (pocetniDatum >= dan.Pocetak && pocetniDatum <= dan.Kraj)
                    {
                        var nalaziUNeradnim = NeradniDani.Any(m => NeradniDani.Contains(pocetniDatum));

                        if (!nalaziUNeradnim)
                        {
                            if (dan.TipID == 1)
                                prisutnostViewModels.NaPoslu += 1;
                            else if (dan.TipID == 2)
                                prisutnostViewModels.PrivatnoOdsutan += 1;
                            else if (dan.TipID == 3)
                                prisutnostViewModels.VisednevnoOdsutan += 1;
                            else if (dan.TipID == 4)
                                prisutnostViewModels.Nepoznato += 1;
                            else if (dan.TipID == 5)
                                prisutnostViewModels.SluzbenoOdsutan += 1;

                            prisutnostViewModels.PreostaloDana -= 1;
                        }

                        pocetniDatum = pocetniDatum.AddDays(1);
                    }
                }
                return Json(prisutnostViewModels, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult OdradjenoVrijemeRadnikaUIntervalu(int? RadnikID, DateTime DatumOd, DateTime DatumDo)
        {
            using(var context = new LMContext())
            {
                var VrijemeKojeJeBioNaPoslu = context.EvidencijaRadas.Where(m => m.RadnikID == RadnikID && m.Pocetak >= DatumOd && m.Pocetak <= DatumDo && m.TipID == 1).ToList();

                // vratiti listu neradnih dana u tom periodu
                
                var  neradniDani = context.NeradniDanis.Where(m => m.NeradniDan >= DatumOd && m.NeradniDan <= DatumDo).Select(m => m.NeradniDan).Distinct().ToList();
                var ukupnoNeradnihDanaUPeriodu = neradniDani.Count();

                double brojRadnihDanaUPeriodu = 0;
                if (DatumDo != DatumOd)
                {
                    brojRadnihDanaUPeriodu = (DatumDo - DatumOd).TotalDays +1;
                }
                else
                {
                    brojRadnihDanaUPeriodu = 1;
                }
                brojRadnihDanaUPeriodu = brojRadnihDanaUPeriodu - ukupnoNeradnihDanaUPeriodu;
                var brojPotrebnihRadnihSatiUPeriodu = brojRadnihDanaUPeriodu * 28800;

                TimeSpan potrebnoRadnoVrijemeZaPeriod = TimeSpan.FromSeconds(brojPotrebnihRadnihSatiUPeriodu);

                // proci kroz dane koje je bio na poslu 
                double totalSeconds = 0;
                foreach (var item in VrijemeKojeJeBioNaPoslu)
                {
                    RadnoVrijemeViewModel radnoVrijeme = new RadnoVrijemeViewModel();
                    radnoVrijeme = UkupnoVrijemeNaPoslu(item.Pocetak, RadnikID ?? 1);
                    totalSeconds += ((TimeSpan)radnoVrijeme.RednovnoVrijemeProvedenoNaPoslu).TotalSeconds + ((TimeSpan)radnoVrijeme.Prekovremeno).TotalSeconds;
                }

                TimeSpan brojOdradjenihRadnihSati = TimeSpan.FromSeconds(totalSeconds);
                TimeSpan brojSatiKojejeProveoPrekovremeno = new TimeSpan();
                TimeSpan brojSatiKojeJeProveoManje = new TimeSpan();
                if(brojOdradjenihRadnihSati > potrebnoRadnoVrijemeZaPeriod)
                {
                    brojSatiKojejeProveoPrekovremeno = brojOdradjenihRadnihSati - potrebnoRadnoVrijemeZaPeriod;
                }
                else
                {
                    brojSatiKojeJeProveoManje = potrebnoRadnoVrijemeZaPeriod - brojOdradjenihRadnihSati;
                }
                string potrebnoRadnoVrijemeZaPeriodString = string.Format("{0:D2}:{1:D2}:{2:D2}",
                           potrebnoRadnoVrijemeZaPeriod.TotalHours.ToString().Split('.')[0],
                           potrebnoRadnoVrijemeZaPeriod.Minutes,
                           potrebnoRadnoVrijemeZaPeriod.Seconds);
                string brojOdradjenihRadnihSatiString = string.Format("{0:D2}:{1:D2}:{2:D2}",
                           brojOdradjenihRadnihSati.TotalHours.ToString().Split('.')[0],
                           brojOdradjenihRadnihSati.Minutes,
                           brojOdradjenihRadnihSati.Seconds);
                string brojSatiKojejeProveoPrekovremenoString = string.Format("{0:D2}:{1:D2}:{2:D2}",
                           brojSatiKojejeProveoPrekovremeno.TotalHours.ToString().Split('.')[0],
                           brojSatiKojejeProveoPrekovremeno.Minutes,
                           brojSatiKojejeProveoPrekovremeno.Seconds);
                string brojSatiKojeJeProveoManjeString = string.Format("{0:D2}:{1:D2}:{2:D2}",
                           brojSatiKojeJeProveoManje.TotalHours.ToString().Split('.')[0],
                           brojSatiKojeJeProveoManje.Minutes,
                           brojSatiKojeJeProveoManje.Seconds);

                var model = new
                {
                    potrebnoRadnoVrijemeZaPeriodString,
                    brojOdradjenihRadnihSatiString,
                    brojSatiKojejeProveoPrekovremenoString,
                    brojSatiKojeJeProveoManjeString
                };
            return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metoda koja vraca za radnika preostali broj dana godisnjeg odmora
        /// </summary>

        public JsonResult GodisnjiOdmorPreostaloDana(int? RadnikID)
        {

            using (var context = new LMContext())
            {
                if (RadnikID == 0 || RadnikID == null)
                    RadnikID = PronadjiIDUlogovanogRadnika();

                var Radnik = context.Radniks.FirstOrDefault(r => r.RadnikID == RadnikID);
                var iskoristeno = context.EvidencijaRadas.Where(i => i.TipID == 3).Where(r => r.RadnikID == RadnikID).Count();
                var godisnjiOdmor = context.GodisnjiOdmors.FirstOrDefault(r => r.RadnikID == RadnikID).BrojDana;
                var preostaloDana = godisnjiOdmor - iskoristeno;
                return Json(preostaloDana, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult PopuniPieGraf(DateTime Datum, int GrupaID)
        {
            using (var context = new LMContext())
            {
                //pronadjena grupa
                var Grupa = context.Grupes.FirstOrDefault(g => g.GrupaID == GrupaID);

                //lista svih radnika iz grupe koji su zaposleni na taj datum
                var listaRadnikaUGrupi = Grupa.GrupaRadniks.Where(m => m.GrupaID == Grupa.GrupaID && m.DatumDo == null &&
                m.Radnik.VrijemeZaposlenjas.Any(vz => vz.DatumZaposlenja <= Datum && (vz.DatumPrestankaRada >= Datum || vz.DatumPrestankaRada == null))).Select(pr => pr.Radnik);

                //Pronaci usere da se moze pristupiti WorkTime

                var Users = listaRadnikaUGrupi.Select(lr => lr.RadnikUsers.FirstOrDefault().User).ToList();

                //izvuci sve statuse za odredjeni datum
                var tipStatusa = listaRadnikaUGrupi.Where(lr => lr.EvidencijaRadas.Any(er => er.Pocetak <= Datum && (er.Kraj >= Datum || er.Kraj == null)))
                    .Select(lr => lr.EvidencijaRadas.FirstOrDefault(er => er.Pocetak <= Datum && (er.Kraj >= Datum || er.Kraj == null))).ToList();

                //izlazi za sve Usere koji su u odredjenoj grupi i zaposleni na odredjeni dan
                List<WorkTime> workTimesList = new List<WorkTime>();
                for (int i = 0; i < Users.Count(); i++)
                {
                    //lista workTimes za svakog radnika na odredjeni datum
                    var workTimes = Users[i].WorkTimes.Where(wt => wt.StartDate == Datum).ToList();
                    foreach (var item in workTimes)
                    {
                        workTimesList.Add(item);
                    }
                };
                //vrijeme u minutama
                short Pauza = 0;
                short Privatno = 0;
                short SluzbeniPut = 0;
                short Posao = 0;

                foreach (var item in workTimesList)
                {
                    TimeSpan? VrijemeNaPauzi = item.OutTime - item.InTime;
                    short MinuteNaPauzi = Convert.ToInt16(((TimeSpan)VrijemeNaPauzi).TotalMinutes);
                    switch (item.IzlazID)
                    {
                        case 1:
                            Pauza += MinuteNaPauzi;
                            break;
                        case 2:
                            Privatno += MinuteNaPauzi;
                            break;
                        case 3:
                            SluzbeniPut += MinuteNaPauzi;
                            break;
                        case 4:
                            Posao += MinuteNaPauzi;
                            break;
                    }
                }
                Posao -= (short)(Privatno + SluzbeniPut + Pauza);

                List<short> BrojeviZaGraf = new List<short>(){
                    Pauza, Privatno, SluzbeniPut, Posao
                };

                return Json(BrojeviZaGraf, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Vraca brojeve za popunjavanje grafika
        /// </summary>
        /// <param name="Datum"></param>
        /// <param name="GrupaID"></param>
        /// <returns></returns>
        public JsonResult PopuniGraf(DateTime Datum, int GrupaID)
        {
            using (var context = new LMContext())
            {
                //pornadjena grupa
                var Grupa = context.Grupes.FirstOrDefault(g => g.GrupaID == GrupaID);

                //lista svih radnika iz grupe koji su zaposleni na taj datum
                var listaRadnikaUGrupi = Grupa.GrupaRadniks.Where(m => m.GrupaID == Grupa.GrupaID && m.DatumDo == null &&
                m.Radnik.VrijemeZaposlenjas.Any(vz => vz.DatumZaposlenja <= Datum && (vz.DatumPrestankaRada >= Datum || vz.DatumPrestankaRada == null))).Select(pr => pr.Radnik);

                //izvuci sve statuse za odredjeni datum
                //var tipStatusa = listaRadnikaUGrupi.Where(lr => lr.EvidencijaRadas.Any(er => er.Pocetak <= Datum && (er.Kraj >= Datum || er.Kraj == null)))
                //    .Select(lr => lr.EvidencijaRadas.FirstOrDefault(er => er.Pocetak <= Datum && (er.Kraj >= Datum || er.Kraj == null))).ToList();

                List<EvidencijaRada> tipStatusa = new List<EvidencijaRada>();
                foreach (var item in listaRadnikaUGrupi)
                {
                    EvidencijaRada novaEvidencija = new EvidencijaRada();
                    novaEvidencija.TipID = (byte)NadjiStatusID(item.RadnikUsers?.FirstOrDefault(m => m.RadnikID == item.RadnikID).UserID, Datum);
                    tipStatusa.Add(novaEvidencija);
                }
                //ukupno radnika
                int ukupnoRadnikaUGrupi = listaRadnikaUGrupi.Count();

                //vraca broj radnika koji nemaju podataka u bazi za odredjeni datum
                int RadniciBezPodataka = ukupnoRadnikaUGrupi - tipStatusa.Count();


                //TipID   Naziv
                //1       Na poslu
                //2       Privatno odsutan
                //3       Visednevno odsutan
                //4       Nepoznato
                //5       Sluzbeno odsutan
                List<int> BrojeviZaGraf = new List<int>() {
                    tipStatusa.Where(ts => ts.TipID == 1).Count(),
                    tipStatusa.Where(ts => ts.TipID == 2).Count(),
                    tipStatusa.Where(ts => ts.TipID == 3).Count(),
                    RadniciBezPodataka + tipStatusa.Where(ts => ts.TipID == 4).Count(),
                    tipStatusa.Where(ts => ts.TipID == 5).Count()
                };
                var model = new
                {
                    BrojeviZaGraf,
                    ukupnoRadnikaUGrupi
                };


                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        #region Racunanje vremena u intervalima 
        public JsonResult VrijemePoDanuNaSluzbenomPutuZaPeriod(DateTime Datum1, DateTime Datum2, int RadnikID)
        {
            using (var context = new LMContext())
            {
                int? UlogovaniRadnik;
                if (RadnikID == 0)
                    UlogovaniRadnik = PronadjiIDUlogovanogRadnika();
                else
                    UlogovaniRadnik = RadnikID;

                var UserID = context.RadnikUsers.FirstOrDefault(r => r.RadnikID == UlogovaniRadnik).UserID;
                var Vrijeme = context.WorkTimes.Where(r => r.UserID == UserID && r.IzlazID == 3 && r.StartDate >= Datum1 && r.StartDate <= Datum2).Select(m => new
                {
                    m.StartDate,
                    m.InTime,
                    m.OutTime
                }).OrderBy(m => m.StartDate).ToList();

                #region deklaracija i inicijalizacija
                SluzbeniPutVrijemeViewModel viewModel = new SluzbeniPutVrijemeViewModel();
                viewModel.ListaVremena = new List<string>();
                viewModel.ListaMinutaNaSluzbenomPutu = new List<short>();
                viewModel.ListaLabela = new List<string>();
                DateTime datumZaProvjeru = new DateTime();
                #endregion

                if (Vrijeme.Count != 0)
                {
                    #region prvi datum
                    datumZaProvjeru = Vrijeme[0].StartDate;
                    double totalTime = 0;
                    TimeSpan? span = Vrijeme[0].OutTime - Vrijeme[0].InTime;
                    if (span != null)
                    {
                        totalTime += ((TimeSpan)span).TotalMinutes;
                    }
                    TimeSpan t = TimeSpan.FromMinutes(totalTime);
                    string VrijemeKojeJeKorisnikProveoNaSluzbenomOdsustvu = string.Format("{0:D2}:{1:D2}",
                    t.Hours,
                    t.Minutes);
                    #endregion

                    for (int i = 1; i < Vrijeme.Count; i++)

                    {
                        if (Vrijeme[i].StartDate == datumZaProvjeru)
                        {
                            VrijemeKojeJeKorisnikProveoNaSluzbenomOdsustvu = "";

                            span = Vrijeme[i].OutTime - Vrijeme[i].InTime;
                            if (span != null)
                            {
                                totalTime += ((TimeSpan)span).TotalMinutes;
                            }
                            t = TimeSpan.FromMinutes(totalTime);
                            VrijemeKojeJeKorisnikProveoNaSluzbenomOdsustvu = string.Format("{0:D2}:{1:D2}",
                            t.Hours,
                            t.Minutes);
                            if (i == Vrijeme.Count)
                            {
                                viewModel.ListaMinutaNaSluzbenomPutu.Add(Convert.ToInt16(totalTime));
                                viewModel.ListaVremena.Add(VrijemeKojeJeKorisnikProveoNaSluzbenomOdsustvu);
                                viewModel.ListaLabela.Add(Vrijeme[i - 1].StartDate.ToString("dd.MM.yyyy"));
                            }
                        }
                        else
                        {

                            viewModel.ListaMinutaNaSluzbenomPutu.Add(Convert.ToInt16(totalTime));
                            viewModel.ListaVremena.Add(VrijemeKojeJeKorisnikProveoNaSluzbenomOdsustvu);
                            viewModel.ListaLabela.Add(Vrijeme[i - 1].StartDate.ToString("dd.MM.yyyy"));
                            datumZaProvjeru = Vrijeme[i].StartDate;
                            totalTime = 0;

                            VrijemeKojeJeKorisnikProveoNaSluzbenomOdsustvu = "";

                            span = Vrijeme[i].OutTime - Vrijeme[i].InTime;
                            if (span != null)
                            {
                                totalTime += ((TimeSpan)span).TotalMinutes;
                            }
                            t = TimeSpan.FromMinutes(totalTime);
                            VrijemeKojeJeKorisnikProveoNaSluzbenomOdsustvu = string.Format("{0:D2}:{1:D2}",
                            t.Hours,
                            t.Minutes);

                        }

                    }

                    #region Zadnji datum
                    viewModel.ListaMinutaNaSluzbenomPutu.Add(Convert.ToInt16(totalTime));
                    viewModel.ListaVremena.Add(VrijemeKojeJeKorisnikProveoNaSluzbenomOdsustvu);
                    viewModel.ListaLabela.Add(Vrijeme[Vrijeme.Count - 1].StartDate.ToString("dd.MM.yyyy"));
                    #endregion  

                    return Json(viewModel, JsonRequestBehavior.AllowGet);
                }
                else
                    return null;
            }

        }
        public JsonResult VrijemePoDanuNaPrivatnomOdsustvuZaPeriod(DateTime Datum1, DateTime Datum2, int RadnikID)
        {
            using (var context = new LMContext())
            {
                int? UlogovaniRadnik;
                if (RadnikID == 0)
                    UlogovaniRadnik = PronadjiIDUlogovanogRadnika();
                else
                    UlogovaniRadnik = RadnikID;

                var UserID = context.RadnikUsers.FirstOrDefault(r => r.RadnikID == UlogovaniRadnik).UserID;
                var Vrijeme = context.WorkTimes.Where(r => r.UserID == UserID && r.IzlazID == 2 && r.StartDate >= Datum1 && r.StartDate <= Datum2).Select(m => new
                {
                    m.StartDate,
                    m.InTime,
                    m.OutTime
                }).OrderBy(m => m.StartDate).ToList();

                #region deklaracija i inicijalizacija
                PrivatnoOdsustvoVrijemeViewModel viewModel = new PrivatnoOdsustvoVrijemeViewModel();
                viewModel.ListaVremena = new List<string>();
                viewModel.ListaMinutaNaPrivatnomOdsustvu = new List<short>();
                viewModel.ListaLabela = new List<string>();
                DateTime datumZaProvjeru = new DateTime();
                #endregion

                if (Vrijeme.Count != 0)
                {
                    #region prvi datum
                    datumZaProvjeru = Vrijeme[0].StartDate;
                    double totalTime = 0;
                    TimeSpan? span = Vrijeme[0].OutTime - Vrijeme[0].InTime;
                    if (span != null)
                    {
                        totalTime += ((TimeSpan)span).TotalMinutes;
                    }
                    TimeSpan t = TimeSpan.FromMinutes(totalTime);
                    string VrijemeKojeJeKorisnikProveoNaPrivatnomOdsustvu = string.Format("{0:D2}:{1:D2}",
                    t.Hours,
                    t.Minutes);
                    #endregion

                    for (int i = 1; i < Vrijeme.Count; i++)

                    {
                        if (Vrijeme[i].StartDate == datumZaProvjeru)
                        {
                            VrijemeKojeJeKorisnikProveoNaPrivatnomOdsustvu = "";

                            span = Vrijeme[i].OutTime - Vrijeme[i].InTime;
                            if (span != null)
                            {
                                totalTime += ((TimeSpan)span).TotalMinutes;
                            }
                            t = TimeSpan.FromMinutes(totalTime);
                            VrijemeKojeJeKorisnikProveoNaPrivatnomOdsustvu = string.Format("{0:D2}:{1:D2}",
                            t.Hours,
                            t.Minutes);
                            if (i == Vrijeme.Count)
                            {
                                viewModel.ListaMinutaNaPrivatnomOdsustvu.Add(Convert.ToInt16(totalTime));
                                viewModel.ListaVremena.Add(VrijemeKojeJeKorisnikProveoNaPrivatnomOdsustvu);
                                viewModel.ListaLabela.Add(Vrijeme[i - 1].StartDate.ToString("dd.MM.yyyy"));
                            }
                        }
                        else
                        {

                            viewModel.ListaMinutaNaPrivatnomOdsustvu.Add(Convert.ToInt16(totalTime));
                            viewModel.ListaVremena.Add(VrijemeKojeJeKorisnikProveoNaPrivatnomOdsustvu);
                            viewModel.ListaLabela.Add(Vrijeme[i - 1].StartDate.ToString("dd.MM.yyyy"));
                            datumZaProvjeru = Vrijeme[i].StartDate;
                            totalTime = 0;

                            VrijemeKojeJeKorisnikProveoNaPrivatnomOdsustvu = "";

                            span = Vrijeme[i].OutTime - Vrijeme[i].InTime;
                            if (span != null)
                            {
                                totalTime += ((TimeSpan)span).TotalMinutes;
                            }
                            t = TimeSpan.FromMinutes(totalTime);
                            VrijemeKojeJeKorisnikProveoNaPrivatnomOdsustvu = string.Format("{0:D2}:{1:D2}",
                            t.Hours,
                            t.Minutes);

                        }

                    }

                    #region Zadnji datum
                    viewModel.ListaMinutaNaPrivatnomOdsustvu.Add(Convert.ToInt16(totalTime));
                    viewModel.ListaVremena.Add(VrijemeKojeJeKorisnikProveoNaPrivatnomOdsustvu);
                    viewModel.ListaLabela.Add(Vrijeme[Vrijeme.Count - 1].StartDate.ToString("dd.MM.yyyy"));
                    #endregion  

                    return Json(viewModel, JsonRequestBehavior.AllowGet);
                }
                else
                    return null;
            }

        }

        public JsonResult VrijemePoDanuNaPauziZaPeriod(DateTime Datum1, DateTime Datum2, int RadnikID)
        {
            using (var context = new LMContext())
            {
                int? UlogovaniRadnik;
                if (RadnikID == 0)
                    UlogovaniRadnik = PronadjiIDUlogovanogRadnika();
                else
                    UlogovaniRadnik = RadnikID;
                var UserID = context.RadnikUsers.FirstOrDefault(r => r.RadnikID == UlogovaniRadnik).UserID;
                var Vrijeme = context.WorkTimes.Where(r => r.UserID == UserID && r.IzlazID == 1 && r.StartDate >= Datum1 && r.StartDate <= Datum2).Select(m => new
                {
                    m.StartDate,
                    m.InTime,
                    m.OutTime
                }).OrderBy(m => m.StartDate).ToList();

                #region deklaracija i inicijalizacija
                PauzaDijagramViewModel viewModel = new PauzaDijagramViewModel();
                viewModel.ListaVremena = new List<string>();
                viewModel.ListaMinutaNaPauzi = new List<short>();
                viewModel.ListaLabela = new List<string>();
                DateTime datumZaProvjeru = new DateTime();
                #endregion

                if (Vrijeme.Count != 0)
                {
                    #region prvi datum
                    datumZaProvjeru = Vrijeme[0].StartDate;
                    double totalTime = 0;
                    TimeSpan? span = Vrijeme[0].OutTime - Vrijeme[0].InTime;
                    if (span != null)
                    {
                        totalTime += ((TimeSpan)span).TotalMinutes;
                    }
                    TimeSpan t = TimeSpan.FromMinutes(totalTime);
                    string VrijemeKojeJeKorisnikProveoNaPauzi = string.Format("{0:D2}:{1:D2}",
                    t.Hours,
                    t.Minutes);
                    #endregion

                    for (int i = 1; i < Vrijeme.Count; i++)

                    {
                        if (Vrijeme[i].StartDate == datumZaProvjeru)
                        {
                            VrijemeKojeJeKorisnikProveoNaPauzi = "";

                            span = Vrijeme[i].OutTime - Vrijeme[i].InTime;
                            if (span != null)
                            {
                                totalTime += ((TimeSpan)span).TotalMinutes;
                            }
                            t = TimeSpan.FromMinutes(totalTime);
                            VrijemeKojeJeKorisnikProveoNaPauzi = string.Format("{0:D2}:{1:D2}",
                            t.Hours,
                            t.Minutes);
                            if (i == Vrijeme.Count)
                            {
                                viewModel.ListaMinutaNaPauzi.Add(Convert.ToInt16(totalTime));
                                viewModel.ListaVremena.Add(VrijemeKojeJeKorisnikProveoNaPauzi);
                                viewModel.ListaLabela.Add(Vrijeme[i - 1].StartDate.ToString("dd.MM.yyyy"));
                            }
                        }
                        else
                        {

                            viewModel.ListaMinutaNaPauzi.Add(Convert.ToInt16(totalTime));
                            viewModel.ListaVremena.Add(VrijemeKojeJeKorisnikProveoNaPauzi);
                            viewModel.ListaLabela.Add(Vrijeme[i - 1].StartDate.ToString("dd.MM.yyyy"));
                            datumZaProvjeru = Vrijeme[i].StartDate;
                            totalTime = 0;

                            VrijemeKojeJeKorisnikProveoNaPauzi = "";

                            span = Vrijeme[i].OutTime - Vrijeme[i].InTime;
                            if (span != null)
                            {
                                totalTime += ((TimeSpan)span).TotalMinutes;
                            }
                            t = TimeSpan.FromMinutes(totalTime);
                            VrijemeKojeJeKorisnikProveoNaPauzi = string.Format("{0:D2}:{1:D2}",
                            t.Hours,
                            t.Minutes);

                        }

                    }

                    #region Zadnji datum
                    viewModel.ListaMinutaNaPauzi.Add(Convert.ToInt16(totalTime));
                    viewModel.ListaVremena.Add(VrijemeKojeJeKorisnikProveoNaPauzi);
                    viewModel.ListaLabela.Add(Vrijeme[Vrijeme.Count - 1].StartDate.ToString("dd.MM.yyyy"));
                    #endregion  

                    return Json(viewModel, JsonRequestBehavior.AllowGet);
                }
                else
                    return null;
            }

        }

        #endregion


        /// <summary>
        /// Vraca podatke za pie char - modal
        /// </summary>
        /// <param name="RadnikID"></param>
        /// <param name="DatumOd"></param>
        /// <param name="DatumDo"></param>
        /// <returns></returns>
        public JsonResult GetPrisutnostModal(int? RadnikID, DateTime DatumOd, DateTime DatumDo)
        {
            using (var context = new LMContext())
            {
                /*RadnikID = 4;
                DateTime datum = new DateTime(2019, 03, 03).Date;
                Mjesec = datum.Month;
                Godina = datum.Year;
                */
                //Pronalazi broj radnih dana u mjesecu

                if (RadnikID == 0 || RadnikID == null)
                    RadnikID = PronadjiIDUlogovanogRadnika();

                //var RadniSatiUMjesecu = context.RadnoVrijemes.FirstOrDefault(m1 => m1.Mjesec == Mjesec).RadnihSatiUMjesecu;
                //var RadnihDanaUMjesecu = RadniSatiUMjesecu / 8;
                var Radnik = context.Radniks.FirstOrDefault(r => r.RadnikID == RadnikID);

                //
                var DaniUMjesecu = Radnik.EvidencijaRadas.Where(r => r.Pocetak >= DatumOd && r.Pocetak <= DatumDo)
                    .Select(r => new
                    {
                        r.Pocetak,
                        r.Kraj,
                        r.TipID
                    }).ToList();

                PrisutnostViewModel prisutnostViewModels = new PrisutnostViewModel()
                {
                    RadnikID = RadnikID,
                    Nepoznato = 0,
                    NaPoslu = 0,
                    PrivatnoOdsutan = 0,
                    SluzbenoOdsutan = 0,
                    VisednevnoOdsutan = 0
                };

                //Lista neradnih dana koja se vuce iz baze
                var NeradniDani = context.NeradniDanis.Select(n => n.NeradniDan);


                foreach (var dan in DaniUMjesecu)
                {
                    var pocetniDatum = dan.Pocetak;
                    while (pocetniDatum >= dan.Pocetak && pocetniDatum <= dan.Kraj)
                    {
                        bool nerdaniDanProvjera = true;
                        foreach (var neradniDan in NeradniDani)
                        {
                            if (pocetniDatum == neradniDan)
                            {
                                nerdaniDanProvjera = false;
                            }
                        }
                        if (nerdaniDanProvjera)
                        {
                            if (dan.TipID == 1)
                                prisutnostViewModels.NaPoslu += 1;
                            else if (dan.TipID == 2)
                                prisutnostViewModels.PrivatnoOdsutan += 1;
                            else if (dan.TipID == 3)
                                prisutnostViewModels.VisednevnoOdsutan += 1;
                            else if (dan.TipID == 4)
                                prisutnostViewModels.Nepoznato += 1;
                            else if (dan.TipID == 5)
                                prisutnostViewModels.SluzbenoOdsutan += 1;
                        }
                        pocetniDatum = pocetniDatum.AddDays(1);
                    }
                }
                return Json(prisutnostViewModels, JsonRequestBehavior.AllowGet);
            }
        }



        #region metode koje se ne koriste, u slucaju da je neka od metoda potrebna ostavljena je do petka
        /// <summary>
        /// Metoda koja vraca intervale za sluzbeni put u toku jednog dana
        /// </summary> 
        //private void IntervalSluzbeniPut(int RadnikID, DateTime datum)
        //{

        //    using (var context = new LMContext())
        //    {
        //        var UserID = context.RadnikUsers.FirstOrDefault(u => u.RadnikID == RadnikID).UserID;
        //        //var SluzbeniPutID = context.Izlazs.FirstOrDefault(s => s.Naziv == "Sluzbeni put").IzlazID;
        //        var SluzbeniPutInterval = context.WorkTimes.Where(s => s.IzlazID == 3 && s.UserID == UserID && s.StartDate == datum).Select(s1 => new
        //        {
        //            Pocetak = s1.InTime,
        //            Kraj = s1.OutTime
        //        }).ToList();

        //    }
        //}

        /// <summary>
        /// Metoda koja vraca intervale za pauzu u toku jednog dana
        /// </summary>
        //private void IntervalPauza(int RadnikID, DateTime datum)
        //{
        //    using (var context = new LMContext())
        //    {
        //        var UserID = context.RadnikUsers.FirstOrDefault(u => u.RadnikID == RadnikID).UserID;
        //        // var PauzaID = context.Izlazs.FirstOrDefault(s => s.Naziv == "Pauza").IzlazID;
        //        var PauzaInterval = context.WorkTimes.Where(s => s.IzlazID == 1 && s.UserID == UserID && s.StartDate == datum).Select(s1 => new
        //        {
        //            Pocetak = s1.InTime,
        //            Kraj = s1.OutTime
        //        }).ToList();

        //    }
        //}
        /// <summary>
        /// Metoda koja vraca intervale za privatno odsustvo u toku jednog dana
        /// </summary>
        //private void IntervalPrivatnoOdsustvo(int RadnikID, DateTime datum)
        //{
        //    using (var context = new LMContext())
        //    {
        //        var UserID = context.RadnikUsers.FirstOrDefault(u => u.RadnikID == RadnikID).UserID;
        //        // var PrivatnoID = context.Izlazs.FirstOrDefault(s => s.Naziv == "Privatno").IzlazID;
        //        var PrivatnoInterval = context.WorkTimes.Where(s => s.IzlazID == 2 && s.UserID == UserID && s.StartDate == datum).Select(s1 => new
        //        {
        //            Pocetak = s1.InTime,
        //            Kraj = s1.OutTime
        //        }).ToList();
        //    }
        //}
        /// <summary>
        /// Metoda koja vraca intervale za sluzbeno odsustvo u toku jednog dana
        /// </summary>
        //private void UkupnoVrijemeNaSluzbenomPutu(DateTime Datum, int RadnikID)
        //{
        //    if (RadnikID == 0)
        //        return;
        //    using (var context = new LMContext())
        //    {
        //        var UserID = context.RadnikUsers.FirstOrDefault(m => m.RadnikID == RadnikID).UserID;
        //        var Vrijeme = context.WorkTimes.Where(r1 => r1.UserID == UserID && r1.IzlazID == 3 && r1.StartDate == Datum).Select(m1 => new
        //        {
        //            inTime = m1.InTime,
        //            outTime = m1.OutTime
        //        }).ToList();

        //        if (Vrijeme.Count != 0)
        //        {
        //            double totalTime = 0;
        //            foreach (var item in Vrijeme)
        //            {
        //                TimeSpan? span = item.outTime - item.inTime;
        //                if (span != null)
        //                {
        //                    totalTime += ((TimeSpan)span).TotalSeconds;
        //                }
        //            }
        //            TimeSpan t = TimeSpan.FromSeconds(totalTime);
        //            string VrijemeKojeJeKorisnikProveoNaSluzbenomPutu = string.Format("{0:D2}:{1:D2}:{2:D2}",
        //             t.Hours,
        //             t.Minutes,
        //             t.Seconds);
        //        }
        //    }
        //}

        /// <summary>
        /// Metoda koja vraca intervale za pauza odsustvo u toku jednog dana
        /// </summary>
        //private void UkupnoVrijemeProvedenoNaPauzi(DateTime Datum, int RadnikID)
        //{
        //    using (var context = new LMContext())
        //    {
        //        //  var SluzbeniPutID = context.Izlazs.FirstOrDefault(s => s.Naziv == "Pauza").IzlazID;
        //        var UserId = context.RadnikUsers.FirstOrDefault(m => m.RadnikID == RadnikID).UserID;
        //        var Vrijeme = context.WorkTimes.Where(r => r.UserID == UserId && r.StartDate == Datum && r.IzlazID == 1).Select(r1 => new
        //        {
        //            inTime = r1.InTime,
        //            outTime = r1.OutTime
        //        }).ToList();

        //        if (Vrijeme.Count != 0)
        //        {
        //            double totalTime = 0;

        //            foreach (var item in Vrijeme)
        //            {
        //                TimeSpan? span = item.outTime - item.inTime;
        //                if (span != null)
        //                {
        //                    totalTime += ((TimeSpan)span).TotalSeconds;
        //                }
        //            }
        //            TimeSpan t = TimeSpan.FromSeconds(totalTime);
        //            string UkupnoVrijemNaPauzi = string.Format("{0:D2}:{1:D2}:{2:D2}",
        //                t.Hours, t.Minutes, t.Seconds);
        //        }
        //    }
        //}
        #endregion
    }
}
