using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Litwa
{
    internal class DiscountResponse
    {
        public List<Discount> Discounts {get;set;}
        public string Note { get; set; }
        public bool Possible { get; set; }

        public DiscountResponse(List<Discount> discounts, string note, bool possible)
        {
            Discounts = discounts;
            Note = note;
            Possible = possible;
        }
    }
}
