using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListaTarefa.Models
{
    public class Tarefa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Titulo { get; set; }
        [Required]
        [StringLength(250)]
        public string? Descricao { get; set; }
        [Required] 
        public DateTime Data { get; set; }
        [Required]
        public EnumStatusTarefa Status { get; set; }
    }
}