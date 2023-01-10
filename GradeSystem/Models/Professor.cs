using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GradeSystem.Models;

[Table("professors")]
public partial class Professor
{
    [Key]
    [Column("AFM")]
    public int Afm { get; set; }

    [StringLength(45)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(45)]
    [Unicode(false)]
    public string Surname { get; set; } = null!;

    [StringLength(45)]
    [Unicode(false)]
    public string Department { get; set; } = null!;

    [Column("username")]
    [StringLength(45)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [InverseProperty("AfmNavigation")]
    public virtual ICollection<Course> Courses { get; } = new List<Course>();

    [ForeignKey("Username")]
    [InverseProperty("Professors")]
    public virtual User UsernameNavigation { get; set; } = null!;
}
