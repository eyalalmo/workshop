using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class DiscountComposite : DiscountComponent
    {
       // public enum Type {or, and, xor};
        private List<DiscountComponent> children;
        private String type;

        public DiscountComposite(List<DiscountComponent> children, String type)
        {
            this.type = type;
            this.children = children ?? throw new IllegalAmountException();
            this.type = type;
          
        }
        public bool checkCondition()
        {
            throw new NotImplementedException();
        }

        

    }
}
