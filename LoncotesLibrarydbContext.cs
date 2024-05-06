using Microsoft.EntityFrameworkCore;
using LoncotesLibrary.Models;

public class LoncotesLibraryDbContext : DbContext
{

    public DbSet<Patron> Patrons { get; set; }
    public DbSet<MaterialType> MaterialTypes { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Genre> Genres { get; set; }

    public DbSet<Checkout> Checkouts {get; set;}

    public LoncotesLibraryDbContext(DbContextOptions<LoncotesLibraryDbContext> context) : base(context)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patron>().HasData(new Patron[]
        {
            new Patron {Id = 1, FirstName = "Sue", LastName = "Secherol", Address = "9123 Hollywood Blvd", Email="SueS@gmail.com", IsActive=true},
            new Patron {Id = 2, FirstName = "Joe", LastName = "Morgenstern", Address = "3245 The Place Dr", Email="JoeM@yahoo.com", IsActive=true}
        });
        modelBuilder.Entity<MaterialType>().HasData(new MaterialType[]
        {
            new MaterialType {Id = 1, Name = "Book", CheckoutDays = 30},
            new MaterialType {Id = 2, Name = "CD", CheckoutDays = 10},
            new MaterialType {Id = 3, Name = "DVD", CheckoutDays = 5}
        });
        modelBuilder.Entity<Material>().HasData(new Material[]
        {
            new Material { Id = 1, MaterialName = "Dune", MaterialTypeId = 1, GenreId = 1, OutOfCirculationSince = null },
            new Material { Id = 2, MaterialName = "The Exorcist", MaterialTypeId = 2, GenreId = 2, OutOfCirculationSince = null },
            new Material { Id = 3, MaterialName = "1984", MaterialTypeId = 1, GenreId = 1, OutOfCirculationSince = null },
            new Material { Id = 4, MaterialName = "Harry Potter and the Sorcerer's Stone", MaterialTypeId = 1, GenreId = 1, OutOfCirculationSince = null },
            new Material { Id = 5, MaterialName = "Abbey Road", MaterialTypeId = 2, GenreId = 2, OutOfCirculationSince = null },
            new Material { Id = 6, MaterialName = "The Shawshank Redemption", MaterialTypeId = 3, GenreId = 3, OutOfCirculationSince = null },
            new Material { Id = 7, MaterialName = "Pride and Prejudice", MaterialTypeId = 1, GenreId = 1, OutOfCirculationSince = null },
            new Material { Id = 8, MaterialName = "The Dark Side of the Moon", MaterialTypeId = 2, GenreId = 2, OutOfCirculationSince = null },
            new Material { Id = 9, MaterialName = "Inception", MaterialTypeId = 3, GenreId = 3, OutOfCirculationSince = null },
            new Material { Id = 10, MaterialName = "Blade Runner", MaterialTypeId = 1, GenreId = 1, OutOfCirculationSince = null },
            new Material { Id = 11, MaterialName = "Die Hard", MaterialTypeId = 1, GenreId = 4, OutOfCirculationSince = null },
            new Material { Id = 12, MaterialName = "Mission Impossible", MaterialTypeId = 2, GenreId = 4, OutOfCirculationSince = null },
            new Material { Id = 13, MaterialName = "The Matrix", MaterialTypeId = 3, GenreId = 4, OutOfCirculationSince = null },
            new Material { Id = 14, MaterialName = "Titanic", MaterialTypeId = 1, GenreId = 5, OutOfCirculationSince = null },
            new Material { Id = 15, MaterialName = "The Notebook", MaterialTypeId = 1, GenreId = 5, OutOfCirculationSince = null },
            new Material { Id = 16, MaterialName = "The Fast and the Furious", MaterialTypeId = 2, GenreId = 4, OutOfCirculationSince = null },
            new Material { Id = 17, MaterialName = "Armageddon", MaterialTypeId = 3, GenreId = 4, OutOfCirculationSince = null },
            new Material { Id = 18, MaterialName = "Casablanca", MaterialTypeId = 1, GenreId = 5, OutOfCirculationSince = null },
            new Material { Id = 19, MaterialName = "Gladiator", MaterialTypeId = 2, GenreId = 4, OutOfCirculationSince = null },
            new Material { Id = 20, MaterialName = "Romeo + Juliet", MaterialTypeId = 3, GenreId = 5, OutOfCirculationSince = null }
        });
        modelBuilder.Entity<Genre>().HasData(new Genre[]
        {
            new Genre { Id = 1, Name = "Science Fiction"},
            new Genre { Id = 2, Name = "Horror"},
            new Genre { Id = 3, Name = "Comedy"},
            new Genre { Id = 4, Name = "Action"},
            new Genre { Id = 5, Name = "Romance"}
        });
    }
}