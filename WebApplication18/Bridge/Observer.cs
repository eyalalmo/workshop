using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Bridge
{
    public interface Observer
    {
        void observe(Messager m);
    }
}
