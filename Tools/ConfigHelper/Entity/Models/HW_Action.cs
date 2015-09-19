using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class HW_Action
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime UpdateTime { get; set; }

        public string Type { get; set; }

        public string Time { get; set; }

        public string Require { get; set; }

        public string Return { get; set; }

        public Guid Building { get; set; }
    }
}
