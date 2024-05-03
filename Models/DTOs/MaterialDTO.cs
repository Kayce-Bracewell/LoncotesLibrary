using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models.DTOs;

public class MaterialDTO()
{
    int Id {get; set;}
    [Required]
    string MaterialName {get; set;}
    [Required]
    int MaterialTypeId {get; set;}
    MaterialTypeDTO MaterialType {get; set;}
    [Required]
    int GenreId {get; set;}
    GenreDTO Genre {get; set;}
    DateTime? OutOfCirculationSince {get; set;}
}