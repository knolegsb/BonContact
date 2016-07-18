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

        [TestMethod]
        public void Test_For_Pagination_View_Model()
        {
            // Arrange
            Mock<IContactRepository> mock = new Mock<IContactRepository>();
            mock.Setup(m => m.GetAllContacts()).Returns(new List<Contact>
            {
                new Contact() {FirstName = "Sean1", LastName = "John1", Interests = "Shopping1", Address = new List<Address>()
                {
                    new Address() { Line1 = "2011 Wilshire Blvd_1", Line2 = "", City = "Los Angeles1", ZipCode = "90010_1", State = "California1", Country = "USA1"}
                } },
                new Contact() {FirstName = "Sean2", LastName = "John2", Interests = "Shopping2", Address = new List<Address>()
                {
                    new Address() { Line1 = "2011 Wilshire Blvd_2", Line2 = "", City = "Los Angeles2", ZipCode = "90010_2", State = "California2", Country = "USA2"}
                } },
                new Contact() {FirstName = "Sean3", LastName = "John3", Interests = "Shopping3", Address = new List<Address>()
                {
                    new Address() { Line1 = "2011 Wilshire Blvd_3", Line2 = "", City = "Los Angeles3", ZipCode = "90010_3", State = "California3", Country = "USA3"}
                } },
                new Contact() {FirstName = "Sean4", LastName = "John4", Interests = "Shopping4", Address = new List<Address>()
                {
                    new Address() { Line1 = "2011 Wilshire Blvd_4", Line2 = "", City = "Los Angeles4", ZipCode = "90010_4", State = "California4", Country = "USA4"}
                } },
                new Contact() {FirstName = "Sean5", LastName = "John5", Interests = "Shopping5", Address = new List<Address>()
                {
                    new Address() { Line1 = "2011 Wilshire Blvd_5", Line2 = "", City = "Los Angeles5", ZipCode = "90010_5", State = "California5", Country = "USA5"}
                } },
            });

            // Arrange
            ContactController controller = new ContactController(mock.Object);
            controller.PageSize = 3;

            // Act
            ContactViewModel result = (ContactViewModel) controller.Index(2).Model;

            // Assert
            PagingInfoViewModel pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
            Assert.AreEqual(result.Contacts.Take(1).First().Address.Count, 1);
            Assert.AreEqual(result.Contacts.Take(1).First().Address.First().City, "Los Angeles4");
            Assert.AreEqual(result.Contacts.Skip(1).First().Address.First().City, "Los Angeles5");
        }
    }
}
