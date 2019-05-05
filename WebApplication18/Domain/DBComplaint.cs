using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class DBComplaint
    {
        private static DBComplaint instance;
        private LinkedList<Complaint> complaints;

        public static DBComplaint getInstance()
        {
            if (instance == null)
                instance = new DBComplaint();
            return instance;
        }

        private DBComplaint()
        {
            complaints = new LinkedList<Complaint>();
        }

        public String addComplaint(Complaint c)
        {
            complaints.AddFirst(c);
            return "";
        }
        public String removeComplaint(Complaint c)
        {
            if (!complaints.Contains(c))
                return "ERROR: Complaint does not exist, cannot remove it";
            complaints.Remove(c);
            return "";
        }

        public LinkedList<Complaint> getComplaints()
        {
            return this.complaints;
        }
    }
}
