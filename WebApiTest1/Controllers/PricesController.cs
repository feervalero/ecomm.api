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
    builder.EntitySet<Price>("Prices");
    builder.EntitySet<PriceType>("PriceType"); 
    builder.EntitySet<Product>("Product"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PricesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/Prices
        [EnableQuery]
        public IQueryable<Price> GetPrices()
        {
            return db.Price;
        }

        // GET: odata/Prices(5)
        [EnableQuery]
        public SingleResult<Price> GetPrice([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Price.Where(price => price.Id == key));
        }

        // PUT: odata/Prices(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Price> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Price price = await db.Price.FindAsync(key);
            if (price == null)
            {
                return NotFound();
            }

            patch.Put(price);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(price);
        }

        // POST: odata/Prices
        public async Task<IHttpActionResult> Post(Price price)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Price.Add(price);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PriceExists(price.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(price);
        }

        // PATCH: odata/Prices(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Price> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Price price = await db.Price.FindAsync(key);
            if (price == null)
            {
                return NotFound();
            }

            patch.Patch(price);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(price);
        }

        // DELETE: odata/Prices(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Price price = await db.Price.FindAsync(key);
            if (price == null)
            {
                return NotFound();
            }

            db.Price.Remove(price);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Prices(5)/PriceType
        [EnableQuery]
        public SingleResult<PriceType> GetPriceType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Price.Where(m => m.Id == key).Select(m => m.PriceType));
        }

        // GET: odata/Prices(5)/Product
        [EnableQuery]
        public IQueryable<Product> GetProduct([FromODataUri] Guid key)
        {
            return db.Price.Where(m => m.Id == key).SelectMany(m => m.Product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PriceExists(Guid key)
        {
            return db.Price.Count(e => e.Id == key) > 0;
        }
    }
}
