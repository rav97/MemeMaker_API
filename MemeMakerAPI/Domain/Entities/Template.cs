using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Template
    {
        public Template()
        {
            GeneratedMeme = new HashSet<GeneratedMeme>();
            TemplateUsage = new HashSet<TemplateUsage>();
        }

        public int id { get; set; }
        public DateTime create_date { get; set; }
        public string name { get; set; } = null!;
        public string path { get; set; } = null!;

        public virtual ICollection<GeneratedMeme> GeneratedMeme { get; set; }
        public virtual ICollection<TemplateUsage> TemplateUsage { get; set; }
    }
}
