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

        public void addComplaint(Complaint c)
        {
            complaints.AddFirst(c);
        }
        public void removeComplaint(Complaint c)
        {
            if (!complaints.Contains(c))
                throw new DoesntExistException("complaint doesn't exist");
            complaints.Remove(c);
        }

        public LinkedList<Complaint> getComplaints()
        {
            return this.complaints;
        }
    }
}
