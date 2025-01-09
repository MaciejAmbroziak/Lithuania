using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Litwa
{
    public class DiscountGenerator
    {
        public List<Discount> Discounts { get; set; }
        public int NumberOfDiscounts { get; set; }
        public object Range { get; set; }
        private DiscountContext _discountContext;
        private DiscountRequest _discountRequest; 

        public DiscountGenerator(int numberOfDiscounts, DiscountRequest discountRequest)
        {
            NumberOfDiscounts = numberOfDiscounts;
            Discounts = new List<Discount>();
            _discountContext = new DiscountContext();
            _discountRequest = discountRequest;
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
            using (DiscountContext discountContext = new DiscountContext())
            {
                discountContext.Discounts.Add(discount);
                discountContext.SaveChanges();
            }
        }

        public int CreateDiscounts(int length)
        {
            int discountsCreated = 0;
            for (int i = 0; i < NumberOfDiscounts; i++)
            {
                string discountCode = Guid.NewGuid().ToString().Take(length).ToString()!;
                Discount discount = new Discount()
                {
                    DiscountCode = discountCode,
                    Used = false,
                    Length = length
                };
                
            }
            using (DiscountContext discountContext = new DiscountContext())
            {
                discountContext.Discounts.AddRange(Discounts);
                discountsCreated = discountContext.SaveChanges();
            }
            return discountsCreated;
        }
        public int GetNumberOfDiscounts(bool used)
        {
            return _discountContext.Discounts.
                Where(x => x.Used == used).
                Where(y => y.Length == _discountRequest.Length)
                .Count();
        }

        public Discount GetDiscount(bool used)
        {
            return _discountContext.Discounts.Where(x=>x.Used == used).FirstOrDefault();
        }

        public IQueryable<Discount> GetDiscounts(bool used, int numberOfDiscounts) 
        {
            return _discountContext.Discounts.Where(x => x.Used == used).Take(numberOfDiscounts);
        }

    }
}
