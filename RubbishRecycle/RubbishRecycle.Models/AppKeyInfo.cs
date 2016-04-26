using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    [Table("app_key")]
    public class AppKeyInfo
    {
        [Key]
        [Column("app_key")]
        public String AppKey { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }
    }
}
