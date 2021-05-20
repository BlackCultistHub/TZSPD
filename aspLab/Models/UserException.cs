using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspLab.Models
{
    [Table ("error_reps")]
    public class UserException
    {
        public int id { get; set; }
        public string project_name { get; set; }
        public string error_text { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yy hh:mm:ss}",
            ApplyFormatInEditMode = true)]
        public DateTime dateTime { get; set; }
    }
}
