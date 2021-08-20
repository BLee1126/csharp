using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace belt1.Models
{ 
    public class Rsvp {
    [Key]
    public int RsvpId {get;set;}
    public int UserId {get;set;}
    public User User {get;set;}
    public int StuffId {get;set;}
    public Stuff Stuff {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    }

}