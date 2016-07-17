using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BonContact.Web.Abstract;
using Moq;
using BonContact.Web.Entities;
using System.Collections.Generic;
using BonContact.Web.Controllers;
using BonContact.Web.Concrete;

namespace BonContact.UnitTests.UintTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Mock<IContactRepository> mock = new Mock<IContactRepository>();
            mock.Setup(m => m.GetAllContacts()).Returns(new List<Contact>
            {
                new Contact() {FirstName = "Sean", LastName = "John", Interests = "Shopping"},
                new Contact() {FirstName = "Sean", LastName = "John", Interests = "Shopping"},
                new Contact() {FirstName = "Sean", LastName = "John", Interests = "Shopping"},
                new Contact() {FirstName = "Sean", LastName = "John", Interests = "Shopping"},
                new Contact() {FirstName = "Sean", LastName = "John", Interests = "Shopping"},
            });
        }
    }
}
