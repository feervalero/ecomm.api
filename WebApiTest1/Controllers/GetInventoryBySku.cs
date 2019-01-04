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
using WebApiTest1.Models;

namespace WebApiTest1.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApiTest1.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<vwInventoryBySku>("GetInventoryBySku");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class GetInventoryBySkuController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/GetInventoryBySku
        [EnableQuery]
        public IQueryable<vwInventoryBySku> GetGetInventoryBySku()
        {
            return db.vwInventoryBySku;
        }

        // GET: odata/GetInventoryBySku(5)
        [EnableQuery]
        public SingleResult<vwInventoryBySku> GetvwInventoryBySku([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.vwInventoryBySku.Where(vwInventoryBySku => vwInventoryBySku.Id == key));
        }

        // PUT: odata/GetInventoryBySku(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<vwInventoryBySku> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            vwInventoryBySku vwInventoryBySku = await db.vwInventoryBySku.FindAsync(key);
            if (vwInventoryBySku == null)
            {
                return NotFound();
            }

            patch.Put(vwInventoryBySku);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vwInventoryBySkuExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(vwInventoryBySku);
        }

        // POST: odata/GetInventoryBySku
        public async Task<IHttpActionResult> Post(vwInventoryBySku vwInventoryBySku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.vwInventoryBySku.Add(vwInventoryBySku);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (vwInventoryBySkuExists(vwInventoryBySku.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(vwInventoryBySku);
        }

        // PATCH: odata/GetInventoryBySku(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<vwInventoryBySku> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            vwInventoryBySku vwInventoryBySku = await db.vwInventoryBySku.FindAsync(key);
            if (vwInventoryBySku == null)
            {
                return NotFound();
            }

            patch.Patch(vwInventoryBySku);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vwInventoryBySkuExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(vwInventoryBySku);
        }

        // DELETE: odata/GetInventoryBySku(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            vwInventoryBySku vwInventoryBySku = await db.vwInventoryBySku.FindAsync(key);
            if (vwInventoryBySku == null)
            {
                return NotFound();
            }

            db.vwInventoryBySku.Remove(vwInventoryBySku);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vwInventoryBySkuExists(Guid key)
        {
            return db.vwInventoryBySku.Count(e => e.Id == key) > 0;
        }
    }
}
