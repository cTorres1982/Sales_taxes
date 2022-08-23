using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales_taxes.DTO
{
    public class SaleResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public SaleItem SaleItem { get; set; }
    }
}
