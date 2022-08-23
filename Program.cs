using Sales_taxes.DTO;

namespace Sales_taxes
{
    public class Program
    {
        static void Main(string[] args)
        {
            var exitFlag = true;
            var salesService = new SalesService();

            do
            {
                try
                {
                    Console.WriteLine("Write your sales --Write Exit to quit --Write End to finish a bill");

                    string? input = Console.ReadLine();                    

                    if (input?.Trim().ToLower() == "exit")
                    {
                        exitFlag = false;
                    }

                    if (input?.Trim().ToLower() == "end")
                    {
                        exitFlag = false;
                        PrintBill(salesService.GetBillResponse());
                        break;
                    }

                    var response = salesService.parseText(input);

                    if (!response.Success)
                    {
                        Console.WriteLine($"{response.Error}");
                    }
                    else
                    {
                        Console.WriteLine("Entered>> " +
                            $"\nQuantity:{response.SaleItem.Quantity} " +
                            $"\nDescription:{response.SaleItem.Description} " +
                            $"\nPrice:{response.SaleItem.Price} " +
                            $"\nTaxes:{response.SaleItem.Taxes}" +
                            $"\nTotal:{response.SaleItem.FinalPrice}");
                    }                    

                }
                catch(Exception ex)
                {
                    
                    Console.WriteLine(ex.Message);
                }
            } while(exitFlag);
            
        }

        static void PrintBill(BillResponse billResponse)
        {
            foreach (var item in billResponse.SaleItems)
            {
                var detail = item.Quantity > 1 ? $"({item.Quantity} @ {item.FinalPrice})" : string.Empty;

                Console.WriteLine($"{item.Description}: {item.FinalPrice} {detail}");
            }
            Console.WriteLine($"Sales Taxes: {billResponse.TotalTaxes}");
            Console.WriteLine($"Total: {billResponse.Total}");
        }
    }
}
