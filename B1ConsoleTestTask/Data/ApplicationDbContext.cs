using B1ConsoleTestTask.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace B1ConsoleTestTask.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Entity> Entities { get; set; }

    // Настройка подключения к базе данных с использованием Microsoft SQL Server
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Настройка подключения к базе данных с использованием Microsoft SQL Server
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=B1ConsoleTest;Trusted_Connection=True;MultipleActiveResultSets=true");
    }
}