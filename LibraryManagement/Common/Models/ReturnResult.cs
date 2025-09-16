using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Common.Models
{
    public class ReturnResult
    {
        public bool Success { get; set; }
        public decimal LateFee { get; set; }
        public int DaysLate { get; set; }
        public string Message { get; set; }
    }
}
