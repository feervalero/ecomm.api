using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using WebApiTest1.Models;

namespace WebApiTest1.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApiTest1.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Resource>("Resources");
    builder.EntitySet<Module>("Module"); 
    builder.EntitySet<ResourceType>("ResourceType"); 
    builder.EntitySet<RoleRight>("RoleRight"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ResourcesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/Resources
        [EnableQuery]
        public IQueryable<Resource> GetResources()
        {
            return db.Resource;
        }

        // GET: odata/Resources(5)
        [EnableQuery]
        public SingleResult<Resource> GetResource([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Resource.Where(resource => resource.Id == key));
        }

        // PUT: odata/Resources(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Resource> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Resource resource = await db.Resource.FindAsync(key);
            if (resource == null)
            {
                return NotFound();
            }

            patch.Put(resource);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(resource);
        }

        // POST: odata/Resources
        public async Task<IHttpActionResult> Post(Resource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Resource.Add(resource);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ResourceExists(resource.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(resource);
        }

        // PATCH: odata/Resources(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Resource> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Resource resource = await db.Resource.FindAsync(key);
            if (resource == null)
            {
                return NotFound();
            }

            patch.Patch(resource);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(resource);
        }

        // DELETE: odata/Resources(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Resource resource = await db.Resource.FindAsync(key);
            if (resource == null)
            {
                return NotFound();
            }

            db.Resource.Remove(resource);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Resources(5)/Module
        [EnableQuery]
        public SingleResult<Module> GetModule([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Resource.Where(m => m.Id == key).Select(m => m.Module));
        }

        // GET: odata/Resources(5)/Resource1
        [EnableQuery]
        public IQueryable<Resource> GetResource1([FromODataUri] Guid key)
        {
            return db.Resource.Where(m => m.Id == key).SelectMany(m => m.Resource1);
        }

        // GET: odata/Resources(5)/Resource2
        [EnableQuery]
        public SingleResult<Resource> GetResource2([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Resource.Where(m => m.Id == key).Select(m => m.Resource2));
        }

        // GET: odata/Resources(5)/ResourceType
        [EnableQuery]
        public SingleResult<ResourceType> GetResourceType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Resource.Where(m => m.Id == key).Select(m => m.ResourceType));
        }

        // GET: odata/Resources(5)/RoleRight
        [EnableQuery]
        public IQueryable<RoleRight> GetRoleRight([FromODataUri] Guid key)
        {
            return db.Resource.Where(m => m.Id == key).SelectMany(m => m.RoleRight);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ResourceExists(Guid key)
        {
            return db.Resource.Count(e => e.Id == key) > 0;
        }
    }
}
