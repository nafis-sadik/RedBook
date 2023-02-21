using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory.Data.Entities
{
    public partial class Catagory
    {
        public Catagory()
        {
            InverseParentCategoryNavigation = new HashSet<Catagory>();
        }

        public int Id { get; set; }
        public string CatagoryName { get; set; }
        public int? ParentCategory { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Catagory ParentCategoryNavigation { get; set; }
        public virtual ICollection<Catagory> InverseParentCategoryNavigation { get; set; }
    }
}
