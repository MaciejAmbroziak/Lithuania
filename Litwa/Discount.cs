using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Litwa
{
    public class Discount
    {
        public int Id { get; set; }
        public int Length { get; set; }
        public bool Used { get; set; }
        public string DiscountCode { get; set; }
        public bool Sent { get; set; }

        public Discount(int length, bool used, string discountCode, bool sent)
        {
            Length = length;
            Used = used;
            DiscountCode = discountCode;
            Sent = sent;
        }
    }
}
