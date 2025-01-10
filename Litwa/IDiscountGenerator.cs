
namespace Litwa
{
    public interface IDiscountGenerator
    {
        List<Discount> Discounts { get; set; }
        int NumberOfDiscounts { get; set; }
        public Range Range { get; set; }
        void CreateDiscount(int length);
        void CreateDiscounts(int length);
        Discount GetDiscount();
        List<Discount> GetDiscounts(int numberOfDiscounts);
    }
}