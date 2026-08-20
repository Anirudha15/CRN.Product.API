using CRN.Product.API.Data;
using CRN.Product.API.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductEntity = CRN.Product.API.Entities.Product;
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<ProductEntity> Products, int TotalRecords)>
    GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Id);

        var totalRecords = await query.CountAsync();

        var products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalRecords);
    }

    public async Task<ProductEntity?> GetByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<ProductEntity> CreateAsync(ProductEntity product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task UpdateAsync(ProductEntity product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProductEntity product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
