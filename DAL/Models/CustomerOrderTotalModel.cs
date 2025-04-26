using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class CustomerOrderTotalModel
    {
        [Key]
        public string CustomerID { get; set; }

        public decimal TotalOrderAmount { get; set; }
    }
}
