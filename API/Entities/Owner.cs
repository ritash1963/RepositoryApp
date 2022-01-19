using System;
using System.Collections;
using System.Collections.Generic;

namespace API.Entities
{
    public class Owner
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public ICollection<Item> Items { get; set; }
        
    }
}