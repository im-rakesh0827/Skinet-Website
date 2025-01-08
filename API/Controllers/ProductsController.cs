using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.ProductsController;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository productRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        try
        {
            return Ok(await productRepository.GetProductsAsync(brand, type, sort));
        }
        catch (System.Exception)
        {
            throw;
        }
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        try
        {
            var product = await productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        try
        {
            productRepository.AddProduct(product);
            if (await productRepository.SaveChangeAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            return BadRequest("Problem occured while createting this product");
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        try
        {
            if (id != product.Id || !IsProductExists(id))
            {
                return BadRequest("Cannot modified the product details");
            }
            productRepository.UpdateProduct(product);
            await productRepository.SaveChangeAsync();

            if (await productRepository.SaveChangeAsync())
            {
                return NoContent();
            }
            return BadRequest("Cannot modified the product details");
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var product = await productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound("Product does not exist");
            }
            productRepository.DeleteProduct(product);
            if (await productRepository.SaveChangeAsync())
            {
                return Content("Product deleted successfully");
                // return NoContent();
            }
            return BadRequest("Cannot delete the product");
        }
        catch (Exception)
        {

            throw;
        }
    }



    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PatchProduct(int id, Product product)
    {
        try
        {
            if (id != product.Id || !productRepository.IsProductExists(id))
            {
                return BadRequest("Cannot modified the product details");
            }
            productRepository.UpdateProduct(product);
            if (await productRepository.SaveChangeAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            return BadRequest("Cannot modified the product details");
        }
        catch (System.Exception)
        {
            throw;
        }
    }


    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        try
        {
            return Ok(await productRepository.GetBrandsAsync());
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        try
        {
            return Ok(await productRepository.GetTypesAsync());
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private bool IsProductExists(int id)
    {
        try
        {
            return productRepository.IsProductExists(id);
        }
        catch (Exception)
        {
            throw;
        }
    }

}