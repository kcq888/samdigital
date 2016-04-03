using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmaBusiness
{
    public class User
    {
        public Guid AccountId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Contact PrimaryCarePhysician { get; set; }

        public List<Contact> EmergencyContacts { get; set; }

        public string ImageUrl { get; set; }

        public User()
        {
            EmergencyContacts = new List<Contact>();
        }
    }

    public class Contact
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set;  }
    }
}
