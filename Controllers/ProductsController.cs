using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrashProduto.API.Models;

namespace CrashProduto.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GetProducts returns products
        /// </summary>        
        /// <returns>List of Products</returns>        
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }


        /// <summary>
        /// GetProductsWithPagination returns products for specific page.
        /// </summary>        
        /// <returns>List of Products</returns>        
        /// <param name="page">Is current Page.</param>
        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsWithPagination(string page)
        {
            string _page = page;
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// GetProduct with specific ID
        /// </summary>
        /// <returns>Return specifc product</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        /// Update specific product
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Products/id
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "description": "Teste"
        ///        "active": true
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Product ID</param>
        /// <param name="product">Product Object</param>
        /// <response code="404">IF product not exists</response>
        /// <response code="400">Format of product it's incorrect</response>  
        /// <response code="204">Product updated</response>  
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutProduct(long id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Products
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "description": "Teste"
        ///        "active": true
        ///     }
        ///
        /// </remarks>
        /// <param name="product"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>  
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        /// <summary>
        /// DeleteProduct 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns removed product</response>
        /// <response code="404">If product not exists</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
