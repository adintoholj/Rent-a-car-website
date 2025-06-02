using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RentACarRWA.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ModelName { get; set; }

        public string? Description { get; set; }

        public decimal PricePerDay { get; set; }

        public bool IsReserved { get; set; } = false;

        // Veza sa korisnikom (nullable ako nije rezervisan)
        public int? UserId { get; set; }

        // [ForeignKey("UserId")]
        [JsonIgnore]
        public User? User { get; set; }
    }
}
