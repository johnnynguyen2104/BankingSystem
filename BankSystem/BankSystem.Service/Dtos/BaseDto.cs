using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Service.Dtos
{
    public class BaseDto<TKey> where TKey : struct
    {
        public TKey Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
