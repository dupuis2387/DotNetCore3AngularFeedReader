﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NgModusFeedReader.Data.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
