using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    //public record Student(string ID, string FirstName, string LastName);
    public class Student
    {
        [Required]
        [StringLength(16, MinimumLength = 8)]
        public string ID { get; init; } // init means that it can only be set via constructor or the initializer list
        [Required]
        public string FirstName { get; init; }
        [Required]
        public string LastName { get; init; }

        // Greedy constructor
        public Student(string ID, string FirstName, string LastName)
        {
            this.ID = ID;
            this.FirstName = FirstName;
            this.LastName = LastName;
        }

        // Parameterless constructor
        public Student()
        { }
    }
}
