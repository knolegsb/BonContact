using BonContact.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BonContact.Web.Models
{
    public class ContactViewModel
    {
        public IEnumerable<Contact> Contacts { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<File> Files { get; set; }

        public PagingInfoViewModel PagingInfo { get; set; }
    }
}