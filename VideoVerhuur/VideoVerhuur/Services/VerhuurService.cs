using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoVerhuur.Models;
namespace VideoVerhuur.Services
{
    public class VerhuurService
    {
        public Klant GetKlant(string naam, string postcode)
        {
            using (var db = new VideoVerhuurEntities())
            {
                return (from klant in db.Klants
                        where klant.Naam == naam.ToUpper() && klant.PostCode.ToString() == postcode
                        select klant).FirstOrDefault();
            }
        }
        public List<Genre> GetAlleGenres()
        {
            using (var db = new VideoVerhuurEntities())
            {
                return db.Genres.OrderBy(m => m.Genre1).ToList();
            }
        }
        public Genre GetGenre(int? id)
        {
            using (var db = new VideoVerhuurEntities())
            {
                return db.Genres.Find(id);
            }
        }
        public Film GetFilm(int? BandNr)
        {
            using (var db = new VideoVerhuurEntities())
            {
                var query = (from film in db.Films
                             where film.BandNr == BandNr
                             select film).FirstOrDefault();
                return query;
            }
        }
        public List<Film> GetAlleFilmsVanEenGenre(int? genrenr)
        {
            using (var db = new VideoVerhuurEntities())
            {
                var query = from film in db.Films.Include("Genre")
                            where film.GenreNr == genrenr
                            orderby film.Titel
                            select film;
                return query.ToList();
            }
        }
        public bool UpdateAlleFilms(List<MandjeItem> Mandje)
        {
            using (var db = new VideoVerhuurEntities())
            {
                foreach (var item in Mandje)
                {
                    db.Films.Find(item.film.BandNr).InVoorraad -= 1;
                    db.Films.Find(item.film.BandNr).UitVoorraad += 1;
                    db.Films.Find(item.film.BandNr).TotaalVerhuurd += 1;
                    item.film.InVoorraad -= 1;
                    item.film.UitVoorraad += 1;
                    db.SaveChanges();
                }
                return true;

            }

        }
        public void MaakRecord(Verhuur verhuur, Klant klant)
        {
            using (var db = new VideoVerhuurEntities())
            {
                db.Klants.Find(klant.KlantNr).HuurAantal += 1;
                db.Verhuurs.Add(verhuur);
                db.SaveChanges();
            }
        }


    }
}