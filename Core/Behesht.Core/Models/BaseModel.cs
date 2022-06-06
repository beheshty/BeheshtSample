﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Core.Models
{
    public class BaseModel<TKey>
    {
        public TKey Id { get; set; }
    }

    public class BaseModel : BaseModel<long>
    {
    }
}
