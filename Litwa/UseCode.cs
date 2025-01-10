using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Litwa
{
    public class UseCode : IUseCode
    {
        string message = "Thank you";
        public UseCode()
        {
        }

        public bool Use(string code)
        {
            try
            {
                using(var dbContext = new DiscountContext())
                {
                    Discount discount = dbContext.Discounts.Where(x => x.DiscountCode.Equals(code, StringComparison.OrdinalIgnoreCase)).First();
                    discount.Used = true;
                    dbContext.SaveChanges();
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                message = "Code already used!!!";
                Console.WriteLine(message);
            }
            return true;
        }
    }
}
