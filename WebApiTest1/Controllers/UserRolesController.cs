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
    builder.EntitySet<UserRole>("UserRoles");
    builder.EntitySet<Role>("Role"); 
    builder.EntitySet<User>("User"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UserRolesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/UserRoles
        [EnableQuery]
        public IQueryable<UserRole> GetUserRoles()
        {
            return db.UserRole;
        }

        // GET: odata/UserRoles(5)
        [EnableQuery]
        public SingleResult<UserRole> GetUserRole([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.UserRole.Where(userRole => userRole.Id == key));
        }

        // PUT: odata/UserRoles(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<UserRole> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserRole userRole = await db.UserRole.FindAsync(key);
            if (userRole == null)
            {
                return NotFound();
            }

            patch.Put(userRole);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(userRole);
        }

        // POST: odata/UserRoles
        public async Task<IHttpActionResult> Post(UserRole userRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserRole.Add(userRole);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserRoleExists(userRole.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(userRole);
        }

        // PATCH: odata/UserRoles(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<UserRole> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserRole userRole = await db.UserRole.FindAsync(key);
            if (userRole == null)
            {
                return NotFound();
            }

            patch.Patch(userRole);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(userRole);
        }

        // DELETE: odata/UserRoles(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            UserRole userRole = await db.UserRole.FindAsync(key);
            if (userRole == null)
            {
                return NotFound();
            }

            db.UserRole.Remove(userRole);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/UserRoles(5)/Role
        [EnableQuery]
        public SingleResult<Role> GetRole([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.UserRole.Where(m => m.Id == key).Select(m => m.Role));
        }

        // GET: odata/UserRoles(5)/User
        [EnableQuery]
        public SingleResult<User> GetUser([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.UserRole.Where(m => m.Id == key).Select(m => m.User));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserRoleExists(Guid key)
        {
            return db.UserRole.Count(e => e.Id == key) > 0;
        }
    }
}
