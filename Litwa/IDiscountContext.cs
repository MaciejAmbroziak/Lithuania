using Microsoft.EntityFrameworkCore;

namespace Litwa
{
    public interface IDiscountContext
    {
        DbSet<Discount> Discounts { get; set; }
    }
}