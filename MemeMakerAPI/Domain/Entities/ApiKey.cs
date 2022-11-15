using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class ApiKey
    {
        public int id { get; set; }
        public DateTime create_date { get; set; }
        public string api_key { get; set; } = null!;
        public DateTime expire_date { get; set; }
        public bool? active { get; set; }
    }
}
