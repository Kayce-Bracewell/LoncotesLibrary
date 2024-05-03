using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models.DTOs;

public class PatronDTO()
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