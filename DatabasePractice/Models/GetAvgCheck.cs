using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace DatabasePractice.Models
{
    public class GetAvgCheck
    {
        [Column("Час")]
        public decimal Час { get; set; }
        [Column("Средний Чек")]
        public decimal СреднийЧек {  get; set; }
        [Column("Количество")]
        public long Количество {  get; set; }
    }
}
