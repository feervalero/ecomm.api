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
    builder.EntitySet<Inventory>("Inventories");
    builder.EntitySet<Product>("Product"); 
    builder.EntitySet<StatusType>("StatusType"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class InventoriesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/Inventories
        [EnableQuery]
        public IQueryable<Inventory> GetInventories()
        {
            return db.Inventory;
        }

        // GET: odata/Inventories(5)
        [EnableQuery]
        public SingleResult<Inventory> GetInventory([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Inventory.Where(inventory => inventory.Id == key));
        }

        // PUT: odata/Inventories(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Inventory> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Inventory inventory = await db.Inventory.FindAsync(key);
            if (inventory == null)
            {
                return NotFound();
            }

            patch.Put(inventory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(inventory);
        }

        // POST: odata/Inventories
        public async Task<IHttpActionResult> Post(Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Inventory.Add(inventory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InventoryExists(inventory.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(inventory);
        }

        // PATCH: odata/Inventories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Inventory> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Inventory inventory = await db.Inventory.FindAsync(key);
            if (inventory == null)
            {
                return NotFound();
            }

            patch.Patch(inventory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(inventory);
        }

        // DELETE: odata/Inventories(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Inventory inventory = await db.Inventory.FindAsync(key);
            if (inventory == null)
            {
                return NotFound();
            }

            db.Inventory.Remove(inventory);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Inventories(5)/Product
        [EnableQuery]
        public SingleResult<Product> GetProduct([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Inventory.Where(m => m.Id == key).Select(m => m.Product));
        }

        // GET: odata/Inventories(5)/StatusType
        [EnableQuery]
        public SingleResult<StatusType> GetStatusType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Inventory.Where(m => m.Id == key).Select(m => m.StatusType));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InventoryExists(Guid key)
        {
            return db.Inventory.Count(e => e.Id == key) > 0;
        }
    }
}
