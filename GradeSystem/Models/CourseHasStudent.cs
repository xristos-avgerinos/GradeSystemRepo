using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GradeSystem.Models;

[PrimaryKey("IdCourse", "RegistrationNumber")]
[Table("course_has_students")]
public partial class CourseHasStudent
{
    [Key]
    [Column("idCOURSE")]
    public int IdCourse { get; set; }

    [Key]
    public int RegistrationNumber { get; set; }

    public int GradeCourseStudent { get; set; }

    [ForeignKey("IdCourse")]
    [InverseProperty("CourseHasStudents")]
    public virtual Course IdCourseNavigation { get; set; } = null!;

    [ForeignKey("RegistrationNumber")]
    [InverseProperty("CourseHasStudents")]
    public virtual Student RegistrationNumberNavigation { get; set; } = null!;
}
