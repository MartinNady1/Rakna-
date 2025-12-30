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
    public class ComplaintHistory
    {
        [Key]
        public int ComplaintHistoryId { get; set; }//البرايمري كي

        public string ComplainantId { get; set; } // اللي عامل الريبورت
        public string ComplainantType { get; set; }// نوع اليوزر اللي عامل الريبورت
        public ComplaintType ComplaintType { get; set; }// نوع الريبورت
        public string ComplaintMessage { get; set; }//  الريبورت
        public DateTime ComplaintTime { get; set; } // وقت الريبورت ما اتعمل
        public DateTime SolvedTime { get; set; }// وقت حل الريبورت
        public string? ComplaintReceiver { get; set; }// مين حل الريبورت

        [ForeignKey("History")]
        public int HistoryId { get; set; }

        public virtual History History { get; set; }
    }
}