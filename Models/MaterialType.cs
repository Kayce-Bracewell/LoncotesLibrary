using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models;

public class MaterialType()
{
    int Id {get; set;}
    [Required]
    string Name {get; set;}
    [Required]
    int CheckoutDays {get; set;}
}