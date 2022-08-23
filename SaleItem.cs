using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales_taxes
{
    public class SaleItem
    {
        public string Description { get; set; }
        
        public int Quantity { get; set; }
        
        public decimal Price { get; set; }

        public decimal Taxes { get; set; }

        public decimal FinalPrice { get; set; }

        public bool hasExceptions { get; set; }
        public bool IsImported { get; set; }

        public SaleItem()
        {

        }
    }
}
