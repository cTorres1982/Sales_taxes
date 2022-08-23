using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales_taxes.DTO
{
    public class BillResponse
    {
        public int Id { get; set; }
        public List<SaleItem> SaleItems { get; set; }
        public decimal TotalTaxes { get; set; }
        public decimal Total { get; set; }
    }
}
