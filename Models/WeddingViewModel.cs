using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingPlanner.Models;

namespace WeddingPlanner.Models
{
    public class WeddingViewModel
    {
        [Required]
        [MinLength(2)]
        public string Name1 {get; set;}
        [Required]
        [MinLength(2)]
        public string Name2 {get; set;}
        [Required]
        [MyDate(ErrorMessage="Date cannot be in the past")]
        public DateTime Date {get; set;}
        [Required]
        public string Address {get; set;}
        public int GuestCount {get; set;}
    
        public WeddingViewModel(){
            GuestCount = 0;
        }
    }
    public class MyDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                DateTime theirDate = Convert.ToDateTime(value);
                return theirDate >= DateTime.Now;
            }
        }  
}        