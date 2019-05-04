using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class DiscountComposite : Discount
    {
        enum Type {or, and, xor};
        private List<Discount> children;
        private Type type;

        public DiscountComposite(List<Discount> children, Type type)
        {
            if (children == null)
            {
                throw new IllegalAmountException();
            }
            this.children = children;
            this.type = type;
        }
        public bool checkCondition()
        {
            throw new NotImplementedException();
        }

    }
}
