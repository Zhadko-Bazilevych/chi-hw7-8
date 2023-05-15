using System;
using System.Collections.Generic;

namespace HW7_8.Models
{
    public partial class Expense
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public double Cost { get; set; }
        public string? Commentary { get; set; }
        public DateTime Datetime { get; set; }

        public virtual Category Category { get; set; } = null!;
    }
}
