using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Models.AccountViewModels
{
    public class FundTransferVM
    {
        [Required]
        public double Value { get; set; }

        [Required]
        public string AccountDesNumber { get; set; }

        public string AccountDesName { get; set; }

        public int AccountDestinationId { get; set; }

        [Required]
        public int AccountId { get; set; }
    }
}
