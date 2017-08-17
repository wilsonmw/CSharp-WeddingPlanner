using System.Collections.Generic;
using WeddingPlanner.Models;

namespace WeddingPlanner.Models
{
    public class Guest
    {
        public int GuestID {get; set;}
        public int WeddingID {get; set;}
        public Wedding Wedding {get; set;}
        public int UserID {get; set;}
        public User User {get; set;}
        
        
    }
}