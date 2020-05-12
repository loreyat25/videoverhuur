using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoVerhuur.Models
{
    public class FilmViewModel
    {
        public Genre Genre { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Film> Films { get; set; }
        public Klant klant { get; set; }
    }
}