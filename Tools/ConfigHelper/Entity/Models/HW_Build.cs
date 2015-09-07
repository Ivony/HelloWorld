using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    [TableName("hw_build"),PrimaryKey("ID",autoIncrement=false)]
    public class HW_Build
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
