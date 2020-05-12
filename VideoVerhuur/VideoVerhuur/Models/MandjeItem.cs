using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoVerhuur.Models;

namespace VideoVerhuur.Models
{
    public class MandjeItem
    {
        public Klant klant { get; set; }
        public Film film { get; set; }
        public DateTime GetDateTime = DateTime.Now;

        public MandjeItem(Klant _klant, Film _film)
        {
            klant = _klant;
            film = _film;
        }

    }
}