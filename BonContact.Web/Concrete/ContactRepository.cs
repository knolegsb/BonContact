using BonContact.Web.Abstract;
using BonContact.Web.DAL;
using BonContact.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;

namespace BonContact.Web.Concrete
{
    public class ContactRepository : IContactRepository
    {
        private BonContactContext _context;
        public ContactRepository(BonContactContext context)
        {
            _context = context;
        }

        public Contact GetContact(int? id)
        {
            var contacts = _context.Contacts.Find(id);
            return contacts;
        }

        public List<Contact> GetAllContacts()
        {
            var contatcs = _context.Contacts.ToList();
            return contatcs;
        }
        
        public Contact GetContactWithFiles(int? id)
        {
            Contact contact = _context.Contacts.Include(c => c.Files).SingleOrDefault(c => c.ID == id);
            return contact;
        }
        public void AddContact(Contact contact)
        {
            _context.Contacts.Add(contact);
        }

        public void ImageUpdate(int? id, HttpPostedFileBase upload)
        {
            var contactToUpdate = _context.Contacts.Find(id);

            if (upload != null && upload.ContentLength > 0)
            {
                if (contactToUpdate.Files.Any(f => f.FileType == FileType.Photo))
                {
                    _context.Files.Remove(contactToUpdate.Files.First(f => f.FileType == FileType.Photo));
                }
                var newImage = new File
                {
                    FileName = System.IO.Path.GetFileName(upload.FileName),
                    FileType = FileType.Photo,
                    ContentType = upload.ContentType
                };
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    newImage.Content = reader.ReadBytes(upload.ContentLength);
                }
                contactToUpdate.Files = new List<File> { newImage };
            }
            _context.Entry(contactToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void RemoveContact(int id)
        {
            Contact contact = _context.Contacts.Find(id);
            _context.People.Remove(contact);
            _context.SaveChanges();
        }
        public void DbSaveChanges()
        {
            _context.SaveChanges();
        }
        public void DbDispose()
        {
            _context.Dispose();
        }
    }
}