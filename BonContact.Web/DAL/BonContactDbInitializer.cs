using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BonContact.Web.DAL
{
    public class BonContactDbInitializer : DropCreateDatabaseIfModelChanges<BonContactContext>
    {
        protected override void Seed(BonContactContext context)
        {
            //base.Seed(context);
        }
    }
}