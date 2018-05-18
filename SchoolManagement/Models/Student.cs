using System;

using SchoolManagement.Interfaces;

namespace SchoolManagement.Models
{
    public class Student : IEntity
    {
        public string Type { get; set; }
        
        public string Name { get; set; }

        public string Gender { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
