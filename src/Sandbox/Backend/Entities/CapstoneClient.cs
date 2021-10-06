﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Backend.Entities
{
    internal partial class CapstoneClient
    {
        public CapstoneClient()
        {
            TeamAssignments = new HashSet<TeamAssignment>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string Slogan { get; set; }
        [Required]
        public string ContactName { get; set; }
        public bool Confirmed { get; set; }

        [InverseProperty(nameof(TeamAssignment.Client))]
        public virtual ICollection<TeamAssignment> TeamAssignments { get; set; }
    }
}