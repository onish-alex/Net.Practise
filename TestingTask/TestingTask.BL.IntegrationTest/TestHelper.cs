using System;
using System.Collections.Generic;
using System.Text;
using TestingTask.Core.Models;
using TestingTask.BL.FormatRules;

namespace TestingTask.BL.IntegrationTest
{
    public static class TestHelper
    {
        public static List<Guest> GenerateGuests(int adultsCount, int childrenCount)
        {
            var rand = new Random();

            var guests = new List<Guest>();

            for (int i = 0; i < adultsCount; i++)
            {
                guests.Add(new Guest()
                {
                    Age = rand.Next(GuestFormatRules.AdultMinAge, GuestFormatRules.AdultMaxAge),
                    FirstName = $"TestAdultFName",
                    LastName = $"TestAdultLName",
                });
            }

            for (int i = 0; i < childrenCount; i++)
            {
                guests.Add(new Guest()
                {
                    Age = rand.Next(GuestFormatRules.ChildMinAge, GuestFormatRules.ChildMaxAge + 1),
                    FirstName = $"TestChildFName",
                    LastName = $"TestChildLName",
                });
            }

            return guests;
        }

        public static string GetRandomTestName(string alphabet, int length)
        {
            var rand = new Random();
            var builder = new StringBuilder();
            
            for (int i = 0; i < length; i++)
            {
                builder.Append(alphabet[rand.Next(0, alphabet.Length)]);
            }

            return builder.ToString();
        }
    }
}
