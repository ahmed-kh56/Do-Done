using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoDone.Domain.Roles
{
    public class Role
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        override public string ToString()
            => $"{Name}";
    }
}
