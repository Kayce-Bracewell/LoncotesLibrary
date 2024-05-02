using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models.DTOs;

public class MaterialTypeDTO()
{
    int Id {get; set;}
    [Required]
    string Name {get; set;}
    [Required]
    int CheckoutDays {get; set;}
}