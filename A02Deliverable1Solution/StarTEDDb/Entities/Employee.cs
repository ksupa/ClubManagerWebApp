﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StarTEDDb.Entities;

[Index("PositionID", Name = "IX_PositionID")]
[Index("ProgramID", Name = "IX_ProgramID")]
public partial class Employee
{
    [Key]
    public int EmployeeID { get; set; }

    [Required]
    [StringLength(20)]
    [Unicode(false)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(20)]
    [Unicode(false)]
    public string LastName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateHired { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReleaseDate { get; set; }

    public int PositionID { get; set; }

    public int ProgramID { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string LoginID { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();

	[NotMapped]
	public string FullName { get { return LastName + ", " + FirstName; } }
}