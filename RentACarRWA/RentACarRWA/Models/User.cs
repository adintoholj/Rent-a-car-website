using System.ComponentModel.DataAnnotations;

namespace RentACarRWA.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^\+?\d{6,15}$", ErrorMessage = "Telefon mora sadržavati samo brojeve (i opcionalno + na početku), između 6 i 15 cifara.")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email nije u ispravnom formatu.")]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<Car> Cars { get; set; } = new List<Car>();

    }
}
