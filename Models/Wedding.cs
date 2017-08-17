using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingPlanner.Models;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        public int WeddingID {get; set;}
        [Required]
        [MinLength(2)]
        public string Name1 {get; set;}
        [Required]
        [MinLength(2)]
        public string Name2 {get; set;}
        [Required]
        [MyDate]
        public DateTime Date {get; set;}
        [Required]
        public string Address {get; set;}
        public int GuestCount {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public List<Guest> Guests {get; set;}
        public int UserID {get; set;}
        public User User {get; set;}


        public Wedding(){
            GuestCount = 0;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}