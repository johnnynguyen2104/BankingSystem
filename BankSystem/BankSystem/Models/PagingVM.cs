using BankSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class PagingVM<TKey, TEntity>
    {
        public TKey Id { get; set; }

        public Pager Pager { get; set; }

        public IList<TEntity> Items { get; set; }
    }
}
