using SplitWise_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Business.DTOs
{
    public class ExpenseRequestDTO
    {
        [Required]
        public int GroupId { get; set; }
        [Required]
        public SplitWise_Constants.Enums.SplitType SplitType { get; set; }
        public List<UserExpenditure> userExpenditures { get; set; }
        public List<UserContribution> userContributions { get; set; }
    }
}
