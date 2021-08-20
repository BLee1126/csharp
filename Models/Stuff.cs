using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace belt1.Models
{
    public class Stuff
    {
        [Key]
        public int StuffId {get;set;}
        [Required]
        public string Title {get;set;}
        [Required]
        public int Duration {get;set;}
        [Required]
        public string DurationType {get;set;}
        [Required]
        [MustBeFuture]
        public DateTime Date {get;set;}
        [Required]
        public string Description {get;set;}
        public List<Rsvp> Rsvp {get;set;}
        public int CreatorId {get;set;}
        public User Creator {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
    public class MustBeFuture : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is DateTime)
            {
                DateTime checkMe;
                checkMe = (DateTime)value;
                if(checkMe < DateTime.Now)
                {
                    return new ValidationResult("Date must be in the future!");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return new ValidationResult("Not a date");
            }
        }
    }
}