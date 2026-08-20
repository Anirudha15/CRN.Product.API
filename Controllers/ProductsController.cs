using CRN.Product.API.DTOs;
using CRN.Product.API.Services;
using Microsoft.AspNetCore.Mvc;
using ProductEntity = CRN.Product.API.Entities.Product;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace CRN.Product.API.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetAll(
    int pageNumber = 1,
    int pageSize = 10)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            if (pageSize > 100)
                pageSize = 100;

            var result = await _productService.GetPagedAsync(
                pageNumber,
                pageSize);

            var response = new PagedResult<ProductEntity>
            {
                Items = result.Products,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = result.TotalRecords
            };

            return Ok(response);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return Ok(product);
        }

        // POST: api/Products
        [Authorize(Roles = "Admin")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            var product = new ProductEntity
            {
                ProductName = dto.ProductName,
                CreatedBy = dto.CreatedBy
            };

            var createdProduct =
                await _productService.CreateAsync(product);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdProduct.Id },
                createdProduct
            );
        }

        // PUT: api/Products/5
        [Authorize(Roles = "Admin")]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            ProductDto dto)
        {
            var product = new ProductEntity
            {
                ProductName = dto.ProductName,
                CreatedBy = dto.CreatedBy
            };

            var updated =
                await _productService.UpdateAsync(id, product);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return NoContent();
        }

        // DELETE: api/Products/5
        [Authorize(Roles = "Admin")]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _productService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return NoContent();
        }
    }
}