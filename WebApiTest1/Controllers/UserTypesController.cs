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
    builder.EntitySet<UserType>("UserTypes");
    builder.EntitySet<User>("User"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UserTypesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/UserTypes
        [EnableQuery]
        public IQueryable<UserType> GetUserTypes()
        {
            return db.UserType;
        }

        // GET: odata/UserTypes(5)
        [EnableQuery]
        public SingleResult<UserType> GetUserType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.UserType.Where(userType => userType.Id == key));
        }

        // PUT: odata/UserTypes(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<UserType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserType userType = await db.UserType.FindAsync(key);
            if (userType == null)
            {
                return NotFound();
            }

            patch.Put(userType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(userType);
        }

        // POST: odata/UserTypes
        public async Task<IHttpActionResult> Post(UserType userType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserType.Add(userType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserTypeExists(userType.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(userType);
        }

        // PATCH: odata/UserTypes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<UserType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserType userType = await db.UserType.FindAsync(key);
            if (userType == null)
            {
                return NotFound();
            }

            patch.Patch(userType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(userType);
        }

        // DELETE: odata/UserTypes(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            UserType userType = await db.UserType.FindAsync(key);
            if (userType == null)
            {
                return NotFound();
            }

            db.UserType.Remove(userType);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/UserTypes(5)/User
        [EnableQuery]
        public IQueryable<User> GetUser([FromODataUri] Guid key)
        {
            return db.UserType.Where(m => m.Id == key).SelectMany(m => m.User);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserTypeExists(Guid key)
        {
            return db.UserType.Count(e => e.Id == key) > 0;
        }
    }
}
