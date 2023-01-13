using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GradeSystem.Models;

[Table("course")]
public partial class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("idCOURSE")]
    public int IdCourse { get; set; }

    [StringLength(60)]
    [Unicode(false)]
    public string? CourseTitle { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string? CourseSemester { get; set; }

    [Column("AFM")]
    public int? Afm { get; set; }

    [ForeignKey("Afm")]
    [InverseProperty("Courses")]
    public virtual Professor? Professor { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<CourseHasStudent> CourseHasStudents { get; } = new List<CourseHasStudent>();
}
