using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Business.DTOs
{
    public class UserRequestDTO
    {
        [Required(ErrorMessage = "Please enter name"), MaxLength(30)]
        public string name { get; set; }
        // We can also add phone numbers and others into this
    }
}
