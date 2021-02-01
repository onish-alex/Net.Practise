using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TestingTask.Core.Interfaces;
using TestingTask.BL.Services;
using TestingTask.BL.Validation;
using TestingTask.Core.Models;
using Moq;
using TestingTask.BL.FormatRules;

namespace TestingTask.BL.IntegrationTest
{
    [TestClass]
    public class BookingServiceTest
    {
        private Mock<IHotelRepository> hotelRepository = new Mock<IHotelRepository>();
        private List<Hotel> testHotelList = new List<Hotel>();

        private Hotel alwaysPassOneRoomHotel = new Hotel()
        {
            Name = "AlwaysPassOneRoom",
            AllowPets = true,
            Rooms = new List<Room>()
                {
                    new Room()
                    {
                        Capacity = int.MaxValue
                    }
                }
        };

        [TestInitialize]
        public void Initialize()
        {
            this.hotelRepository.Setup(x => x.GetAllHotels()).Returns(() => testHotelList);
        }

        [TestCleanup]
        public void Cleanup()
        {
            testHotelList.Clear();
        }

        [TestMethod]
        public void GetSuitableHotelNames_Null_ArgumentException()
        {
            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);
            Assert.ThrowsException<ArgumentNullException>(() => booking.GetSuitableHotelNames(null));
        }

