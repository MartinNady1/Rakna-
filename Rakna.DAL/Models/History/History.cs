using Rakna.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.DAL.Models.History
{
    public class History
    {
        [Key]
        public int HistoryId { get; set; }

        public HistroyType HistoryType { get; set; }
        public DateTime Timestamp { get; set; }
    }
}