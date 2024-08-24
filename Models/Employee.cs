using System.ComponentModel.DataAnnotations.Schema;

namespace sm202101Eje1.Models
{
    public class Employee : BaseEntity
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dui { get; set; }
        public string NumeroTelefonico { get; set; }

        [ForeignKey("TypeEmployee")]
        public int TypeEmployeeId { get; set; }
        public TypeEmployee? TypeEmployee { get; set; }
    }
}
