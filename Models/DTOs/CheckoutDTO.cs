using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models.DTOs;

public class CheckoutDTO()
{
    int Id {get; set;}
    [Required]
    int MaterialId {get; set;}
    MaterialDTO Material {get; set;}
    [Required]
    int PatronId {get; set;}
    PatronDTO Patron {get; set;}
    [Required]
    DateTime CheckoutDate {get; set;}

    DateTime? ReturnDate {get; set;}
}