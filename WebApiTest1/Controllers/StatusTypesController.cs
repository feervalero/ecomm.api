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
    builder.EntitySet<StatusType>("StatusTypes");
    builder.EntitySet<Inventory>("Inventory"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class StatusTypesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/StatusTypes
        [EnableQuery]
        public IQueryable<StatusType> GetStatusTypes()
        {
            return db.StatusType;
        }

        // GET: odata/StatusTypes(5)
        [EnableQuery]
        public SingleResult<StatusType> GetStatusType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.StatusType.Where(statusType => statusType.Id == key));
        }

        // PUT: odata/StatusTypes(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<StatusType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StatusType statusType = await db.StatusType.FindAsync(key);
            if (statusType == null)
            {
                return NotFound();
            }

            patch.Put(statusType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(statusType);
        }

        // POST: odata/StatusTypes
        public async Task<IHttpActionResult> Post(StatusType statusType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StatusType.Add(statusType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StatusTypeExists(statusType.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(statusType);
        }

        // PATCH: odata/StatusTypes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<StatusType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StatusType statusType = await db.StatusType.FindAsync(key);
            if (statusType == null)
            {
                return NotFound();
            }

            patch.Patch(statusType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(statusType);
        }

        // DELETE: odata/StatusTypes(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            StatusType statusType = await db.StatusType.FindAsync(key);
            if (statusType == null)
            {
                return NotFound();
            }

            db.StatusType.Remove(statusType);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/StatusTypes(5)/Inventory
        [EnableQuery]
        public IQueryable<Inventory> GetInventory([FromODataUri] Guid key)
        {
            return db.StatusType.Where(m => m.Id == key).SelectMany(m => m.Inventory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StatusTypeExists(Guid key)
        {
            return db.StatusType.Count(e => e.Id == key) > 0;
        }
    }
}
