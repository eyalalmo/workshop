using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class ConsistencySystem
    {
        private static ConsistencySystem instance = null;

        private ConsistencySystem()
        {


        }

        public static ConsistencySystem getInstance()
        {
            if (instance == null)
            {
                instance = new ConsistencySystem();
            }
            return instance;
        }

        public bool connectToSystem()
        {
            return true;
        }
    }
}
