using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels;
using ProductCatalog.ViewModels.ProductsViewModels;

namespace ProductCatalog.Controllers
{
    public class ProductController : Controller
    {
        private readonly StoreDataContext _context;

        public ProductController(StoreDataContext context)
        {
            _context = context;
        }

        [Route("v1/products")]
        [HttpGet]
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

        [Route("v1/products/{id}")]
        [HttpGet]
        public Product Get(int id)
        {
            return _context.Products
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        [Route("v1/products")]
        [HttpPost]
        public ResultViewModel Post([FromBody]EditorProductViewModel model)
        {
            model.Validate();
            if (model.Invalid)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possível cadastrar o produto",
                    Data = model.Notifications
                };

            var dateNow = DateTime.Now;

            var product = new Product();
            product.Id = model.Id;
            product.Name = model.Name;
            product.Description = model.Description;
            product.Image = model.Image;
            product.Price = model.Price;
            product.Quantity = model.Quantity;
            product.CategoryId = model.CategoryId;
            product.CreateDate = dateNow;
            product.LastUpdateDate = dateNow;

            _context.Products.Add(product);
            _context.SaveChanges();

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto cadastrado com sucesso",
                Data = product,
            };
        }

        [Route("v1/products")]
        [HttpPut]
        public ResultViewModel Put([FromBody] EditorProductViewModel model)
        {
            model.Validate();
            if (model.Invalid)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possível editar o produto",
                    Data = model.Notifications,
                };

            var dateNow = DateTime.Now;

            var product =_context.Products.Find(model.Id);
            product.Id = model.Id;
            product.Name = model.Name;
            product.Description = model.Description;
            product.Image = model.Image;
            product.Price = model.Price;
            product.Quantity = model.Quantity;
            product.CategoryId = model.CategoryId;
            product.LastUpdateDate = dateNow;

            _context.Entry<Product>(product).State = EntityState.Modified;
            _context.SaveChanges();

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto cadastrado com sucesso",
                Data = product
            };
        }
    }
}
