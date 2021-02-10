namespace Dapper.Practice.UserAnonimizer
{
    using System;
    using Dapper.Practice.DAL.Entity;
    using Dapper.Practice.DAL.Repository;

    public class Program
    {
        public static void Main(string[] args)
        {
            string input = string.Empty;

            while (input != "exit")
            {
                Console.Write(">");
                input = Console.ReadLine();

                switch (input)
                {
                    case "filldb":
                        FillDB();
                        break;

                    case "clean":
                        RemoveFromDbByEmail();
                        break;
                }
            }
        }

        public static void FillDB()
        {
            var users = new DapperRepository<User>("Users");
            var mails = new DapperRepository<Mail>("Mails");

            users.CreateAsync(new User()
            {
                Name = "Vasiliy",
                Surname = "Petrov",
                DateOfBirth = DateTime.Now,
                Email = "vasiliy.petrov@gmail.com",
            });

            users.CreateAsync(new User()
            {
                Name = "Sergey",
                Surname = "Semenov",
                Email = "sergey.semenov@gmail.com",
                DateOfBirth = DateTime.Now,
            });

            users.CreateAsync(new User()
            {
                Name = "Ivan",
                Surname = "Vasiliev",
                Email = "ivan.vasiliev@mail.ru",
                DateOfBirth = DateTime.Now,
            });

            mails.CreateAsync(new Mail()
            {
                Object = MailContentConverter.ToJSON(new MailContent()
                {
                    To = "sergey.semenov@gmail.com",
                    From = "ivan.vasiliev@mail.ru",
                    Date = new DateTime(2020, 5, 10),
                    Subject = "Greeting",
                    Letter = "Hi! How are you?",
                }),
            });

            mails.CreateAsync(new Mail()
            {
                Object = MailContentConverter.ToJSON(new MailContent()
                {
                    To = "ivan.vasiliev@mail.ru",
                    From = "sergey.semenov@gmail.com",
                    Date = new DateTime(2020, 5, 10),
                    Subject = "None",
                    Letter = "Fine, Ivan, and you?",
                }),
            });

            mails.CreateAsync(new Mail()
            {
                Object = MailContentConverter.ToJSON(new MailContent()
                {
                    To = "sergey.semenov@gmail.com",
                    From = "ivan.vasiliev@mail.ru",
                    Date = new DateTime(2020, 5, 10),
                    Subject = "None",
                    Letter = "Good, as usually",
                }),
            });

            mails.CreateAsync(new Mail()
            {
                Object = MailContentConverter.ToJSON(new MailContent()
                {
                    To = "vasiliy.petrov@gmail.com",
                    From = "ivan.vasiliev@mail.ru",
                    Date = new DateTime(2020, 5, 10),
                    Subject = "Greetings",
                    Letter = "Hi! I'm Ivan Vasiliev. Nice to meet you!",
                }),
            });

            mails.CreateAsync(new Mail()
            {
                Object = MailContentConverter.ToJSON(new MailContent()
                {
                    To = "sergey.semenov@gmail.com",
                    From = "vasiliy.petrov@gmail.com",
                    Date = new DateTime(2020, 5, 10),
                    Subject = "Ivan Vasiliev",
                    Letter = "Hi! I've got a mail recently, from ivan.vasiliev@mail.ru. Do you know him?",
                }),
            });
        }

        public static void RemoveFromDbByEmail()
        {
            Console.Write("Enter user email:");
            var email = Console.ReadLine();
            Console.Write("Enter alias:");
            var alias = Console.ReadLine();
            Console.WriteLine("Run anonimizer...");
            var cleaner = new PersonalDataCleaner(email, alias);
            cleaner.RunAsync();
        }
    }
}
