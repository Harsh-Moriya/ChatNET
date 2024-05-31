using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ChatNet.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [Remote(action:"IsUserIDValid", controller:"RemoteValidation", ErrorMessage = "User ID is already in use!")]
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Remote(action: "IsUserEmailValid", controller: "RemoteValidation", ErrorMessage = "Email is already in use!")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; }
    }
}
