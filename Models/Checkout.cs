using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models;

public class Checkout()
{
    int Id {get; set;}
    [Required]
    int MaterialId {get; set;}
    Material Material {get; set;}
    [Required]
    int PatronId {get; set;}
    Patron Patron {get; set;}
    [Required]
    DateTime CheckoutDate {get; set;}

    DateTime? ReturnDate {get; set;}
}