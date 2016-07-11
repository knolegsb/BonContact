using BonContact.Web.Entities;
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
            ////base.Seed(context);
            var contacts = new List<Contact>
            {
                new Contact { FirstName = "Sean", LastName = "John", DateAdded = DateTime.Parse("2016-02-21"), Interests = "Sports"},
                new Contact { FirstName = "Brad", LastName = "Pit", DateAdded = DateTime.Parse("2015-02-03"), Interests = "Reading Books"},
                new Contact { FirstName = "Angelina", LastName = "Jollie", DateAdded = DateTime.Parse("2016-03-04"), Interests = "Reading Books"},
                new Contact { FirstName = "Alicia", LastName = "Keys", DateAdded = DateTime.Parse("2016-04-17"), Interests = "Watching Movies"},
                new Contact { FirstName = "Mandolin", LastName = "Greg", DateAdded = DateTime.Parse("2016-01-11"), Interests = "Traveling"},
                new Contact { FirstName = "Gregory", LastName = "Johnson", DateAdded = DateTime.Parse("2016-05-16"), Interests = "Mountain Climbing"},
                new Contact { FirstName = "Mansur", LastName = "Kiffur", DateAdded = DateTime.Parse("2016-02-01"), Interests = "Breathing"},
                new Contact { FirstName = "Anderson", LastName = "Nguyen", DateAdded = DateTime.Parse("2016-06-11"), Interests = "Reading Books"},
                new Contact { FirstName = "GaGa", LastName = "Minerva", DateAdded = DateTime.Parse("2016-04-21"), Interests = "Watching Movies"},
                new Contact { FirstName = "Georgia", LastName = "Ann", DateAdded = DateTime.Parse("2016-05-01"), Interests = "Reading Books"},
            };

            contacts.ForEach(c => context.Contacts.Add(c));
            context.SaveChanges();

            var addresses = new List<Address>
            {
                new Address { Line1 = "2011 Wilshire", City = "Los Angeles", ZipCode = "90010", State = "California", Country = "USA", ContactID = 1 },
                new Address { Line1 = "3011 Normandie", City = "Los Angeles", ZipCode = "90011", State = "California", Country = "USA", ContactID = 2 },
                new Address { Line1 = "4011 Gilber", City = "Los Angeles", ZipCode = "90012", State = "California", Country = "USA", ContactID = 3 },
                new Address { Line1 = "6011 Western", City = "Los Angeles", ZipCode = "92010", State = "California", Country = "USA", ContactID = 4 },
                new Address { Line1 = "711 3TH", City = "Los Angeles", ZipCode = "90510", State = "California", Country = "USA", ContactID = 5 },
                new Address { Line1 = "71 De Mar", City = "Los Angeles", ZipCode = "94010", State = "California", Country = "USA", ContactID = 6 },
                new Address { Line1 = "1011 Madrid", City = "Los Angeles", ZipCode = "97010", State = "California", Country = "USA", ContactID = 7 },
                new Address { Line1 = "811 San Vincente", City = "Los Angeles", ZipCode = "90110", State = "California", Country = "USA", ContactID = 8 },
                new Address { Line1 = "80011 West Hollywood", City = "Los Angeles", ZipCode = "94010", State = "California", Country = "USA", ContactID = 9 },
                new Address { Line1 = "32011 Vermont", City = "Los Angeles", ZipCode = "90910", State = "California", Country = "USA", ContactID = 10 },
            };

            addresses.ForEach(a => context.Addresses.Add(a));
            context.SaveChanges();
        }
    }
}