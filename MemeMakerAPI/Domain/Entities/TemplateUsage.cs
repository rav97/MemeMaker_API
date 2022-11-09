using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class TemplateUsage
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public int template_id { get; set; }

        public virtual Template template { get; set; } = null!;
    }
}
