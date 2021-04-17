using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tic_tac_toe.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "No username specified")]
        public string Username { get; set; }

        [Required(ErrorMessage = "No password specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
