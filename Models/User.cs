
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace belt1.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required]
        [MinLength(2)]
        [Display(Name = "Name")]
        public string Name {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage ="must have 8 characters for the password!")]
        [RegularExpression(@"^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Invalid Password. Min 8 Characteres. Requires 1 lowerase letter, 1 number, and 1 special character.")]
        public string Password {get;set;}
        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string Confirm {get;set;}
        public List<Rsvp> Rsvp {get;set;}
        public List<Stuff> Stuff {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }

    // public class PasswordValid : ValidationAttribute
    // {
    //     protected override ValidationResult IsValid(string value, ValidationContext validationContext)
    //     {
    //         if(value is string)
    //         {
    //             var input = value;
    //             ErrorMessage = string.Empty;

    //             if (string.IsNullOrWhiteSpace(input))
    //             {
    //                 return new ValidationResult("Password should not be empty");
    //             }

    //             var hasNumber = new Regex(@"[0-9]+");
    //             var hasUpperChar = new Regex(@"[A-Z]+");
    //             var hasMiniMaxChars = new Regex(@".{8,15}");
    //             var hasLowerChar = new Regex(@"[a-z]+");
    //             var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

    //             if (!hasLowerChar.IsMatch(input))
    //             {
    //                 ErrorMessage = "Password should contain At least one lower case letter";
    //                 return new ValidationResult(ErrorMessage);
    //             }
    //             else if (!hasUpperChar.IsMatch(input))
    //             {
    //                 ErrorMessage = "Password should contain At least one upper case letter";
    //                 return new ValidationResult(ErrorMessage);
    //             }
    //             else if (!hasMiniMaxChars.IsMatch(input))
    //             {
    //                 ErrorMessage = "Password should not be less than or greater than 12 characters";
    //                 return new ValidationResult(ErrorMessage);
    //             }
    //             else if (!hasNumber.IsMatch(input))
    //             {
    //                 ErrorMessage = "Password should contain At least one numeric value";
    //                 return new ValidationResult(ErrorMessage);
    //             }

    //             else if (!hasSymbols.IsMatch(input))
    //             {
    //                 ErrorMessage = "Password should contain At least one numeric value";
    //                 return new ValidationResult(ErrorMessage);
    //             }
    //             return ValidationResult.Success;
    //         }
    //         else
    //         {
    //             return new ValidationResult("Not a valid password");
    //         }
    //     }
    // }
}
// ^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$
