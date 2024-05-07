using LoncotesLibrary.Models;
using LoncotesLibrary.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore.Query.Internal;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<LoncotesLibraryDbContext>(builder.Configuration["LoncotesLibraryDbConnectionString"]);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// The librarians would like to see a list of all the circulating materials.
// Include the Genre and MaterialType.
// Exclude materials that have a OutOfCirculationSince value.
app.MapGet("/api/materials", (LoncotesLibraryDbContext db, int? materialTypeId, int? genreId) =>
{
    IQueryable<MaterialDTO> materialsQuery = db.Materials
        .Include(m => m.Genre)
        .Include(m => m.MaterialType)
        .Select(m => new MaterialDTO
        {
            Id = m.Id,
            MaterialName = m.MaterialName,
            MaterialTypeId = m.MaterialTypeId,
            MaterialType = new MaterialTypeDTO
            {
                Id = m.MaterialType.Id,
                Name = m.MaterialType.Name
            },
            GenreId = m.GenreId,
            Genre = new GenreDTO
            {
                Id = m.Genre.Id,
                Name = m.Genre.Name
            }
        });

    // Apply filters based on query parameters
    if (materialTypeId != null)
    {
        materialsQuery = materialsQuery.Where(m => m.MaterialTypeId == materialTypeId);
    }
    if (genreId != null)
    {
        materialsQuery = materialsQuery.Where(m => m.GenreId == genreId);
    }

    List<MaterialDTO> materials = materialsQuery.ToList();

    return materials;
});

// The librarians would like to see details for a material. 
// Include the Genre, MaterialType, and Checkouts 
// (as well as the Patron associated with each checkout using ThenInclude).
// Do not add the Material and MaterialType to each checkout.

app.MapGet("/api/materials/{materialId}", (LoncotesLibraryDbContext db, int materialId) =>
{
     var materialDetails = db.Materials
        .Include(m => m.Genre)
        .Include(m => m.MaterialType)
        .Include(m => m.Checkout)
            .ThenInclude(co => co.Patron)
        .Where(m => m.Id == materialId)
        .Select(m => new MaterialDTO
        {
            Id = m.Id,
            MaterialName = m.MaterialName,
            MaterialType = new MaterialTypeDTO
            {
                Id = m.MaterialType.Id,
                Name = m.MaterialType.Name,
                CheckoutDays = m.MaterialType.CheckoutDays
            },
            Genre = new GenreDTO
            {
                Id = m.Genre.Id,
                Name = m.Genre.Name
            },
            Checkout = m.Checkout.Select(co => new CheckoutDTO
            {
                Id = co.Id,
                CheckoutDate = co.CheckoutDate,
                ReturnDate = co.ReturnDate,
                Patron = new PatronDTO
                {
                    Id = co.Patron.Id,
                    FirstName = co.Patron.FirstName,
                    LastName = co.Patron.LastName,
                    Address = co.Patron.Address,
                    Email = co.Patron.Email,
                    IsActive = co.Patron.IsActive
                }
            }).ToList(),
            OutOfCirculationSince = m.OutOfCirculationSince
        });
    if (materialDetails == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(materialDetails);
});

app.MapPost("/api/materials", (LoncotesLibraryDbContext db, Material material) =>
{
    db.Materials.Add(material);
    db.SaveChanges();
    return Results.Created($"/api/materials/{material.Id}", material);
});

// Endpoint is ONLY to update the OutOfCirculationSince property
app.MapPut("/api/materials/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    Material materialToUpdate = db.Materials.FirstOrDefault(material => material.Id == id);
    if (materialToUpdate == null)
    {
        return Results.NotFound();
    }
    materialToUpdate.OutOfCirculationSince = DateTime.Now;
    
    db.SaveChanges();
    return Results.NoContent();
});

// The librarians will need a form in their app that let's them choose material types.
// Create an endpoint that retrieves all of the material types to eventually populate that form field

app.MapGet("/api/materialtypes", (LoncotesLibraryDbContext db) =>
{
    return db.MaterialTypes
        .Select(mt => new MaterialTypeDTO
        {
            Id = mt.Id,
            Name = mt.Name,
            CheckoutDays = mt.CheckoutDays
        });
});

// The librarians will also need form fields that have all of the genres to choose from.
// Create an endpoint that gets all of the genres.
app.MapGet("/api/genres", (LoncotesLibraryDbContext db) =>
{
    return db.Genres
        .Select(g => new GenreDTO
        {
            Id = g.Id,
            Name = g.Name
        });
});

// The librarians want to see a list of library patrons.
app.MapGet("/api/patrons", (LoncotesLibraryDbContext db) =>
{
    return db.Patrons
        .Select(p => new PatronDTO
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Address = p.Address,
            Email = p.Email,
            IsActive = p.IsActive
        });
});

