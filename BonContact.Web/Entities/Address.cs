using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BonContact.Web.Entities
{
    public class Address
    {
        [HiddenInput(DisplayValue=false)]
        public int AddressID { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public AddressType? AddressType { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ContactID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public virtual Contact Contact { get; set; }
    }
}