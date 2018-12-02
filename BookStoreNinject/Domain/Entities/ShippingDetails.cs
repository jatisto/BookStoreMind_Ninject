using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Укажите Ваше имя")]
        [Display(Name = "Введите имя:")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Адрес отправки")]
        [Display(Name = "Первый адрес:")]
        public string Line1 { get; set; }

        [Display(Name = "Второй адрес:")] public string Line2 { get; set; }
        [Display(Name = "Третий адрес:")] public string Line3 { get; set; }

        [Required(ErrorMessage = "Укажите город")]
        [Display(Name = "Город:")]
        public string City { get; set; }

        [Required(ErrorMessage = "Укажите  страну")]
        [Display(Name = "Cтрану:")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}