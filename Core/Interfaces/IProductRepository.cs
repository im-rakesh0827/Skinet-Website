using System.Collections;
using Core.Entities;
namespace Core.Interfaces;
public interface IProductRepository
{
    public Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort);
    public Task<Product?> GetProductByIdAsync(int id);
    public Task<IEnumerable> GetProductByNameAsync(string name);
    public Task<IReadOnlyList<string>> GetBrandsAsync();
    public Task<IReadOnlyList<string>> GetTypesAsync();
    public void AddProduct(Product product);
    public void UpdateProduct(Product product);
    public void DeleteProduct(Product product);
    public bool IsProductExists(int id);
    public Task<bool> SaveChangeAsync();

}