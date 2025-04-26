using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class OrderCountViewModel
    {
        [Key]
        public string UserID { get; set; }
        public int OrderCount { get; set; }
    }
}
