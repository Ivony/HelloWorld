using PetaPoco;
using System;

namespace Entity.Models
{
    [TableName("Item"), PrimaryKey("ID", autoIncrement = false)]
    public class Item
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}