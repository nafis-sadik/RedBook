using System.ComponentModel.DataAnnotations;

namespace RedBook.Core.EntityFramework
{
    public class BaseEntity<IPrimaryKey>
    {
        [Key]
        public IPrimaryKey Id { get; set; }
    }
}
