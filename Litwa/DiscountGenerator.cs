using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Litwa
{
    public class DiscountGenerator : IDiscountGenerator
    {
        public List<Discount> Discounts { get; set; }
        public int NumberOfDiscounts { get; set; }
        public Range Range { get; set; }

        public DiscountGenerator(int numberOfDiscounts)
        {
            NumberOfDiscounts = numberOfDiscounts;
            Discounts = new List<Discount>();
        }

        public void CreateDiscount(int length)
        {
            string discountCode = Guid.NewGuid().ToString().Take(length).ToString()!;
            Discount discount = new Discount(length, false, discountCode, false);
            using(var dbContext = new DiscountContext())
            {
                dbContext.Add(discount);
                dbContext.SaveChanges();
            }
        }

        public void CreateDiscounts(int length)
        {
            using (var dbContext = new DiscountContext())
            {
                List<Discount> discountList = dbContext.Discounts.ToList();
                for (int i = 0; i < NumberOfDiscounts; i++)
                {
                    string discountCode = Guid.NewGuid().ToString().Substring(0, length);
                    Discount discount = new Discount(length, false, discountCode, false);
                    if (discountList.IsNullOrEmpty())
                    {
                        discountList.Add(discount);
                    }
                    if (discountList.Any(x => x.DiscountCode != discount.DiscountCode))
                    {
                        discountList.Add(discount);
                    }
                }

                dbContext.AddRange(discountList);
                dbContext.SaveChanges();
            }
        }
        public Discount GetDiscount()
        {
            Discount result;
            using (var dbContext = new DiscountContext())
            {
                result = dbContext.Discounts.Where(x => !x.Used).FirstOrDefault()!;
            }
            return result;
        }

        public List<Discount> GetDiscounts(int numberOfDiscounts)
        {
            List<Discount> result;
            using (var dbContext = new DiscountContext())
            {
                result = dbContext.Discounts.Where(x => !x.Used).Where(y => !y.Sent).Take(numberOfDiscounts).ToList();
                foreach (var discount in result)
                {
                    discount.Sent = true;
                    dbContext.SaveChanges();
                }
            }
            return result;
        }
        public int GetNumberOfDiscounts()
        {
            throw new NotImplementedException();
        }
    }
}
