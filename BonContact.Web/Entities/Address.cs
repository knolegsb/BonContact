﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BonContact.Web.Entities
{
    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public int ContactID { get; set; }
        public virtual Contact Contact { get; set; }
    }
}