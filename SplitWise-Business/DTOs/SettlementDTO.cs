using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Business.DTOs
{
    public class SettlementDTO
    {
        [Required]
        public int GroupID_under_which_settlement_is_done { get; set; }
        [Required]
        public int FromUserID { get; set; }
        [Required]
        public int ToUserID { get; set; }
        [Required]
        public double Amount_Settled { get; set; }
    }
}
