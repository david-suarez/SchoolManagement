using System;
using System.Collections.Generic;

using SchoolManagement.Interfaces;
using SchoolManagement.Utilities;

namespace SchoolManagement.Models
{
    public class Student : IEntity, IEquatable<Student>
    {
        public string Type { get; set; }
        
        public string Name { get; set; }

        public string Gender { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool Equals(Student other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            var objects = new List<object>()
            {
                this.Type,
                this.Name,
                this.LastUpdate
            };

            return this.CalculateHashCode(objects.ToArray());
        }
    }
}
