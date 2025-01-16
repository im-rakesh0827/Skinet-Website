using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.ProductsController;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        try
        {
            var spec = new ProducctSpecification(brand, type, sort);
            var products = await repository.ListAsync(spec);
            return Ok(products);
        }
        catch (Exception)
        {
            throw;
        }
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        try
        {
            var product = await repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        try
        {
            repository.Add(product);
            if (await repository.SaveAllAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            return BadRequest("Problem occured while createting this product");
        }
        catch (Exception)
        {

            throw;
        }
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
    {
        try
        {
            if (id != product.Id)
            {
                return BadRequest($"Cannot modified the product details, Product ID({id}) in the URL does not match the product ID({product.Id}) in the body.");
            }
            if (!repository.Exists(id))
            {
                return NotFound($"Product does not exist with ID: {id}");
            }
            repository.Update(product);
            if (await repository.SaveAllAsync())
            {
                return Content("Product updated successfully");
            }
            return BadRequest("Cannot modified the product details");
        }
        // catch (DbUpdateConcurrencyException ex)
        // {
        //     Console.WriteLine($"Concurrency error: {ex.Message}");
        //     return Conflict("A concurrency issue occurred while updating the product.");
        // }
        // catch (DbUpdateException ex)
        // {
        //     Console.WriteLine($"Database error: {ex.Message}");
        //     return StatusCode(500, "Database error occurred. Please try again.");
        // }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var product = await repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product does not exist with ID : {id}");
            }
            repository.Remove(product);
            if (await repository.SaveAllAsync())
            {
                return Content("Product deleted successfully");
            }
            return BadRequest("Cannot delete the product");
        }
        catch (Exception)
        {

            throw;
        }
    }



    #region Patch Method
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PatchProduct(int id, Product product)
    {
        try
        {
            if (id != product.Id)
            {
                return BadRequest($"Product ID({id}) in the URL does not match the product ID({product.Id}) in the body.");
            }
            if (!repository.Exists(id))
            {
                return NotFound($"Product with ID({id}) was not found.");
            }
            repository.Update(product);
            if (await repository.SaveAllAsync())
            {
                return Content("Product details updated successfully");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the product.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
        }
    }
    #endregion

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        try
        {
            var spec = new BrandListSpecification();
            return Ok(await repository.ListAsync(spec));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        try
        {
            var spec = new TypeListSpecification();
            return Ok(await repository.ListAsync(spec));
        }
        catch (Exception)
        {
            throw;
        }
    }

    private bool IsProductExists(int id)
    {
        try
        {
            return repository.Exists(id);
        }
        catch (Exception)
        {
            throw;
        }
    }

}