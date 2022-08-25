using Sales_taxes.DTO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace Sales_taxes
{
    public class SalesService
    {
        private const string pattern = "(^|\\.\\s+)\\d+\\s[\\b\\w*\\b]+at\\s(\\d+(?:[.,]\\d+)?)$";
        private const decimal basicTax = 10;
        private const decimal importTax = 5;
        private List<SaleItem> _saleItems = new List<SaleItem>();
        //This emulates a repository with the excluded categories
        private List<ItemCategories> _excludedCategories = new List<ItemCategories>() { ItemCategories.Medicine, ItemCategories.Food, ItemCategories.Book};

        public BillResponse GetBillResponse()
        {
            BillResponse billResponse = new BillResponse();

            //Mocking repository query
            billResponse.SaleItems = _saleItems.ToList();
            calculateTotals(billResponse);

            return billResponse;
        }

        public void calculateTotals(BillResponse bill)
        {
            bill.Total = bill.SaleItems.Select(x => x.FinalPrice * x.Quantity).Sum();
            bill.TotalTaxes = bill.SaleItems.Select(x => x.Taxes * x.Quantity).Sum();
        }
        
        public SaleResponse parseText(string input)
        {
            decimal price = 0;
            int quantity = 0;
            string category = string.Empty;

            SaleResponse response = new SaleResponse();
            SaleItem item = new SaleItem();

            if (!isValid(input))
            {
                response.Error = "Invalid input";
            }

            var stringParts = input.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if (stringParts.Length < 4)
            {
                response.Error = "Invalid format";
            }

            if (!int.TryParse(stringParts[0], out quantity))
            {
                response.Error = "Error parsing item quantity";
            }
            else
            {
                item.Quantity = quantity;
            }

            if (!decimal.TryParse(stringParts[stringParts.Length - 1], out price))
            {
                response.Error = "Error parsing item price";
            }
            else
            {
                item.Price = price;
            }

            if (item.Price <= 0)
            {
                response.Error = "Invalid price entered";
            }

            if (stringParts.Length >= 5)
            {
                category = stringParts[stringParts.Length - 3];
                item.Category = GetItemCategory(category);

                if (item.Category != ItemCategories.Unknown)
                {
                    item.Description = string.Join(' ', stringParts.Skip(1).Take(stringParts.Length - 4));
                }
            }
            else//where only one word is written
            {
                category = stringParts[1];
                item.Category = GetItemCategory(category);
                item.Description = category;
            }            

            if (item.Description.ToLower().Contains("imported"))
            {
                item.IsImported = true;
            }
            
            item.hasExceptions = _excludedCategories.Contains(item.Category);

            response.SaleItem = item;
            
            response.Success = string.IsNullOrEmpty(response.Error);

            calculateTaxes(item);
            addToBill(item);

            return response;
        }

        private ItemCategories GetItemCategory(string input)
        {
            ItemCategories output = ItemCategories.Unknown;

            var enumString = Enum.GetNames(typeof(ItemCategories)).ToList();            
            input = input.ToLower();

            var firstMatch = enumString.FirstOrDefault(_ => input.Contains(_.ToLower()));

            if (firstMatch != default)
            {
                Enum.TryParse<ItemCategories>(firstMatch, out output);
            }
            
            return output;            
        }

        private bool isValid(string input)
        {
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(input);
        }

        private void addToBill(SaleItem item)
        {
            var existingItem = _saleItems.FirstOrDefault(_ => _.Description.ToLower() == item.Description.ToLower() && _.Price == item.Price);

            if (existingItem is not null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                _saleItems.Add(item);
            }            
        }

        private void calculateTaxes(SaleItem item)
        {
            decimal basic = 0;
            decimal import = 0;
            
            if (!item.hasExceptions)
            {
                basic = Math.Round((item.Price * basicTax / 100) * 5, 2) / 5;
            }

            if (item.IsImported)
            {
                import = Math.Round((item.Price * importTax / 100)* 5, 2) / 5;
            }

            item.Taxes = (basic + import);
            item.FinalPrice = (item.Price + basic + import);
        }
    }    
}