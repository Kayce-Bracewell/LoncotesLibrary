

using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models;

public class Material()
{
    int Id {get; set;}
    [Required]
    string MaterialName {get; set;}
    [Required]
    int MaterialTypeId {get; set;}
    MaterialType MaterialType {get; set;}
    [Required]
    int GenreId {get; set;}
    Genre Genre {get; set;}
    DateTime? OutOfCirculationSince {get; set;}
}