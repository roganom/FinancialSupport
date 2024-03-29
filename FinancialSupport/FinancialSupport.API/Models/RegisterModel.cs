﻿using System.ComponentModel.DataAnnotations;

namespace FinancialSupport.API.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("Password", ErrorMessage = "As senhas são diferentes")]
        public string ConfirmPassword { get; set; }
    }
}
