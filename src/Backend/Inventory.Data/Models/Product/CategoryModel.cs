﻿namespace Inventory.Data.Models.Product
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        public string CatagoryName { get; set; }

        public int? ParentCategoryId { get; set; }

        public int OrganizationId { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
