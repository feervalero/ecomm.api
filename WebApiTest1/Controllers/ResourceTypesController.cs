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
    builder.EntitySet<ResourceType>("ResourceTypes");
    builder.EntitySet<Promotion>("Promotion"); 
    builder.EntitySet<Resource>("Resource"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ResourceTypesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/ResourceTypes
        [EnableQuery]
        public IQueryable<ResourceType> GetResourceTypes()
        {
            return db.ResourceType;
        }

        // GET: odata/ResourceTypes(5)
        [EnableQuery]
        public SingleResult<ResourceType> GetResourceType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.ResourceType.Where(resourceType => resourceType.Id == key));
        }

        // PUT: odata/ResourceTypes(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<ResourceType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResourceType resourceType = await db.ResourceType.FindAsync(key);
            if (resourceType == null)
            {
                return NotFound();
            }

            patch.Put(resourceType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(resourceType);
        }

        // POST: odata/ResourceTypes
        public async Task<IHttpActionResult> Post(ResourceType resourceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ResourceType.Add(resourceType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ResourceTypeExists(resourceType.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(resourceType);
        }

        // PATCH: odata/ResourceTypes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<ResourceType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResourceType resourceType = await db.ResourceType.FindAsync(key);
            if (resourceType == null)
            {
                return NotFound();
            }

            patch.Patch(resourceType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(resourceType);
        }

        // DELETE: odata/ResourceTypes(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            ResourceType resourceType = await db.ResourceType.FindAsync(key);
            if (resourceType == null)
            {
                return NotFound();
            }

            db.ResourceType.Remove(resourceType);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ResourceTypes(5)/Promotion
        [EnableQuery]
        public IQueryable<Promotion> GetPromotion([FromODataUri] Guid key)
        {
            return db.ResourceType.Where(m => m.Id == key).SelectMany(m => m.Promotion);
        }

        // GET: odata/ResourceTypes(5)/Resource
        [EnableQuery]
        public IQueryable<Resource> GetResource([FromODataUri] Guid key)
        {
            return db.ResourceType.Where(m => m.Id == key).SelectMany(m => m.Resource);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ResourceTypeExists(Guid key)
        {
            return db.ResourceType.Count(e => e.Id == key) > 0;
        }
    }
}
