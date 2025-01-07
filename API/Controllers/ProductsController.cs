using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.ProductsController;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{

    private readonly StoreContext _context;
    public ProductsController(StoreContext context)
    {
        this._context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        // return Ok();
        return await _context.Products.ToListAsync();
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        // return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        return product;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<IEnumerable<Product>>> UpdateProduct(int id, Product product)
    {
        try
        {
            if (id != product.Id || !IsProductExists(id))
            {
                return BadRequest("Cannot modified the product details");
            }
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            // return NoContent();
            // return await _context.Products.ToListAsync();

            var updatedProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            return updatedProduct == null ? NoContent() : Ok(updatedProduct);

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
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();

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
            if (id != product.Id || !IsProductExists(id))
            {
                return BadRequest("Cannot modified the product details");
            }
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var updatedProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            // return NoContent();
            // return Ok(updatedProduct);
            return updatedProduct == null ? NoContent() : Ok(updatedProduct);
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
            return _context.Products.Any(e => e.Id == id);
        }
        catch (Exception)
        {
            throw;
        }
    }

}