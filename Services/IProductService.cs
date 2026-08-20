using ProductEntity = CRN.Product.API.Entities.Product;

namespace CRN.Product.API.Services
{
    public interface IProductService
    {
        Task<(IEnumerable<ProductEntity> Products, int TotalRecords)>
            GetPagedAsync(int pageNumber, int pageSize);

        Task<ProductEntity?> GetByIdAsync(int id);

        Task<ProductEntity> CreateAsync(ProductEntity product);

        Task<bool> UpdateAsync(int id, ProductEntity product);

        Task<bool> DeleteAsync(int id);
    }
}