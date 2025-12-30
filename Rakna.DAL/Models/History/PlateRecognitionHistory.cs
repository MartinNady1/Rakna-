using Rakna.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rakna.Common.Enum;

namespace Rakna.DAL.Models.History
{
    public class PlateRecognitionHistory
    {
        [Key]
        public int ModuleConfidenceId { get; set; }
        public double LettersConfidence { get; set; }
        public double objectConfidence { get; set; }

        [ForeignKey("History")]
        public int HistoryId { get; set; }
        public virtual History History { get; set; }
    }
}
