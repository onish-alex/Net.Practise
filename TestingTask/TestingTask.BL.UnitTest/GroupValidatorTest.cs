using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TestingTask.BL.Validation;
using TestingTask.Core.Interfaces;
using TestingTask.Core.Models;
using TestingTask.BL.FormatRules;

using TestingTask.BL.IntegrationTest;

namespace TestingTask.BL.UnitTest
{
    [TestClass]
    public class GroupValidatorTest
    {        
        private IValidator<Group> validator;
        private Group testGroup;

        [TestInitialize]
        public void Initialize()
        {
            validator = new GroupValidator();
            testGroup = new Group();
        }

        [TestMethod]
        public void Validate_Null_False()
        {
            Assert.IsFalse(validator.Validate(null));
        }

        [TestMethod]
        public void Validate_GroupWithNullGuests_False()
        {
            Assert.IsFalse(validator.Validate(testGroup));
        }

        [TestMethod]
        public void Validate_GroupWithoutGuests_False()
        {
            testGroup.Guests = new List<Guest>();
            Assert.IsFalse(validator.Validate(testGroup));
        }

        [TestMethod]
        public void Validate_NullNamesGuests_False()
        {
            testGroup.Guests = new List<Guest>();
            testGroup.Guests.Add(new Guest() { Age = 20 });

            Assert.IsFalse(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = "TestFirstName"});

            Assert.IsFalse(validator.Validate(testGroup));
            
            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest() { Age = 20, LastName = "TestLastName"});

            Assert.IsFalse(validator.Validate(testGroup));
        }

        [TestMethod]
        public void Validate_EmptyNamesGuests_False()
        {
            testGroup.Guests = new List<Guest>();
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = string.Empty, LastName = string.Empty });
            Assert.IsFalse(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = "TestFirstName", LastName = string.Empty });
            Assert.IsFalse(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = string.Empty, LastName = "TestLastName" });
            Assert.IsFalse(validator.Validate(testGroup));
        }

        [TestMethod]
        public void Validate_NamesWithUnallowableSymbols_False()
        {
            testGroup.Guests = new List<Guest>();
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = "NormalFName", LastName = "LNameWithNumber1" });
            Assert.IsFalse(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = "NormalFName", LastName = "12345" });
            Assert.IsFalse(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = "FNameWithNumber2", LastName = "NormalLName" });
            Assert.IsFalse(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = "12345", LastName = "NormalLName" });
            Assert.IsFalse(validator.Validate(testGroup));
        }

        [TestMethod]
        public void Validate_TooShortName_False()
        {
            testGroup.Guests = new List<Guest>();
            testGroup.Guests.Add(new Guest() 
            { 
                Age = 20, 
                FirstName = TestHelper.GetRandomTestName(GuestFormatRules.AllowableSymbols, GuestFormatRules.FirstNameMinLength - 1), 
                LastName = "NormalLName" 
            });
            Assert.IsFalse(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest() 
            { 
                Age = 20, 
                FirstName = "NormalFName",
                LastName = TestHelper.GetRandomTestName(GuestFormatRules.AllowableSymbols, GuestFormatRules.LastNameMinLength - 1)
            });
            Assert.IsFalse(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest() 
            { 
                Age = 20, 
                FirstName = TestHelper.GetRandomTestName(GuestFormatRules.AllowableSymbols, GuestFormatRules.FirstNameMinLength - 1), 
                LastName = TestHelper.GetRandomTestName(GuestFormatRules.AllowableSymbols, GuestFormatRules.LastNameMinLength - 1)
            });
            Assert.IsFalse(validator.Validate(testGroup));
        }

        [TestMethod]
        public void Validate_TooLongName_False()
        {
            testGroup.Guests = new List<Guest>();
            testGroup.Guests.Add(new Guest()
            {
                Age = 20,
                FirstName = TestHelper.GetRandomTestName(GuestFormatRules.AllowableSymbols, GuestFormatRules.FirstNameMaxLength + 1),
                LastName = "NormalLName"
            });
            Assert.IsFalse(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest()
            {
                Age = 20,
                FirstName = "NormalFName",
                LastName = TestHelper.GetRandomTestName(GuestFormatRules.AllowableSymbols, GuestFormatRules.LastNameMaxLength + 1)
            });
            Assert.IsFalse(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest()
            {
                Age = 20,
                FirstName = TestHelper.GetRandomTestName(GuestFormatRules.AllowableSymbols, GuestFormatRules.FirstNameMaxLength + 1),
                LastName = TestHelper.GetRandomTestName(GuestFormatRules.AllowableSymbols, GuestFormatRules.LastNameMaxLength + 1)
            });
            Assert.IsFalse(validator.Validate(testGroup));
        }

        [TestMethod]
        public void Validate_OnlyChildren_False()
        {
            var rand = new Random();
            testGroup.Guests = new List<Guest>();
            testGroup.Guests.Add(new Guest() { Age = rand.Next(GuestFormatRules.AdultMinAge, GuestFormatRules.AdultMaxAge + 1), FirstName = "TestFirstName1", LastName = "TestLastName1" });
            testGroup.Guests.Add(new Guest() { Age = rand.Next(GuestFormatRules.AdultMinAge, GuestFormatRules.AdultMaxAge + 1), FirstName = "TestFirstName2", LastName = "TestLastName2" });

            Assert.IsFalse(validator.Validate(testGroup));
        }

        [TestMethod]
        public void Validate_NegativeAge_False()
        {
            testGroup.Guests = new List<Guest>();
            testGroup.Guests.Add(new Guest() { Age = 25, FirstName = "TestFirstName1", LastName = "TestLastName1" });
            testGroup.Guests.Add(new Guest() { Age = -3, FirstName = "TestFirstName2", LastName = "TestLastName2" });

            Assert.IsFalse(validator.Validate(testGroup));
        }

        [TestMethod]
        public void Validate_ValidGroup_True()
        {
            testGroup.Guests = new List<Guest>();
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = "TestFName", LastName = "TestLName" });
            testGroup.Guests.Add(new Guest() { Age = 5, FirstName = "TestChildFName", LastName = "TestChildFName" });
            Assert.IsTrue(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = "TestFName", LastName = "TestLName" });
            Assert.IsTrue(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = "TestFName", LastName = "TestLName" });
            testGroup.Guests.Add(new Guest() { Age = 5, FirstName = "TestChildFName", LastName = "TestChildFName" });
            testGroup.Guests.Add(new Guest() { Age = 4, FirstName = "TestChildFName", LastName = "TestChildFName" });
            Assert.IsTrue(validator.Validate(testGroup));

            testGroup.Guests.Clear();
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = "TestFName", LastName = "TestLName" });
            testGroup.Guests.Add(new Guest() { Age = 20, FirstName = "AnotherTestFName", LastName = "AnotherTestLName" });
            Assert.IsTrue(validator.Validate(testGroup));
        }
    }
}
