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

            for (int i = 0; i < 10; i++)
            {
            // Insert 1000 rows
            var entries = Enumerable.Range(1, 1000).Select(i => new MyTable { value = $"Value {i}" }).ToList();
            context.MyTable.AddRange(entries);
            context.SaveChanges();
            //context.BulkSaveChanges();
            Console.WriteLine("1000 rows inserted into my_table.");  
            }


            // Execute native SQL and display the rows
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @"SELECT calls, rows, total_time, query FROM pg_stat_statements WHERE calls>@CallsThreshold ORDER BY calls, total_time";
                context.Database.OpenConnection();
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@CallsThreshold";
                parameter.Value = 0;
                command.Parameters.Add(parameter);
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        Console.WriteLine($"{result.GetInt64(0),10} calls, {result.GetInt64(1),10} rows, {Math.Round(result.GetDouble(2),2),10} ms, Query: {result.GetString(3)}");
                    }
                }
                context.Database.CloseConnection();
            }

            
        }

    }
}
