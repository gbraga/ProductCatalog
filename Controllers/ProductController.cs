using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.Repositories;
using ProductCatalog.ViewModels;
using ProductCatalog.ViewModels.ProductsViewModels;

namespace ProductCatalog.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _repository;

        public ProductController(ProductRepository repository)
        {
            _repository = repository;
        }

        [Route("v1/products")]
        [HttpGet]
        public IEnumerable<ListProductViewModel> Get()
        {
            return _repository.Get();
        }

        [Route("v1/products/{id}")]
        [HttpGet]
        public Product Get(int id)
        {
            return  _repository.Get(id);
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

            _repository.Save(product);

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

            var product =_repository.Get(model.Id);
            product.Id = model.Id;
            product.Name = model.Name;
            product.Price = model.Price;
            product.Description ??= model.Description;
            product.Image ??= model.Image;
            product.Quantity = model.Quantity > 0 ? model.Quantity : product.Quantity;
            product.CategoryId = model.CategoryId  > 0 ? model.CategoryId : product.CategoryId;
            product.LastUpdateDate = dateNow;

            _repository.Update(product);

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto cadastrado com sucesso",
                Data = product
            };
        }
    }
}
