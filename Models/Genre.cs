using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models;

public class Genre()
{
    int Id {get; set;}
    [Required]
    string Name {get; set;}
}