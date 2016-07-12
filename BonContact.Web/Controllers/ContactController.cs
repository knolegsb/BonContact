using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BonContact.Web.DAL;
using BonContact.Web.Entities;
using System.Data.Entity.Infrastructure;
using BonContact.Web.Models;
using PagedList;

namespace BonContact.Web.Controllers
{
    public class ContactController : Controller
    {
        private BonContactContext db = new BonContactContext();

        // GET: Contact
        //public ActionResult Index()
        //{
        //    return View(db.Contacts.ToList());
        //}

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var contacts = db.Contacts.Select(c => c)
                                        .Where(c => string.IsNullOrEmpty(searchString) 
                                                    || c.FirstName.Contains(searchString) 
                                                    || c.LastName.Contains(searchString));

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    var result = contacts.Where(c => c.LastName.Contains(searchString) 
            //                                || c.FirstName.Contains(searchString));  
            //}



            switch (sortOrder)
            {
                case "name_desc":
                    contacts = contacts.OrderByDescending(c => c.LastName);
                    break;
                case "Date":
                    contacts = contacts.OrderBy(c => c.DateAdded);
                    break;
                case "date_desc":
                    contacts = contacts.OrderByDescending(c => c.DateAdded);
                    break;
                default:
                    contacts = contacts.OrderBy(c => c.LastName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(contacts.ToPagedList(pageNumber, pageSize));
        }

        // GET: Contact/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            
            return View(contact);
        }

        // GET: Contact/Create
        public ActionResult Create()
        {
            //var contact = new Contact();
            //contact.Address = new List<Address>();  
            return View();
        }

        // POST: Contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName,FirstName,DateAdded,Interests, Address")] Contact contact, HttpPostedFileBase upload, Address address)
        {
            
            //if (ModelState.IsValid)
            //{
            //    db.People.Add(contact);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        var newImage = new File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Photo,
                            ContentType = upload.ContentType
                        };
                        using(var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            newImage.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        contact.Files = new List<File> { newImage };
                    }

                    //var newAddress = db.Addresses.Find(contact.ID);
                    //if (address != null)
                    //{
                    //    newAddress.Line1 = address.Line1;
                    //    newAddress.Line2 = address.Line2;
                    //    newAddress.City = address.City;
                    //    newAddress.State = address.State;
                    //    newAddress.ZipCode = address.ZipCode;
                    //    newAddress.Country = address.Country;
                    //}
                    //contact.Address = new Address()
                    //{
                    //    Line1 = Address.Line1,

                    //};

                    if (address != null)
                    {
                        var newAddress = new Address
                        {
                            Line1 = address.Line1,
                            Line2 = address.Line2,
                            City = address.City,
                            State = address.State,
                            ZipCode = address.ZipCode,
                            Country = address.Country
                        };
                        
                        contact.Address = new List<Address> { newAddress };
                    }

                    db.Contacts.Add(contact);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(contact);
        }

        // GET: Contact/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Contact contact = db.Contacts.Find(id);
            Contact contact = db.Contacts.Include(s => s.Files).SingleOrDefault(s => s.ID == id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,LastName,FirstName,DateAdded,Interests")] Contact contact)
        public ActionResult Edit(int? id, HttpPostedFileBase upload)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(contact).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(contact);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contactToUpdate = db.Contacts.Find(id);
            if(TryUpdateModel(contactToUpdate, "", new string[] { "LastName", "FirstName", "DateAdded" }))
            {
                try
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        if (contactToUpdate.Files.Any(f => f.FileType == FileType.Photo))
                        {
                            db.Files.Remove(contactToUpdate.Files.First(f => f.FileType == FileType.Photo));
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
                    db.Entry(contactToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(contactToUpdate);
        }

        // GET: Contact/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Contact contact = db.Contacts.Find(id);
                db.People.Remove(contact);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
