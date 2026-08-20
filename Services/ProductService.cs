using CRN.Product.API.Repositories;
using ProductEntity = CRN.Product.API.Entities.Product;

namespace CRN.Product.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<(IEnumerable<ProductEntity> Products, int TotalRecords)>
        GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _repository.GetPagedAsync(
                pageNumber,
                pageSize);
        }

        public async Task<ProductEntity?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ProductEntity> CreateAsync(ProductEntity product)
        {
            product.CreatedOn = DateTime.UtcNow;

            return await _repository.CreateAsync(product);
        }

        public async Task<bool> UpdateAsync(
            int id,
            ProductEntity product)
        {
            var existingProduct =
                await _repository.GetByIdAsync(id);

            if (existingProduct == null)
                return false;

            existingProduct.ProductName =
                product.ProductName;

            existingProduct.ModifiedBy =
                product.CreatedBy;

            existingProduct.ModifiedOn =
                DateTime.UtcNow;

            await _repository.UpdateAsync(existingProduct);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product =
                await _repository.GetByIdAsync(id);

            if (product == null)
                return false;

            await _repository.DeleteAsync(product);

            return true;
        }
    }
}