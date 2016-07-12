using BonContact.Web.Abstract;
using BonContact.Web.DAL;
using BonContact.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BonContact.Web.Concrete
{
    public class ContactRepository : IContactRepository
    {
        private readonly BonContactContext _context;
        public ContactRepository(BonContactContext context)
        {
            _context = context;
        }

        public Contact GetContact(int? id)
        {
            var contacts = _context.Contacts.Find(id);
            return contacts;
        }

        public void GetNewImage()
        {

        }
    }
}