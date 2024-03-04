using System.ComponentModel.DataAnnotations;

namespace TPI_ProgramacionIII.Models
{
    public class AdminPostDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
        //public string UserType { get; set; }
    }
}
