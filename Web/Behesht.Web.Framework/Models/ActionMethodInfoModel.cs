using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Web.Framework.Models
{
    public class ActionMethodInfoModel
    {
        public string FullName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ActionName { get; set; }
        public string Verb { get; set; }
        public bool IsAllowAnonymous { get; set; }
    }
}
