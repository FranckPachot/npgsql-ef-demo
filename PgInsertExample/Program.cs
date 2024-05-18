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
        }
        Console.WriteLine("1000 rows inserted into my_table.");
    }
}
