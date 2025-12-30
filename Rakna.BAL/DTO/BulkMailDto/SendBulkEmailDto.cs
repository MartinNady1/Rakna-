using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.BulkMailDto
{
    public class SendBulkEmailDto
    {
        public List<string> Emails { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
    }
}