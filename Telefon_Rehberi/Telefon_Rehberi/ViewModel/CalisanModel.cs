using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Telefon_Rehberi.ViewModel
{
    public class CalisanModel
    {
        public int CalisanId { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string Telefon { get; set; }
        public string Departman { get; set; }
        [Display(Name ="Yönetici Bilgisi")]
        public string Yonetici { get; set; }

    }
}