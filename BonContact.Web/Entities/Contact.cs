using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BonContact.Web.Entities
{
    public class Contact : Person
    {
        public DateTime DateAdded { get; set; }
        public virtual ICollection<Address> Address { get; set; }
    }
}