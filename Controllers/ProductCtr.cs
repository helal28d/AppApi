using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppApi.Data;
using AppApi.DTOs;
using AppApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace AppApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductCtr : ControllerBase
{
    private readonly DataContext _context;
    public ProductCtr(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> Get()
    {
        return await _context.Products.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        if (id < 1)
            return BadRequest();
        var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
            return NotFound();
        return Ok(product);

    }

    [HttpPost]
    public async Task<ActionResult<List<Product>>> Post(ProductCreateDto product)
    {
        var newProduct = new Product
        {
            Name = product.Name,
            Price = product.Price
        };

        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();
        return Ok(await _context.Products.ToListAsync());
    }

    [HttpPut]
    public async Task<IActionResult> Put(Product productData)
    {
        if (productData == null || productData.Id == 0)
            return BadRequest();

        var product = await _context.Products.FindAsync(productData.Id);
        if (product == null)
            return NotFound();
        product.Name = productData.Name;

        product.Price = productData.Price;
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id < 1)
            return BadRequest();
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return Ok();

    }
}
