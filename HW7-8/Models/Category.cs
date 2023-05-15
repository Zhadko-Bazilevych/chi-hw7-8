using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HW7_8.Models
{
    public partial class Category
    {
        public Category()
        {
            Expenses = new HashSet<Expense>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательное")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
