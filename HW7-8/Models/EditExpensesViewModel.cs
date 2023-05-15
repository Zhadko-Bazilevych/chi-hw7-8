using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HW7_8.Models
{
    public class EditExpensesViewModel : ReadExpensesViewModel
    {
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem>? SelectCategories { get; set; }
    }
}
