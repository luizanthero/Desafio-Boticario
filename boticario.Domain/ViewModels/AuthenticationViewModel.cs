using System.ComponentModel.DataAnnotations;

namespace boticario.ViewModels
{
    public class AuthenticationViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
