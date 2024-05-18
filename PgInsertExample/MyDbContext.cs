using Microsoft.EntityFrameworkCore;
public class MyDbContext : DbContext
{
    public DbSet<MyTable> MyTable { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Username=yugabyte;Password=yugabyte;Database=yugabyte;Pooling=true;Minimum Pool Size=1;Maximum Pool Size=20;No Reset On Close=false;Multiplexing=false;Max Auto Prepare=10;ServerCompatibilityMode=");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MyTable>().ToTable("my_table");
    }
}
public class MyTable
{
    public int id { get; set; }
    public string value { get; set; } = string.Empty;
}

