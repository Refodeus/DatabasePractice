using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace DatabasePractice.Models;

public partial class Order
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Precision(12, 2)]
    public decimal Сумма { get; set; }

    [Column("Дата и время")]
    public DateTime ДатаИВремя { get; set; }
    public string Статус { get; set; } = null!;
    [Column("Клиент")]
    public int Клиент { get; set; }

    [ForeignKey(nameof(Клиент))]
    public virtual Client? КлиентNavigation { get; set; }
}
