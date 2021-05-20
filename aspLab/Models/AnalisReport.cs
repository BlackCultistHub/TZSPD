using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
namespace aspLab.Models
{
    [Table("analis_reps")]
    public class AnalisReport
    {
        public int id { get; set; }
        public string object_name { get; set; }
        
        public int duration_h { get; set; }
        public string result { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yy hh:mm:ss}",
            ApplyFormatInEditMode = true)]
        public DateTime dateTime { get; set; }
    }
}
