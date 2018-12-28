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
    builder.EntitySet<Role>("Roles");
    builder.EntitySet<RoleRight>("RoleRight"); 
    builder.EntitySet<UserRole>("UserRole"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class RolesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/Roles
        [EnableQuery]
        public IQueryable<Role> GetRoles()
        {
            return db.Role;
        }

        // GET: odata/Roles(5)
        [EnableQuery]
        public SingleResult<Role> GetRole([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Role.Where(role => role.Id == key));
        }

        // PUT: odata/Roles(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Role> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Role role = await db.Role.FindAsync(key);
            if (role == null)
            {
                return NotFound();
            }

            patch.Put(role);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(role);
        }

        // POST: odata/Roles
        public async Task<IHttpActionResult> Post(Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Role.Add(role);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RoleExists(role.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(role);
        }

        // PATCH: odata/Roles(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Role> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Role role = await db.Role.FindAsync(key);
            if (role == null)
            {
                return NotFound();
            }

            patch.Patch(role);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(role);
        }

        // DELETE: odata/Roles(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Role role = await db.Role.FindAsync(key);
            if (role == null)
            {
                return NotFound();
            }

            db.Role.Remove(role);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Roles(5)/RoleRight
        [EnableQuery]
        public IQueryable<RoleRight> GetRoleRight([FromODataUri] Guid key)
        {
            return db.Role.Where(m => m.Id == key).SelectMany(m => m.RoleRight);
        }

        // GET: odata/Roles(5)/UserRole
        [EnableQuery]
        public IQueryable<UserRole> GetUserRole([FromODataUri] Guid key)
        {
            return db.Role.Where(m => m.Id == key).SelectMany(m => m.UserRole);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoleExists(Guid key)
        {
            return db.Role.Count(e => e.Id == key) > 0;
        }
    }
}
