using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Core.Models
{
    public class FileModel
    {
        public string MimeType { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public byte[] File { get; set; }
    }
}
