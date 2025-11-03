using Linq_Aggregation_and_Quering;
using System.Collections.Specialized;

namespace Program
{
    public class Program 
    {
        public static void Main(string[] args)
        {
            List<Person> people = new List<Person>()
            {
                new Person{Id = 1,Name = "Alice",Age = 30,Email = "alice@example.com" },
                new Person{Id = 2,Name = "Bob",Age = 25,Email = null },
                new Person{Id = 3,Name = "Charlie",Age = 35,Email = "charlie@example.com" },
                new Person{Id = 4,Name = "Diana",Age = 40,Email = "diana@example.com" },
                new Person{Id = 5,Name = "Mohit",Age = 20,Email = "mohit@example.com" },
            };

            List<Order> orders = new List<Order>()
            {
                new Order{OrderId = 1, PersonId = 1, Amount = 100, OderDate = new DateTime(2025,01,15)},
                new Order{OrderId = 2, PersonId = 2, Amount = 50, OderDate = new DateTime(2025,02,20)},
                new Order{OrderId = 3, PersonId = 1, Amount = 75, OderDate = new DateTime(2025,03,05)},
                new Order{OrderId = 4, PersonId = 3, Amount = 120, OderDate = new DateTime(2025,03,15)},
                new Order{OrderId = 5, PersonId = 4, Amount = 200, OderDate = new DateTime(2025,04,10)},
            };

            //1) Aggregate Operations
            Console.WriteLine("Task - 1 : Total number of orders and sum of order amounts for each person");

            var JoinTable = people.Join(
                orders,
                p => p.Id,
                o => o.PersonId,
                (p, o) => new
                {
                    Id = p.Id,
                    Name = p.Name,
                    Age = p.Age,
                    Amount = o.Amount,
                    Date = o.OderDate,
                });
            var Task1 = JoinTable.GroupBy(j => j.Id).Select(g => new
            {
                Id = g.Key,
                Count = g.Count(),
                TotalSum = g.Sum(p=>p.Amount),
            });

            foreach (var item in Task1)
            {
                Console.WriteLine($"Id: {item.Id}\tCount: {item.Count}\tSum: {item.TotalSum}");
            }

            Console.WriteLine();

            Console.WriteLine("Task - 2 : Average order amount for people older than 30");
            var Task2 = JoinTable.Where(j=>j.Age>30).Average(j=>j.Amount);
            Console.WriteLine("Average of Order Amount of people greater than age 30 : " + Task2 );
            Console.WriteLine();

            Console.WriteLine("Task - 3 : Min, Max, Avg order amount for each person");
            var Task3 = JoinTable.GroupBy(j => j.Id).Select(g => new
            {
                Id = g.Key,
                Maximum = g.Max(k => k.Amount),
                Minimum = g.Min(k => k.Amount),
                Average = g.Average(k => k.Amount),
            });

            foreach (var item in Task3)
            {

                Console.WriteLine($"Id : {item.Id}\tMaximum Order : {item.Maximum}\t Minimum Order : {item.Minimum}\t Average Order: {item.Average}");
            }

            Console.WriteLine();

            Console.WriteLine("Task - 4 : Find order placed on specific date (e.g. March 5, 2025)");
            //2) Element Operators
            DateTime date = new DateTime(2025, 02, 20);
            var Task4 = JoinTable.SingleOrDefault(j=>j.Date == date);
            Console.WriteLine("Order on 20-02-2025");
            if(Task4 != null)
            Console.WriteLine($"Id : {Task4.Id}\t Amount : {Task4.Amount}");
            Console.WriteLine();

            Console.WriteLine("Task - 5 Find first order with amount > 150");
            var Task5 = orders.SingleOrDefault(o => o.Amount > 150);
            if (Task5 != null)
                Console.WriteLine($"Order Id : {Task5.OrderId}\tPerson Id : {Task5.PersonId}\tAmount : {Task5.Amount}\tDate : {Task5.OderDate}");
            else
            {
                Console.WriteLine("None of order has amount > $ 150");
            }
            Console.WriteLine();

            //3) Quantifer Operator
            Console.WriteLine("Task - 6 : Check if all people have placed at least one order");
            bool Task6 = people.All(p => orders.Any(o=>o.PersonId == p.Id));
            Console.WriteLine(Task6 == true);
            Console.WriteLine();

            Console.WriteLine("Task - 7 : Check if any order > 250");
            var Task7 = orders.Any(j => j.Amount > 250);

            if(Task7 != null)
            {
                Console.WriteLine("Yes there are orders > $250");
            }
            else
            {
                Console.WriteLine("Yes there are NO orders > $250");
            }
            Console.WriteLine();

            Console.WriteLine("Task - 8 : Convert to Dictionary<string, List<Order>>");
            //4) Collection Conversion
            var Task8 = people.Join(
                orders,
                p => p.Id,
                o => o.PersonId,
                (p, o) => new { Name = p.Name, Order = o })
                .GroupBy(x => x.Name)
                .ToDictionary( 
                
                    g => g.Key,
                    g => g.Select(x => x.Order).ToList());

            foreach (var entry in Task8)
            {
                Console.WriteLine($"Person: {entry.Key}");
                foreach (var order in entry.Value)
                {
                    Console.WriteLine($"\tOrder ID: {order.OrderId}, Amount: {order.Amount}, Date: {order.OderDate}");
                }
            }
            Console.WriteLine();

            Console.WriteLine("Task - 9 : Number of Orders per Person");
            var Task9 = people.Join(
                orders,
                p => p.Id,
                o => o.PersonId,
                (o, p) => new { Name = o.Name, p.OrderId})
                .GroupBy(p=>p.Name)
                .Select(g=> new
                {
                    Name = g.Key,
                    Count = g.Count(),
                });

            
            foreach(var entry in Task9)
            {
                Console.WriteLine($"Name : {entry.Name}\tCount : {entry.Count}");
            }
        }
    }

}