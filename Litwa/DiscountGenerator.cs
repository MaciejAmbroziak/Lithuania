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

        private IDiscountContext _discountContext;

        public DiscountGenerator(int numberOfDiscounts)
        {
            NumberOfDiscounts = numberOfDiscounts;
            Discounts = new List<Discount>();
            _discountContext = new DiscountContext();
        }

        public void CreateDiscount(int length)
        {
            string discountCode = Guid.NewGuid().ToString().Take(length).ToString()!;
            Discount discount = new Discount()
            {
                DiscountCode = discountCode,
                Used = false,
                Length = length
            };
            using(var dbContext = new DiscountContext())
            {
                dbContext.Add(discount);
                dbContext.SaveChanges();
            }
        }

        public void CreateDiscounts(int length)
        {
            List<Discount> discountList = _discountContext.Discounts.ToList();
            for (int i = 0; i < NumberOfDiscounts; i++)
            {
                string discountCode = Guid.NewGuid().ToString().Substring(0, length);
                Discount discount = new Discount()
                {
                    DiscountCode = discountCode,
                    Used = false,
                    Length = length
                };
                if (discountList.IsNullOrEmpty())
                {
                    discountList.Add(discount);
                }
                if (discountList.Any(x => x.DiscountCode != discount.DiscountCode))
                {
                    discountList.Add(discount);
                }
            }
            using (var dbContext = new DiscountContext())
            {
                dbContext.AddRange(discountList);
                dbContext.SaveChanges();
            }
            

        }
        public Discount GetDiscount(bool used)
        {
            return _discountContext.Discounts.Where(x => x.Used == used).FirstOrDefault();
        }

        public IQueryable<Discount> GetDiscounts(bool used, int numberOfDiscounts)
        {
            return _discountContext.Discounts.Where(x => x.Used == used).Take(numberOfDiscounts);
        }

        public int GetNumberOfDiscounts()
        {
            int i = _discountContext.Discounts.Count();
            return i;
        }
    }
}
