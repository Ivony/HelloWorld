using PetaPoco;
using System;

namespace Entity.Models
{
    [TableName("Construction"),PrimaryKey("ID",autoIncrement=false)]
    public
        class Construction
    {
        public Guid ID { get; set; }

        public Guid OriginBuilding { get; set; }

        public Guid Building { get; set; }

        public string Items { get; set; }

        public int TIme { get; set; }

        public string Description { get; set; }

        public int Workers { get; set; }
    }
}