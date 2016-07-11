using BonContact.Web.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BonContact.Web.Controllers
{
    public class FileController : Controller
    {
        private BonContactContext db = new BonContactContext();

        // GET: File
        public ActionResult Index(int id)
        {
            var fileToRetrive = db.Files.Find(id);
            return File(fileToRetrive.Content, fileToRetrive.ContentType);
        }
    }
}