using Rakna.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.HistoryDto
{
    public class ComplaintsHistoryDto
    {
        public ComplaintType ComplaintType { get; set; }// نوع الريبورت
        public string ComplaintMessage { get; set; }//  الريبورت
        public DateTime ComplaintTime { get; set; } // وقت الريبورت ما اتعمل
        public DateTime SolvedTime { get; set; }// وقت حل الريبورت
    }
}