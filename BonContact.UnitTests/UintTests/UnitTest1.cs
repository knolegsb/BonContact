using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BonContact.Web.Abstract;
using Moq;
using BonContact.Web.Entities;
using System.Collections.Generic;
using System.Linq;
using BonContact.Web.Controllers;
using BonContact.Web.Concrete;
using BonContact.Web.Models;

namespace BonContact.UnitTests.UintTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_For_Pagination()
        {
            // Arrange
            Mock<IContactRepository> mock = new Mock<IContactRepository>();
            mock.Setup(m => m.GetAllContacts()).Returns(new List<Contact>
            {
                new Contact() {FirstName = "Sean1", LastName = "John1", Interests = "Shopping1"},
                new Contact() {FirstName = "Sean2", LastName = "John2", Interests = "Shopping2"},
                new Contact() {FirstName = "Sean3", LastName = "John3", Interests = "Shopping3"},
                new Contact() {FirstName = "Sean4", LastName = "John4", Interests = "Shopping4"},
                new Contact() {FirstName = "Sean5", LastName = "John5", Interests = "Shopping5"},
            });

            ContactController controller = new ContactController(mock.Object);
            controller.PageSize = 3;

            // Act
            ContactViewModel result = (ContactViewModel) controller.Index(2).Model;

            // Assert
            ContactViewModel contactList = result;
            //Assert.IsTrue(contactList. == 2);
            Assert.AreEqual(result.PagingInfo.CurrentPage, 2);
            Assert.AreEqual(result.Contacts.Take(1).First().LastName, "John4");
            Assert.AreEqual(result.Contacts.Count(), 2);
            Assert.AreEqual(result.Contacts.OrderByDescending(c => c.Interests).Take(1).First().Interests, "Shopping5");
        }
    }
}
