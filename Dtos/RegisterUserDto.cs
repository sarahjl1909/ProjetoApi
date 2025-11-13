using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoApi.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        public List<string> Roles { get; set; } = new List<string>();

    }
}