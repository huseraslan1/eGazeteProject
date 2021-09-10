using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_GazeteUyg.Models.DTO
{
    public class HomeDTO
    {
        public int gazeteID{ get; set; }
        public string gazeteAdi { get; set; }
        public DateTime gazeteTarih { get; set; }
        public string gazetePdf { get; set; }
        public string gazeteImage { get; set; }
        public int gazeteEkID { get; set; }
        public string gazeteEkAdi { get; set; }
        public bool Uyelik { get; set; }
    }
}