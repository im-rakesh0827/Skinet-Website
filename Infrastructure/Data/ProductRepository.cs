using System.Collections;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data;
public class ProductRepository(StoreContext context) : IProductRepository
{
    public void AddProduct(Product product)
    {
        try
        {
            context.Products.Add(product);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void DeleteProduct(Product product)
    {
        try
        {
            context.Products.Remove(product);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        try
        {
            return await context.Products.Select(x => x.Brand).Distinct().ToListAsync();
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        try
        {
            return await context.Products.Select(x => x.Type).Distinct().ToListAsync();
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        try
        {
            return await context.Products.FindAsync(id);

        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public async Task<IEnumerable> GetProductByNameAsync(string name)
    {
        try
        {
            return await context.Products.Where(x => x.Name == name).ToListAsync();
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        try
        {
            var query = context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(brand))
            {
                query = query.Where(x => x.Brand == brand);
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(x => x.Type == type);
            }
            // if (!string.IsNullOrWhiteSpace(sort))
            // {
            //     switch (sort.ToLower())
            //     {
            //         case "priceasc":
            //             query = query.OrderBy(x => x.Price);
            //             break;
            //         case "pricedesc":
            //             query = query.OrderByDescending(x => x.Price);
            //             break;
            //         case "nameasc":
            //             query = query.OrderBy(x => x.Name);
            //             break;
            //         case "namedesc":
            //             query = query.OrderByDescending(x => x.Name);
            //             break;
            //         case "brandasc":
            //             query = query.OrderBy(x => x.Brand);
            //             break;
            //         case "branddesc":
            //             query = query.OrderByDescending(x => x.Brand);
            //             break;
            //         case "typeasc":
            //             query = query.OrderBy(x => x.Type);
            //             break;
            //         case "typedesc":
            //             query = query.OrderByDescending(x => x.Type);
            //             break;
            //         default:
            //             break;
            //     }

            // }

            query = sort switch
            {
                "priceasc" => query.OrderBy(x => x.Price),
                "pricedesc" => query.OrderByDescending(x => x.Price),
                "nameasc" => query.OrderBy(x => x.Name),
                "namedesc" => query.OrderByDescending(x => x.Name),
                "brandasc" => query.OrderBy(x => x.Brand),
                "branddesc" => query.OrderByDescending(x => x.Brand),
                "typeasc" => query.OrderBy(x => x.Type),
                "typedesc" => query.OrderByDescending(x => x.Type),
                _ => query
            };

            return await query.ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool IsProductExists(int id)
    {
        try
        {
            return context.Products.Any(x => x.Id == id);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<bool> SaveChangeAsync()
    {
        try
        {
            return await context.SaveChangesAsync() > 0;
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void UpdateProduct(Product product)
    {
        try
        {
            // context.Products.Update(product);
            context.Entry(product).State = EntityState.Modified;
        }
        catch (System.Exception)
        {

            throw;
        }
    }
}