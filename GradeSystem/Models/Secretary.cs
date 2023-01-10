using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GradeSystem.Models;

[Table("secretaries")]
public partial class Secretary
{
    [Key]
    public int Phonenumber { get; set; }

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

    [ForeignKey("Username")]
    [InverseProperty("Secretaries")]
    public virtual User UsernameNavigation { get; set; } = null!;
}
