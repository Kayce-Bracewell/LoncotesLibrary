using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models;

public class Patron()
{
    int Id {get; set;}
    [Required]
    string FirstName {get; set;}
    [Required]
    string LastName {get; set;}
    [Required]
    string Address {get; set;}
    [Required]
    string Email {get; set;}
    [Required]
    bool IsActive {get; set;}
}