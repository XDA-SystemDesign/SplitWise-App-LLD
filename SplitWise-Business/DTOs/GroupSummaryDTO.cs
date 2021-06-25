using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Business.DTOs
{
    public class GroupSummaryDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int GroupId { get; set; }
    }
}
