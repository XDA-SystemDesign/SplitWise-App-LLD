using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Business.DTOs
{
    public class GroupDTO
    {
        [Required(ErrorMessage = "Please enter name"), MaxLength(30)]
        public string GroupName { get; set; }
        public List<UserRequestDTO> Users { get; set; }
    }
}
