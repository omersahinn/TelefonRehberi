using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Telefon_Rehberi.ViewModel
{
    public class AdminModel
    {
        public int AdminId { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilemez", AllowEmptyStrings = false)]
        [Display(Name = "E-Posta")]
        public string Eposta { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilemez" ,AllowEmptyStrings =false)]
        [DataType(DataType.Password)]
        [Display(Name = "Eski sifre")]
        public string EskiSifre { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilemez", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string YeniSifre { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilemez", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Doğrula")]
        public string SifreDogrula { get; set; }


    }
}