// This endpoint should get a patron and include their checkouts,
// and further include the materials and their material types
app.MapGet("/api/patrons/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    // COME BACK -- Needs Checkouts to confirm this endpoint
    var patronDetails = db.Checkouts
        .Include(c => c.Patron)
        .Include(c => c.Material)
            .ThenInclude(m => m.MaterialType)
        .Where(c => c.Patron.Id == id)
        .Select(c => new CheckoutDTO
        {
            Id = c.Id,
            MaterialId = c.MaterialId,
            Material = new MaterialDTO
            {
                Id = c.Material.Id,
                MaterialName = c.Material.MaterialName,
                MaterialTypeId = c.Material.MaterialTypeId,
                MaterialType = new MaterialTypeDTO
                {
                    Id = c.Material.MaterialType.Id,
                    Name = c.Material.MaterialType.Name,
                    CheckoutDays = c.Material.MaterialType.CheckoutDays
                },
                GenreId = c.Material.GenreId,
                Genre = new GenreDTO
                {
                    Id = c.Material.Genre.Id,
                    Name = c.Material.Genre.Name
                },
                OutOfCirculationSince = c.Material.OutOfCirculationSince
            },
            PatronId = c.PatronId,
            Patron = new PatronDTO
            {
                Id = c.Patron.Id,
                FirstName = c.Patron.FirstName,
                LastName = c.Patron.LastName,
                Address = c.Patron.Address,
                Email = c.Patron.Email,
                IsActive = c.Patron.IsActive
            },
            CheckoutDate = c.CheckoutDate,
            ReturnDate = c.ReturnDate
        })
        .ToList();

    return patronDetails;
});

// Sometimes patrons move or change their email address.
// Add an endpoint that updates these properties only.
app.MapPut("/api/patrons/{id}", (LoncotesLibraryDbContext db, Patron updatedPatron, int id) =>
{
    Patron patronToUpdate = db.Patrons.FirstOrDefault(patron => patron.Id == id);
    if (patronToUpdate == null)
    {
        return Results.NotFound();
    }
    patronToUpdate.Address = updatedPatron.Address;
    patronToUpdate.Email = updatedPatron.Email;

    db.SaveChanges();
    return Results.Ok(patronToUpdate);
});

// Sometimes patrons move out of the county.
// Allow the librarians to deactivate a patron (another soft delete example!).
app.MapPut("/api/patrons/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    // Endpoint works but might change endpoint spec bc there is already a put on patron{id} but requires a body
    Patron patronToUpdate = db.Patrons.FirstOrDefault(patron => patron.Id == id);
    if (patronToUpdate == null)
    {
        return Results.NotFound();
    }
    patronToUpdate.IsActive = false;

    db.SaveChanges();
    return Results.Ok(patronToUpdate);
});

// The librarians need to be able to checkout items for patrons.
// Add an endpoint to create a new Checkout for a material and patron.
// Automatically set the checkout date to DateTime.Today.
app.MapPost("/api/checkouts", (LoncotesLibraryDbContext db, Checkout checkout) =>
{
    checkout.CheckoutDate = DateTime.Today;
    db.Checkouts.Add(checkout);
    db.SaveChanges();
    return Results.Created($"/api/checkouts/{checkout.Id}", checkout);
});

// The librarians need an endpoint to mark a checked out item as returned by item id.
// Add an endpoint expecting a checkout id, and update the checkout with a return date of DateTime.Today.
app.MapPut("/api/checkouts/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    Checkout checkout = db.Checkouts.FirstOrDefault(c => c.Id == id);
    checkout.ReturnDate = DateTime.Today;
    return Results.Ok(checkout);
});

app.MapGet("/api/materials/available", (LoncotesLibraryDbContext db) =>
{
    return db.Materials
    .Where(m => m.OutOfCirculationSince == null)
    .Where(m => m.Checkout.All(co => co.ReturnDate != null))
    .Select(material => new MaterialDTO
    {
        Id = material.Id,
        MaterialName = material.MaterialName,
        MaterialTypeId = material.MaterialTypeId,
        GenreId = material.GenreId,
        OutOfCirculationSince = material.OutOfCirculationSince
    })
    .ToList();
});

app.MapGet("/api/checkouts/overdue", (LoncotesLibraryDbContext db) =>
{
    return db.Checkouts
    .Include(p => p.Patron)
    .Include(co => co.Material)
    .ThenInclude(m => m.MaterialType)
    .Where(co =>
        (DateTime.Today - co.CheckoutDate).Days >
        co.Material.MaterialType.CheckoutDays &&
        co.ReturnDate == null)
        .Select(co => new CheckoutDTO
        {
            Id = co.Id,
            MaterialId = co.MaterialId,
            Material = new MaterialDTO
            {
                Id = co.Material.Id,
                MaterialName = co.Material.MaterialName,
                MaterialTypeId = co.Material.MaterialTypeId,
                MaterialType = new MaterialTypeDTO
                {
                    Id = co.Material.MaterialTypeId,
                    Name = co.Material.MaterialType.Name,
                    CheckoutDays = co.Material.MaterialType.CheckoutDays
                },
                GenreId = co.Material.GenreId,
                OutOfCirculationSince = co.Material.OutOfCirculationSince
            },
            PatronId = co.PatronId,
            Patron = new PatronDTO
            {
                Id = co.Patron.Id,
                FirstName = co.Patron.FirstName,
                LastName = co.Patron.LastName,
                Address = co.Patron.Address,
                Email = co.Patron.Email,
                IsActive = co.Patron.IsActive
            },
            CheckoutDate = co.CheckoutDate,
            ReturnDate = co.ReturnDate
        })
    .ToList();
});

app.Run();