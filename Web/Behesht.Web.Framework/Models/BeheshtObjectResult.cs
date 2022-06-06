using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Web.Framework.Models
{
    public class BeheshtObjectResult : ObjectResult
    {
        public BeheshtObjectResult(object value) : base(value)
        {
        }

    }
}
