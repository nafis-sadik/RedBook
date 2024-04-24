namespace Inventory.Data.Models
{
    public class CommonAttributeModel
    {
        public int AttributeId { get; set; }

        public string AttributeType { get; set; }

        public string AttributeName { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateBy { get; set; }
    }
}
