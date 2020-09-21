using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels.ProductsViewModels;

namespace ProductCatalog.Repositories
{
    public class ProductRepository
    {
        private readonly StoreDataContext _context;

        public ProductRepository(StoreDataContext context)
        {
            _context = context;
        }

        public IEnumerable<ListProductViewModel> Get()
        {
            return _context.Products
                .Include(x => x.Category)
                .Select(x => new ListProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Category = x.Category.Title,
                    CategoryId = x.CategoryId,
                })
                .AsNoTracking()
                .ToList();
        }

        public Product Get(int id)
        {
            // return _context.Products.Find(id);
            return _context.Products
                .AsTracking()
                .SingleOrDefault(x => x.Id == id);
        }

        public void Save(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Entry<Product>(product).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Product product)
        {

        }
    }
}
