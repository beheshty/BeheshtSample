using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Core
{
    public interface IBaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
