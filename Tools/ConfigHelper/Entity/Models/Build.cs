using PetaPoco;
using System;

namespace Entity.Models
{
    [TableName("Build"), PrimaryKey("ID", autoIncrement = false)]
    public class Build
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}