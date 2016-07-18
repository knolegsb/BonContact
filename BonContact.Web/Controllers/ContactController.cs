using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private IContactRepository _repo;

        public int PageSize = 4;

        public ContactController(IContactRepository repo)
        {
            this._repo = repo;
        }

        public ViewResult Index(string searchString, int page = 1)
        {
            var contacts = _repo.GetAllContacts().OrderBy(c => c.ID).Skip((page - 1)*PageSize).Take(PageSize);
            var searchContacts = _repo.GetAllContacts().Where(c => string.IsNullOrEmpty(searchString)
                            || c.FirstName.Contains(searchString)
                            || c.FirstName.ToLower().Contains(searchString)
                            || c.FirstName.ToUpper().Contains(searchString)
                            || c.LastName.Contains(searchString)
                            || c.LastName.ToLower().Contains(searchString)
                            || c.LastName.ToUpper().Contains(searchString)
                            || c.Interests.Contains(searchString)
                            || c.Interests.ToLower().Contains(searchString)
                            || c.Interests.ToUpper().Contains(searchString));

            ContactViewModel viewModel = new ContactViewModel()
            {
                Contacts = searchString == null ? contacts : searchContacts,
                PagingInfo = new PagingInfoViewModel()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = searchString == null ? _repo.GetAllContacts().Count() : searchContacts.Count() 
                }
            };
            
            return View(viewModel);
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
            
            return View("Details", contact);
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
