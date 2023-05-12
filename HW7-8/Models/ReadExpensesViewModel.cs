using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HW7_8.Models
{
    public class ReadExpensesViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательное")]
        public double Cost { get; set; }
        public string? Commentary { get; set; }

        [Required(ErrorMessage = "Это поле обязательное")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy hh:mm:ss}")]
        public DateTime Datetime { get; set; }
        public string? CategoryName { get; set; }
    }
}
