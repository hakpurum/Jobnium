using System;
using System.ComponentModel.DataAnnotations;
using Jobnium.Abstract;

namespace Jobnium.Model
{
    public class JobLog
    {
        [Key]
        public int Id { get; set; }
        public string JobName { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public long ElapsedMilliseconds { get; set; }
        public string Message { get; set; } = JobLogStatus.Pending.ToString();
        public int Status { get; set; }
        
    }
}