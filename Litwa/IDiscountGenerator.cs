
namespace Litwa
{
    public interface IDiscountGenerator
    {
        List<Discount> Discounts { get; set; }
        int NumberOfDiscounts { get; set; }
        public Range Range { get; set; }
        void CreateDiscount(int length);
        void CreateDiscounts(int length);
        Discount GetDiscount(bool used);
        IQueryable<Discount> GetDiscounts(bool used, int numberOfDiscounts);
        int GetNumberOfDiscounts();
    }
}