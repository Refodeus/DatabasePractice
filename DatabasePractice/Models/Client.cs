using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatabasePractice.Models;

[Table("Clients")]
public partial class Client
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public string Имя { get; set; } = null!;

    public string Фамилия { get; set; } = null!;

    [Column("Дата рождения")]
    public DateOnly ДатаРождения { get; set; }
}
