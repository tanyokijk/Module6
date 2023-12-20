namespace Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Net.Security;
using System.Collections.Generic;

public class DataContex : DbContext
{
    public DataContex()
    {
        this.Database.EnsureDeleted();
        this.Database.EnsureCreated();
    }

    public DbSet<Game> Games => this.Set<Game>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=games.sqlite;");
    }


}