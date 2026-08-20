using ProductEntity = CRN.Product.API.Entities.Product;

namespace CRN.Product.API.Repositories
{
    public interface IProductRepository
    {
        Task<(IEnumerable<ProductEntity> Products, int TotalRecords)>
            GetPagedAsync(int pageNumber, int pageSize);

        Task<ProductEntity?> GetByIdAsync(int id);

        Task<ProductEntity> CreateAsync(ProductEntity product);

        Task UpdateAsync(ProductEntity product);

        Task DeleteAsync(ProductEntity product);
    }
}