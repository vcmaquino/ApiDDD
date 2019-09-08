using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos
{
    public class LoginDto
    {
        //validações
        [Required(ErrorMessage = "Email é um campo obrigatorio para login.")]
        [EmailAddress(ErrorMessage = "Email em formato invalido.")]
        [StringLength(100, ErrorMessage = "Email deve ter no maximo {1} caracteres.")]
        public string Email { get; set; }
    }
}
