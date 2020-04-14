using System;
using System.ComponentModel.DataAnnotations;

namespace NgModusFeedReader.Data.ViewModels
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
