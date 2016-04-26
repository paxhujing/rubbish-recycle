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
    public class AppKey
    {
        [Key]
        [Column("key")]
        public String Key { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
