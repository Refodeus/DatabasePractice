using System.ComponentModel.DataAnnotations.Schema;

namespace DatabasePractice.Models
{
    public class GetPurchasesOfBirthday
    {
        [Column("ID")]
        public int ID { get; set; }
        [Column("Имя")]
        public string Имя { get; set; }
        [Column("Фамилия")]
        public string Фамилия { get; set; }
        [Column("Общая сумма")]
        public decimal ОбщаяСумма {  get; set; }
    }
}
