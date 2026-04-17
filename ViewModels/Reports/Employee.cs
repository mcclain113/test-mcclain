using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("DimEmployee")]
public class Employee
{
    [Key]
    [Column("EmployeeKey")]
    public int EmployeeKey { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string DepartmentName { get; set; }

    public string Title { get; set; }

    public string Status { get; set; }
}