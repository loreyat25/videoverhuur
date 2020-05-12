using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoVerhuur.Models;
using VideoVerhuur.Services;
namespace VideoVerhuur.Controllers
{  

    public class HomeController : Controller
    {
      
        private FilmViewModel fvm = new FilmViewModel();
        private VerhuurService db = new VerhuurService();
        public ActionResult Index()
        {

           
            Session["FilmLijst"] = null;
            Session["klant"] = null;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult BestaandeKlant()
        {
            try { 
            if (Request["aanmelden"] != null)
            {
                var naam = Request["naam"];
                var postcode =Request["postcode"];
                var klant = db.GetKlant(naam, postcode);
                if (klant != null)
                {

                    Session["klant"] = klant;

                }
                else
                    ViewBag.errorMessage = "Verkeerde naam of postcode";
               
            }
            return View();
            } catch(Exception ex)
            {
                return View(ex.Message);
            }
        }
       
        public ActionResult Genres(int? id)
        {
            if(Session["klant"] != null)
            {
                 
                fvm.Genres = db.GetAlleGenres();
                fvm.Genre = db.GetGenre(id);
                fvm.Films = db.GetAlleFilmsVanEenGenre(id);
                fvm.klant =(Klant)Session["klant"];
                return View(fvm);
            }
            else
            {
                return View();
            }
        }
        public ActionResult DetailPagina(int? id)
        {
            var filmviewmodel = new FilmViewModel();
            filmviewmodel.Genres = db.GetAlleGenres();
            filmviewmodel.Genre = db.GetGenre(id);
            filmviewmodel.Films = db.GetAlleFilmsVanEenGenre(id);
            return View(filmviewmodel);
        
        }
        private List<MandjeItem> winkelmand = new List<MandjeItem>();
         public ActionResult WinkelMandje(int? filmid)
       {
            try
            {
                if (Session["FilmLijst"] != null)
                    winkelmand = (List<MandjeItem>)Session["FilmLijst"];
                var Film = db.GetFilm(filmid);
                var klant = Session["klant"];
                MandjeItem item = new MandjeItem((Klant)klant, Film);

                winkelmand.Add(item);
                Session["FilmLijst"] = winkelmand;
                return View(winkelmand);
            }catch(Exception)
            {
                return View("Index", "Home");
            }
        




       }

      
        public ActionResult Verwijderen()
        {
            winkelmand = (List<MandjeItem>)Session["FilmLijst"];
              if (Request["Ja"] != null)
               {
                   winkelmand.Remove((MandjeItem)Session["VerwijderFilm"]);
                   Session["FilmLijst"] = winkelmand;
                   return View("WinkelMandje",winkelmand);
               }
              if(Request["Nee"] != null)
                return View("WinkelMandje", winkelmand);
            return View((MandjeItem)Session["VerwijderFilm"]);
        }
        public ActionResult Afrekening()
        {


            
            decimal totaleprijs = 0;
            winkelmand = (List<MandjeItem>)Session["FilmLijst"];
            foreach(var mandjeitem in winkelmand)
            {

                Verhuur verhuur = new Verhuur();
                verhuur.BandNr = mandjeitem.film.BandNr;
                verhuur.VerhuurDatum = DateTime.Now;
                verhuur.KlantNr = mandjeitem.klant.KlantNr;
                db.MaakRecord(verhuur,(Klant)Session["klant"]);
               
                totaleprijs += mandjeitem.film.Prijs;
                

            }
            Session["klant"] = null;
            db.UpdateAlleFilms(winkelmand);
            Session["TotalePrijs"] = totaleprijs;
            return View(winkelmand);
        }
    }
}