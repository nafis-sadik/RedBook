namespace Inventory.Data.Models
{
    public class CommonAttributeModel
    {
        public int AttributeId { get; set; }

        public string AttributeType { get; set; }

        public string AttributeName { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UpdateBy { get; set; }
    }
}
