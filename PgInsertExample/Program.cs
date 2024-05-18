using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
class Program
{
    static void Main()
    {
        using (var context = new MyDbContext())
        {

            // Ensure database is created
            context.Database.EnsureCreated();

            // Reset pg_stat_statements
            context.Database.ExecuteSqlRaw("SELECT pg_stat_statements_reset();");

            // Insert 1000 rows
            var entries = Enumerable.Range(1, 1000).Select(i => new MyTable { value = $"Value {i}" }).ToList();
            context.MyTable.AddRange(entries);
            context.SaveChanges();
            Console.WriteLine("1000 rows inserted into my_table.");  
            

            // Execute native SQL and display the rows
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT calls, rows, query FROM pg_stat_statements ORDER BY calls";
                context.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        Console.WriteLine($"{result.GetInt64(0),10} calls, {result.GetInt64(1)} rows, Query: {result.GetString(2)}");
                    }
                }
                context.Database.CloseConnection();
            }

            
        }

    }
}
