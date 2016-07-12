using BonContact.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BonContact.Web.Abstract
{
    public interface IContactRepository
    {
        Contact GetContact(int? id);
    }
}