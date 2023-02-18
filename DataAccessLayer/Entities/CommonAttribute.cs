using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory.Domain.Entities
{
    public partial class CommonAttribute
    {
        public int Id { get; set; }
        public string AttributeType { get; set; }
        public string AttributeName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; } = "";
    }
}
