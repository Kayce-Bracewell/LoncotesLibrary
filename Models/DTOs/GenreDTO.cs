using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models.DTOs;

public class GenreDTO()
{
    int Id {get; set;}
    [Required]
    string Name {get; set;}
}