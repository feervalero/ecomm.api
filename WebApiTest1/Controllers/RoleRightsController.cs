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
    builder.EntitySet<RoleRight>("RoleRights");
    builder.EntitySet<Resource>("Resource"); 
    builder.EntitySet<Role>("Role"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class RoleRightsController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/RoleRights
        [EnableQuery]
        public IQueryable<RoleRight> GetRoleRights()
        {
            return db.RoleRight;
        }

        // GET: odata/RoleRights(5)
        [EnableQuery]
        public SingleResult<RoleRight> GetRoleRight([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RoleRight.Where(roleRight => roleRight.Id == key));
        }

        // PUT: odata/RoleRights(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<RoleRight> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RoleRight roleRight = await db.RoleRight.FindAsync(key);
            if (roleRight == null)
            {
                return NotFound();
            }

            patch.Put(roleRight);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleRightExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(roleRight);
        }

        // POST: odata/RoleRights
        public async Task<IHttpActionResult> Post(RoleRight roleRight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RoleRight.Add(roleRight);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RoleRightExists(roleRight.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(roleRight);
        }

        // PATCH: odata/RoleRights(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<RoleRight> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RoleRight roleRight = await db.RoleRight.FindAsync(key);
            if (roleRight == null)
            {
                return NotFound();
            }

            patch.Patch(roleRight);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleRightExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(roleRight);
        }

        // DELETE: odata/RoleRights(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            RoleRight roleRight = await db.RoleRight.FindAsync(key);
            if (roleRight == null)
            {
                return NotFound();
            }

            db.RoleRight.Remove(roleRight);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/RoleRights(5)/Resource
        [EnableQuery]
        public SingleResult<Resource> GetResource([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RoleRight.Where(m => m.Id == key).Select(m => m.Resource));
        }

        // GET: odata/RoleRights(5)/Role
        [EnableQuery]
        public SingleResult<Role> GetRole([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RoleRight.Where(m => m.Id == key).Select(m => m.Role));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoleRightExists(Guid key)
        {
            return db.RoleRight.Count(e => e.Id == key) > 0;
        }
    }
}
