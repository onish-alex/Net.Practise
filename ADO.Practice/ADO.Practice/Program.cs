using System;

namespace ADO.Practice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DbHelper helper = new DbHelper();

            helper.InsertRoom(1, "153");
            helper.InsertSubject(1, "Test1", "TestDesc1");
            helper.InsertLesson(1, 1, 1, DateTime.Now);

            helper.InsertRoom(2, "346");
            helper.InsertSubject(2, "Test2", "TestDesc2");
            helper.InsertLesson(2, 2, 2, DateTime.Now);

            helper.UpdateRoom(2, "103");
            helper.UpdateSubject(2, "Test3", "TestDesc3");
            helper.UpdateLesson(2, 2, 1, DateTime.Now);

            helper.DeleteLesson(1);
            helper.DeleteLesson(2);
            helper.DeleteRoom(1);
            helper.DeleteRoom(2);
            helper.DeleteSubject(1);
            helper.DeleteSubject(2);

            var query1Result = helper.GetQuery1();
            Console.WriteLine("1. Вывести Ид, ФИО сотрудников, которые живут в городе London");
            Console.WriteLine("{0,10}{1,20}{2,20}", "Id", "First Name", "Last Name");
            foreach (var employee in query1Result)
            {
                Console.WriteLine("{0,10}{1,20}{2,20}", employee.Id, employee.FirstName, employee.LastName);
            }
            Console.WriteLine("----------------------------------------");

            var query2Result = helper.GetQuery2();
            Console.WriteLine("2. Найти количество покупателей, которых обслужил сотрудник, который имеет наибольшее кол-во заказов");
            Console.WriteLine(query2Result);
            Console.WriteLine("----------------------------------------");

            var query3Result = helper.GetQuery3();
            Console.WriteLine("3. Вывести страны и города, куда было доставленно более двух заказов");
            foreach (var row in query3Result)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine("----------------------------------------");

            var query4Result = helper.GetQuery4();
            Console.WriteLine("4. Найти самый дорогой товар из категории \"Морепродукти\" (Seafood)");
            Console.WriteLine(query4Result);
            Console.WriteLine("----------------------------------------");

            var query5Result = helper.GetQuery5();
            Console.WriteLine("5. Вывести Ид, ФИО покупателей, которые делали заказ в другой города");
            foreach (var row in query5Result)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine("----------------------------------------");

            var query6Result = helper.GetQuery6();
            Console.WriteLine("6. Вывести Ид, ФИО покупателей, у которых промежуток между заказами превышал полгода");
            foreach (var row in query6Result)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine("----------------------------------------");
        }
    }
}
