using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApiTest1.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Product>("Products");
    builder.EntitySet<Inventory>("Inventory"); 
    builder.EntitySet<Module>("Module"); 
    builder.EntitySet<OrderDetail>("OrderDetail"); 
    builder.EntitySet<Price>("Price"); 
    builder.EntitySet<ProductFeature>("ProductFeature"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ProductsController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/Products
        [EnableQuery]
        public IQueryable<Product> GetProducts()
        {
            return db.Product;
        }

        // GET: odata/Products(5)
        [EnableQuery]
        public SingleResult<Product> GetProduct([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Product.Where(product => product.Id == key));
        }

        // PUT: odata/Products(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Product> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = await db.Product.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            patch.Put(product);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(product);
        }

        // POST: odata/Products
        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Product.Add(product);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(product);
        }

        // PATCH: odata/Products(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Product> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = await db.Product.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            patch.Patch(product);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(product);
        }

        // DELETE: odata/Products(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Product product = await db.Product.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            db.Product.Remove(product);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Products(5)/Inventory
        [EnableQuery]
        public IQueryable<Inventory> GetInventory([FromODataUri] Guid key)
        {
            return db.Product.Where(m => m.Id == key).SelectMany(m => m.Inventory);
        }

        // GET: odata/Products(5)/Module
        [EnableQuery]
        public SingleResult<Module> GetModule([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Product.Where(m => m.Id == key).Select(m => m.Module));
        }

        // GET: odata/Products(5)/OrderDetail
        [EnableQuery]
        public IQueryable<OrderDetail> GetOrderDetail([FromODataUri] Guid key)
        {
            return db.Product.Where(m => m.Id == key).SelectMany(m => m.OrderDetail);
        }

        // GET: odata/Products(5)/Price
        [EnableQuery]
        public SingleResult<Price> GetPrice([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Product.Where(m => m.Id == key).Select(m => m.Price));
        }

        // GET: odata/Products(5)/ProductFeature
        [EnableQuery]
        public SingleResult<ProductFeature> GetProductFeature([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Product.Where(m => m.Id == key).Select(m => m.ProductFeature));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(Guid key)
        {
            return db.Product.Count(e => e.Id == key) > 0;
        }
    }
}
