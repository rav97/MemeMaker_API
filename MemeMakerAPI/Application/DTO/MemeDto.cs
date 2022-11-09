using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class MemeDto
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
