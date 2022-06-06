using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Core
{
    public class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        public TKey Id { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool IsDelete { get; set; }
    }

    public class BaseEntity : BaseEntity<long>
    {
        public long CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public long ModifyUserId { get; set; }
        public DateTime? ModifyDate { get; set; }
        public long DeleteUserId { get; set; }
       
    }

   

    //public class BaseEntity : BaseEntity<long>
    //{

    //}
}
