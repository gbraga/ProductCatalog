using System.Collections.Generic;

namespace ProductCatalog.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }
    }
}
