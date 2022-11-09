using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class GeneratedMeme
    {
        public int id { get; set; }
        public DateTime create_date { get; set; }
        public int template_id { get; set; }
        public string path { get; set; } = null!;

        public virtual Template template { get; set; } = null!;
    }
}
