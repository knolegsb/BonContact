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
using BonContact.Web.Abstract;

namespace BonContact.Web.Controllers
{
    public class ContactController : Controller
    {
        //private BonContactContext db = new BonContactContext();

        private IContactRepository _repo;

        public ContactController(IContactRepository repo)
        {
            this._repo = repo;
        }
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

            var contacts = _repo.GetAllContacts().Select(c => c)
                                        .Where(c => string.IsNullOrEmpty(searchString) 
                                                    || c.FirstName.Contains(searchString) 
                                                    || c.LastName.Contains(searchString));

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
            var contact = _repo.GetContact(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            
            return View(contact);
        }

        // GET: Contact/Create
        public ActionResult Create()
        {            
            return View();
        }

        // POST: Contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName,FirstName,DateAdded,Interests, Address")] Contact contact, HttpPostedFileBase upload, Address address)
        {
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

                    _repo.AddContact(contact);
                        //db.Contacts.Add(contact);
                    _repo.DbSaveChanges();
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
            Contact contact = _repo.GetContactWithFiles(id);
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
        public ActionResult Edit(int? id, HttpPostedFileBase upload)
        {           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contactToUpdate = _repo.GetContact(id);
            if (TryUpdateModel(contactToUpdate, "", new string[] { "LastName", "FirstName", "DateAdded" }))
            {
                try
                {
                    _repo.ImageUpdate(id, upload);

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
            Contact contact = _repo.GetContact(id);
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
                _repo.RemoveContact(id);
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
                _repo.DbDispose();
            }
            base.Dispose(disposing);
        }
    }
}
