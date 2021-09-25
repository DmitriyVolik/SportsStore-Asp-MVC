using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models.ViewModels
{
    public class ChangePswdModel
    {
        [Required]
        [MinLength(8)]
        [MaxLength(16)]
        
        public string Password { get; set; }
        
        [MaxLength(16)]
        [MinLength(8)]
        [Required]
        [Compare(otherProperty:"Password", ErrorMessage = "Passwords not equal" )]
        public string PasswordConfirm { get; set; }
        
        [Required]
        public string OldPassword { get; set;}
        public string ReturnUrl { get; set; } = "/";
    }
}