using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BonContact.Web.Models
{
    public class SearchViewModel
    {
        public string currentFilter { get; set; }
        public string searchString { get; set; }
    }
}