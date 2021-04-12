using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SomeBusinessService.Interfaces;
using SomeBusinessService.Models;
using SomeBusinessService.Services;
using System;

namespace SomeBusinessService.Test
{
    [TestClass]
    public class MainBusinessLogicServiceTest
    {
        private MainBusinessLogicService service;
        private Mock<IDBManager> dbManager;

        [TestInitialize]
        public void InitializeEnvironment()
        {
            this.dbManager = new Mock<IDBManager>();
            this.service = new MainBusinessLogicService(dbManager.Object);
        }

        [TestMethod]
        public void Create_Product_DbManagerCallsCreate()
        {
            this.dbManager.Setup(db => db.Create(It.IsAny<Product>()));
            
            var product = new Product() { Name = "TestProductName" };

            this.service.Create(product);

            dbManager.Verify(mock => mock.Create(product), Times.Once);
        }

        [TestMethod]
        public void Create_ProductIsNull_ArgumentExceptionThrow()
        {
            this.dbManager.Setup(db => db.Create(It.IsAny<Product>()));

            Product product = null;

            Assert.ThrowsException<ArgumentException>(() => this.service.Create(product));
        }

        [TestMethod]
        public void Create_ProductNameIsNull_ArgumentExceptionThrow()
        {
            this.dbManager.Setup(db => db.Create(It.IsAny<Product>()));

            var product = new Product();

            Assert.ThrowsException<ArgumentException>(() => this.service.Create(product));
        }

        [TestMethod]
        public void Create_ProductNameIsEmpty_ArgumentExceptionThrow()
        {
            this.dbManager.Setup(db => db.Create(It.IsAny<Product>()));

            var product = new Product() { Name = string.Empty };

            Assert.ThrowsException<ArgumentException>(() => this.service.Create(product));
        }

        [TestMethod]
        public void Delete_Name_DbManagerCallsDelete()
        {
            this.dbManager.Setup(db => db.Delete(It.IsAny<string>()));

            var testName = "testName";

            this.service.Delete(testName);

            dbManager.Verify(mock => mock.Delete(testName), Times.Once);
        }

        [TestMethod]
        public void Delete_NameIsNull_DbManagerNotCallsDelete()
        {
            this.dbManager.Setup(db => db.Delete(It.IsAny<string>()));

            string testName = null;

            this.service.Delete(testName);

            this.dbManager.Verify(mock => mock.Delete(testName), Times.Never);
        }

        [TestMethod]
        public void Delete_NameIsEmpty_DbManagerNotCallsDelete()
        {
            this.dbManager.Setup(db => db.Delete(It.IsAny<string>()));

            string testName = string.Empty;

            this.service.Delete(testName);

            this.dbManager.Verify(mock => mock.Delete(testName), Times.Never);
        }

        [TestMethod]
        public void Update_ProductName_DbManagerCallsGet()
        {
            this.dbManager.Setup(db => db.Get(It.IsAny<string>()))
                     .Returns<string>(a => new Product() { Name = a, LastUpdated = DateTime.Now.AddHours(1)});

            var testProduct = new Product();
            string testName = "testProductName";

            this.service.Update(testProduct, testName);

            this.dbManager.Verify(mock => mock.Get(testName), Times.Once);
        }

        [TestMethod]
        public void Update_ProductName_DbManagerCallsUpdate()
        {
            this.dbManager.Setup(db => db.Update(It.IsAny<Product>(), It.IsAny<string>()));
            this.dbManager.Setup(db => db.Get(It.IsAny<string>()))
                          .Returns<string>(a => new Product() { Name = a, LastUpdated = DateTime.Now.AddHours(1) });

            var testProduct = new Product();
            string testName = "testProductName";

            this.service.Update(testProduct, testName);

            this.dbManager.Verify(mock => mock.Update(testProduct, testName), Times.Once);
        }

        [TestMethod]
        public void Update_ProductWithBadDateTime_ArgumentNullExceptionThrow()
        {
            this.dbManager.Setup(db => db.Get(It.IsAny<string>()))
                          .Returns<string>(a => new Product() { Name = a, LastUpdated = DateTime.Now.AddHours(-1)});
            
            var testProduct = new Product();
            string testName = "testProductName";

            Assert.ThrowsException<ArgumentNullException>(() => this.service.Update(testProduct, testName));
        }

        [TestMethod]
        public void Update_NameIsNull_ArgumentNullExceptionThrow()
        {            
            var testProduct = new Product();
            string testName = null;

            Assert.ThrowsException<ArgumentNullException>(() => this.service.Update(testProduct, testName));
        }

        [TestMethod]
        public void Update_NameIsEmpty_ArgumentNullExceptionThrow()
        {
            var testProduct = new Product();
            string testName = string.Empty;

            Assert.ThrowsException<ArgumentNullException>(() => this.service.Update(testProduct, testName));
        }

        [TestMethod]
        public void Get_Name_DbManagerCallGet()
        {
            this.dbManager.Setup(db => db.Get(It.IsAny<string>()))
                     .Returns<string>(a => new Product() { Name = a });

            string testName = "testProductName";

            this.service.Get(testName);

            this.dbManager.Verify(mock => mock.Get(testName), Times.Once);

        }

        [TestMethod]
        public void Get_NameIsNull_ArgumentNullExceptionThrow()
        {            
            string testName = null;

            Assert.ThrowsException<ArgumentNullException>(() => this.service.Get(testName));
        }

        [TestMethod]
        public void Get_NameIsEmpty_ArgumentNullExceptionThrow()
        {
            string testName = string.Empty;

            Assert.ThrowsException<ArgumentNullException>(() => this.service.Get(testName));
        }
    }
}
