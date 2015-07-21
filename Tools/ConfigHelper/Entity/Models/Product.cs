using PetaPoco;
using System;

namespace Entity.Models
{
    [TableName("Product"), PrimaryKey("ID", autoIncrement = false)]
    public class Product
    {
        public Guid ID { get; set; }

        public string Resource { get; set; }

        public string Products { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid Building { get; set; }

        public int Time { get; set; }

        public int Workers { get; set; }
    }
}