        [TestMethod]
        public void GetSuitableHotelNames_GroupNullGuests_ArgumentException()
        {
            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);
            var group = new Group();
            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));
        }

        [TestMethod]
        public void GetSuitableHotelNames_NullNamesGuests_ArgumentException()
        {
            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = new List<Guest>()
                {
                    new Guest()
                    {
                        Age = 25
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));
            
            group.Guests[0].FirstName = "TestFName";
            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));

            group.Guests[0].FirstName = null;
            group.Guests[0].LastName = "TestLName";
            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));
        }

        [TestMethod]
        public void GetSuitableHotelNames_EmptyNamesGuests_ArgumentException()
        {
            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = new List<Guest>()
                {
                    new Guest()
                    {
                        Age = 25,
                        FirstName = string.Empty,
                        LastName = string.Empty
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));

            group.Guests[0].FirstName = "TestFName";
            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));

            group.Guests[0].FirstName = string.Empty;
            group.Guests[0].LastName = "TestLName";
            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));
        }

        [TestMethod]
        public void GetSuitableHotelNames_NegativeAgeGuests_ArgumentException()
        {
            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(3, 0)
            };

            group.Guests[0].Age = -15;

            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));
        }

        [TestMethod]
        public void GetSuitableHotelNames_GuestsNamesWithUnallowableSymbols_ArgumentException()
        {
            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = new List<Guest>()
                {
                    new Guest()
                    {
                        FirstName = "NormalFName",
                        LastName = "LNameWithSymbol1"
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));

            group.Guests[0].FirstName = "FNameWithSymbol1";
            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));

            group.Guests[0].LastName = "NormalLName";
            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));
        }

        [TestMethod]
        public void GetSuitableHotelNames_GroupWithoutGuests_ArgumentException()
        {
            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group();
            group.Guests = new List<Guest>();

            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));
        }

        [TestMethod]
        public void GetSuitableHotelNames_GroupWithPets_HotelsWithPetsAllowed()
        {
            var withPetsHotel = new Hotel()
            {
                AllowPets = true,
                Name = "WithPetsHotel"
            };

            testHotelList.Add(withPetsHotel);

            testHotelList.Add(new Hotel()
            {
                AllowPets = false,
                Name = "WithoutPetsHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(1, 0),
                HasPets = true
            };

            var expectedList = new List<Hotel>()
            {
                withPetsHotel
            };

            var actualList = booking.GetSuitableHotelNames(group);

            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void GetSuitableHotelNames_GroupWithoutPets_HotelsWithAndWithoutPetsAllowed()
        {
            testHotelList.Add(new Hotel()
            {
                AllowPets = false,
                Name = "WithoutsPetsHotel"
            });

            testHotelList.Add(new Hotel()
            {
                AllowPets = true,
                Name = "WithPetsHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(1, 0),
                HasPets = false
            };

            var expectedList = new List<Hotel>(testHotelList);

            var actualList = booking.GetSuitableHotelNames(group);

            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void GetSuitableHotelNames_OnlyChildrenGroup_ArgumentException()
        {
            var validator = new GroupValidator();
            var validatiorMock = new Mock<IValidator<Group>>();
            validatiorMock.Setup(x => x.Validate(It.IsAny<Group>()))
                          .Returns<Group>(x => validator.Validate(x));

            var booking = new BookingService(validatiorMock.Object, hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(0, 3)
            };

            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));

            validatiorMock.Verify(x => x.Validate(It.IsAny<Group>()), Times.Once);
        }

        [TestMethod]
        public void GetSuitableHotelNames_TooShortGuestName_ArgumentException()
        {
            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = new List<Guest>()
                {
                    new Guest()
                    {
                        Age = 25,
                        FirstName = TestHelper.GetRandomTestName(GuestFormatRules.AllowableSymbols, GuestFormatRules.FirstNameMinLength - 1),
                        LastName = "TestLName"
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));

            group.Guests[0].LastName = TestHelper.GetRandomTestName(GuestFormatRules.AllowableSymbols, GuestFormatRules.LastNameMinLength - 1);
            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));

            group.Guests[0].FirstName = "TestFName";
            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));
        }

        [TestMethod]
        public void GetSuitableHotelNames_TooLongGuestName_ArgumentException()
        {
            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = new List<Guest>()
                {
                    new Guest()
                    {
                        Age = 25,
                        FirstName = TestHelper.GetRandomTestName(GuestFormatRules.AllowableSymbols, GuestFormatRules.FirstNameMaxLength + 1),
                        LastName = "TestLName"
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));

            group.Guests[0].LastName = TestHelper.GetRandomTestName(GuestFormatRules.AllowableSymbols, GuestFormatRules.LastNameMaxLength + 1);
            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));

            group.Guests[0].FirstName = "TestFName";
            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));
        }

        [TestMethod]
        public void GetSuitableHotelNames_OneRoomNotEnoughSpace_NotAddedToResult()
        {
            testHotelList.Add(alwaysPassOneRoomHotel);
            testHotelList.Add(new Hotel()
            {
                AllowPets = true,
                Name = "TestName",
                Rooms = new List<Room>()
                {
                    new Room()
                    {
                        Capacity = 3
                    }
                }
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(3, 0)
            };

            var expectedResult = new List<Hotel>() { alwaysPassOneRoomHotel };
            var actualResult = booking.GetSuitableHotelNames(group);

            CollectionAssert.AreEqual(expectedResult, actualResult);

            group.Guests = TestHelper.GenerateGuests(2, 2);

            actualResult = booking.GetSuitableHotelNames(group);

            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetSuitableHotelNames_OneRoomEnoughSpace_AddedToResult()
        {
            testHotelList.Add(new Hotel()
            {
                AllowPets = true,
                Name = "TestName",
                Rooms = new List<Room>()
                {
                    new Room()
                    {
                        Capacity = 4
                    }
                }
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(3, 0)
            };

            var expectedResult = new List<Hotel>(testHotelList);
            var actualResult = booking.GetSuitableHotelNames(group);

            //3 adults
            CollectionAssert.AreEqual(expectedResult, actualResult);

            group.Guests = TestHelper.GenerateGuests(3, 1);
            actualResult = booking.GetSuitableHotelNames(group);

            //3 adult, 1 child
            CollectionAssert.AreEqual(expectedResult, actualResult);

            group.Guests = TestHelper.GenerateGuests(1, 5);
            actualResult = booking.GetSuitableHotelNames(group);

            //1 adult, 5 children
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetSuitableHotelNames_OneRoomEnoughSpaceAlreadyBooked_NotAddedToResult()
        {
            testHotelList.Add(alwaysPassOneRoomHotel);
            testHotelList.Add(new Hotel()
            {
                AllowPets = true,
                Name = "TestName",
                Rooms = new List<Room>()
                {
                    new Room()
                    {
                        Capacity = 4,
                        BookedBy = new Group()
                    }
                }
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(3, 0)
            };

            var expectedResult = new List<Hotel>() { alwaysPassOneRoomHotel };
            var actualResult = booking.GetSuitableHotelNames(group);

            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetSuitableHotelNames_TwoRoomsEnoughSpace_AddedToResult()
        {
            var twoRoomsHotel = new Hotel()
            {
                Name = "TwoRoomsHotel",
                Rooms = new List<Room>()
                {
                    new Room()
                    {
                        Capacity = 2
                    },
                    new Room()
                    {
                        Capacity = 2
                    }
                }
            };

            testHotelList.Add(twoRoomsHotel);

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(1, 0)
            };

            var expectedResult = new List<Hotel>() { twoRoomsHotel };
            var actualResult = booking.GetSuitableHotelNames(group);

            //1 adult
            CollectionAssert.AreEqual(expectedResult, actualResult);

            group.Guests = TestHelper.GenerateGuests(2, 0);
            actualResult = booking.GetSuitableHotelNames(group);

            //2 adult 
            CollectionAssert.AreEqual(expectedResult, actualResult);

            group.Guests = TestHelper.GenerateGuests(1, 1);
            actualResult = booking.GetSuitableHotelNames(group);

            //1 adult, 1 child 
            CollectionAssert.AreEqual(expectedResult, actualResult);

            group.Guests = TestHelper.GenerateGuests(2, 1);
            actualResult = booking.GetSuitableHotelNames(group);

            //2 adult, 1 child 
            CollectionAssert.AreEqual(expectedResult, actualResult);

            group.Guests = TestHelper.GenerateGuests(2, 2);
            actualResult = booking.GetSuitableHotelNames(group);

            //2 adult, 2 child 
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetSuitableHotelNames_OneRoomOnePlace_NotAddedToResult()
        {
            testHotelList.Add(alwaysPassOneRoomHotel);
            testHotelList.Add(new Hotel()
            {
                AllowPets = true,
                Name = "TestName",
                Rooms = new List<Room>()
                {
                    new Room()
                    {
                        Capacity = 1
                    }
                }
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(1, 0)
            };

            var expectedResult = new List<Hotel>() { alwaysPassOneRoomHotel };
            var actualResult = booking.GetSuitableHotelNames(group);

            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetSuitableHotelNames_AnySuitableHotels_ArgumentException()
        {
            testHotelList.AddRange(new Hotel[]
            {
                new Hotel()
                {
                    Rooms = new List<Room>()
                    {
                        new Room()
                        {
                            Capacity = 2
                        }
                    }
                }
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(3, 0)
            };

            Assert.ThrowsException<ArgumentException>(() => booking.GetSuitableHotelNames(group));
        }

        [TestMethod]
        public void Book_NullHotelName_ArgumentException()
        {
            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            Assert.ThrowsException<ArgumentException>(() => booking.Book(null, new Group()));
        }

        [TestMethod]
        public void Book_NullGroup_ArgumentException()
        {
            testHotelList.Add(new Hotel()
            {
                Name = "TestHotel"
            });
            
            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", null));
        }

        [TestMethod]
        public void Book_GroupNullGuests_ArgumentException()
        {
            testHotelList.Add(new Hotel()
            {
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);
            var group = new Group();

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));
        }

        [TestMethod]
        public void Book_GroupWithoutGuests_ArgumentException()
        {
            testHotelList.Add(new Hotel()
            {
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group
            {
                Guests = new List<Guest>()
            };

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));
        }

        [TestMethod]
        public void Book_NullNamesGuests_ArgumentException()
        {
            testHotelList.Add(new Hotel()
            {
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = new List<Guest>()
                {
                    new Guest()
                    {
                        Age = 25
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));

            group.Guests[0].FirstName = "TestFName";
            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));

            group.Guests[0].FirstName = null;
            group.Guests[0].LastName = "TestLName";
            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));
        }

        [TestMethod]
        public void Book_EmptyNameGuests_ArgumentException()
        {
            testHotelList.Add(new Hotel()
            {
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = new List<Guest>()
                {
                    new Guest()
                    {
                        Age = 25,
                        FirstName = string.Empty,
                        LastName = string.Empty
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));

            group.Guests[0].FirstName = "TestFName";
            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));

            group.Guests[0].FirstName = string.Empty;
            group.Guests[0].LastName = "TestLName";
            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));
        }

        [TestMethod]
        public void Book_OnlyChildrenGroup_ArgumentException()
        {
            testHotelList.Add(new Hotel()
            {
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(0, 3)
            };

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));
        }

        [TestMethod]
        public void Book_NegativeAgeGuests_ArgumentException()
        {
            testHotelList.Add(new Hotel()
            {
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(3, 0)
            };

            group.Guests[0].Age = -15;

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));
        }

        [TestMethod]
        public void Book_GuestsNamesWithUnallowableSymbols_ArgumentException()
        {
            testHotelList.Add(new Hotel()
            {
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = new List<Guest>()
                {
                    new Guest()
                    {
                        FirstName = "NormalFName",
                        LastName = "LNameWithSymbol1"
                    }
                }
            };

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));

            group.Guests[0].FirstName = "FNameWithSymbol1";
            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));

            group.Guests[0].LastName = "NormalLName";
            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));
        }

        [TestMethod]
        public void Book_NotExistedHotelName_ArgumentException()
        {
            testHotelList.AddRange(new Hotel[]
            {
                new Hotel()
                {
                    Name = "TestHotel1"
                },
                new Hotel()
                {
                    Name = "TestHotel2"
                }
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel3", new Group()));
        }

        [TestMethod]
        public void Book_HotelAlreadyBookedByThisGroup_ArgumentException()
        {
            var group = new Group();

            testHotelList.Add(new Hotel()
            {
                Name = "testName",
                Rooms = new List<Room>()
                {
                    new Room()
                    {
                        BookedBy = group
                    },
                    new Room(),
                    new Room(),
                }
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            Assert.ThrowsException<ArgumentException>(() => booking.Book("testName", group));
        }

        [TestMethod]
        public void Book_ValidGroupWithPets_HotelWithoutPets_ArgumentException()
        {
            testHotelList.Add(new Hotel()
            {
                AllowPets = false,
                Rooms = new List<Room>()
                {
                    new Room()
                    {
                        Capacity = 4
                    }
                },
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(3, 0),
                HasPets = true
            };

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));
        }

        [TestMethod]
        public void Book_ValidGroup_OneRoomNotEnoughSpace_ArgumentException()
        {
            testHotelList.Add(new Hotel()
            {
                AllowPets = false,
                Rooms = new List<Room>()
                {
                    new Room()
                    {
                        Capacity = 3
                    },
                },
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(3, 0)
            };

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));

            group = new Group()
            {
                Guests = TestHelper.GenerateGuests(2, 2)
            };

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));


            group = new Group()
            {
                Guests = TestHelper.GenerateGuests(1, 4)
            };

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));
        }

        [TestMethod]
        public void Book_ValidGroup_TwoRoomsNotEnoughSpace_ArgumentException()
        {
            testHotelList.Add(new Hotel()
            {
                AllowPets = false,
                Rooms = new List<Room>()
                {
                    new Room()
                    {
                        Capacity = 2
                    },
                    new Room()
                    {
                        Capacity = 2
                    }
                },
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(2, 3)
            };

            Assert.ThrowsException<ArgumentException>(() => booking.Book("TestHotel", group));
        }

        [TestMethod]
        public void Book_ValidGroup_OneRoomHotel_OneRoom()
        {
            var testRoom = new Room()
            {
                Capacity = 4
            };

            testHotelList.Add(new Hotel()
            {
                Rooms = new List<Room>()
                {
                    testRoom
                },
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(3, 0),
            };

            var rooms = booking.Book("TestHotel", group);

            Assert.AreEqual(testRoom.BookedBy, group);
            CollectionAssert.AreEqual(rooms, new Room[] { testRoom });

            testRoom.BookedBy = null;

            group.Guests = TestHelper.GenerateGuests(3, 1);

            rooms = booking.Book("TestHotel", group);

            Assert.AreEqual(testRoom.BookedBy, group);
            CollectionAssert.AreEqual(rooms, new Room[] { testRoom });

            testRoom.BookedBy = null;

            group.Guests = TestHelper.GenerateGuests(1, 5);

            rooms = booking.Book("TestHotel", group);

            Assert.AreEqual(testRoom.BookedBy, group);
            CollectionAssert.AreEqual(rooms, new Room[] { testRoom });
        }

        [TestMethod]
        public void Book_ValidGroup_TwoRoomsHotel_OneRoom()
        {
            var firstRoom = new Room()
            {
                Capacity = 3
            };

            var secondRoom = new Room()
            {
                Capacity = 2
            };

            testHotelList.Add(new Hotel()
            {
                Rooms = new List<Room>()
                {
                    firstRoom, secondRoom
                },
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(2, 1)
            };

            var rooms = booking.Book("TestHotel", group);

            Assert.AreEqual(firstRoom.BookedBy, group);
            Assert.AreEqual(secondRoom.BookedBy, null);
            CollectionAssert.AreEqual(rooms, new Room[] { firstRoom });

            firstRoom.BookedBy = null;

            group.Guests = TestHelper.GenerateGuests(1, 1);
            rooms = booking.Book("TestHotel", group);

            Assert.AreEqual(firstRoom.BookedBy, null);
            Assert.AreEqual(secondRoom.BookedBy, group);
            CollectionAssert.AreEqual(rooms, new Room[] { secondRoom });

            secondRoom.BookedBy = null;

            group.Guests = TestHelper.GenerateGuests(1, 3);
            rooms = booking.Book("TestHotel", group);

            Assert.AreEqual(firstRoom.BookedBy, group);
            Assert.AreEqual(secondRoom.BookedBy, null);
            CollectionAssert.AreEqual(rooms, new Room[] { firstRoom });
        }

        [TestMethod]
        public void Book_ValidGroup_TwoRoomsHotel_TwoRoom()
        {
            var firstRoom = new Room()
            {
                Capacity = 3
            };

            var secondRoom = new Room()
            {
                Capacity = 2
            };

            testHotelList.Add(new Hotel()
            {
                Rooms = new List<Room>()
                {
                    firstRoom, secondRoom
                },
                Name = "TestHotel"
            });

            var booking = new BookingService(new GroupValidator(), hotelRepository.Object);

            var group = new Group()
            {
                Guests = TestHelper.GenerateGuests(3, 1)
            };

            var rooms = booking.Book("TestHotel", group);

            Assert.AreEqual(firstRoom.BookedBy, group);
            Assert.AreEqual(secondRoom.BookedBy, group);
            CollectionAssert.AreEqual(rooms, testHotelList[0].Rooms);

            firstRoom.BookedBy = null;
            secondRoom.BookedBy = null;

            group.Guests = TestHelper.GenerateGuests(2, 2);
            rooms = booking.Book("TestHotel", group);

            Assert.AreEqual(firstRoom.BookedBy, group);
            Assert.AreEqual(secondRoom.BookedBy, group);
            CollectionAssert.AreEqual(rooms, testHotelList[0].Rooms);
        }
    }
}
