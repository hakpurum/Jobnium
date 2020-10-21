using System;
using System.ComponentModel.DataAnnotations;

namespace Jobnium.Model
{
    public class JobTable
    {
        [Key]
        public int Id { get; set; }
        public string JobName { get; set; }
        public int RunSecond { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}