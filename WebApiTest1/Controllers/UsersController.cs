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
    builder.EntitySet<User>("Users");
    builder.EntitySet<RefreshToken>("RefreshToken"); 
    builder.EntitySet<UserType>("UserType"); 
    builder.EntitySet<UserClaim>("UserClaim"); 
    builder.EntitySet<UserRole>("UserRole"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UsersController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/Users
        [EnableQuery]
        public IQueryable<User> GetUsers()
        {
            return db.User;
        }

        // GET: odata/Users(5)
        [EnableQuery]
        public SingleResult<User> GetUser([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.User.Where(user => user.Id == key));
        }

        // PUT: odata/Users(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<User> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await db.User.FindAsync(key);
            if (user == null)
            {
                return NotFound();
            }

            patch.Put(user);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(user);
        }

        // POST: odata/Users
        public async Task<IHttpActionResult> Post(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User.Add(user);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(user);
        }

        // PATCH: odata/Users(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<User> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await db.User.FindAsync(key);
            if (user == null)
            {
                return NotFound();
            }

            patch.Patch(user);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(user);
        }

        // DELETE: odata/Users(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            User user = await db.User.FindAsync(key);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Users(5)/RefreshToken
        [EnableQuery]
        public IQueryable<RefreshToken> GetRefreshToken([FromODataUri] Guid key)
        {
            return db.User.Where(m => m.Id == key).SelectMany(m => m.RefreshToken);
        }

        // GET: odata/Users(5)/UserType
        [EnableQuery]
        public SingleResult<UserType> GetUserType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.User.Where(m => m.Id == key).Select(m => m.UserType));
        }

        // GET: odata/Users(5)/UserClaim
        [EnableQuery]
        public IQueryable<UserClaim> GetUserClaim([FromODataUri] Guid key)
        {
            return db.User.Where(m => m.Id == key).SelectMany(m => m.UserClaim);
        }

        // GET: odata/Users(5)/UserRole
        [EnableQuery]
        public IQueryable<UserRole> GetUserRole([FromODataUri] Guid key)
        {
            return db.User.Where(m => m.Id == key).SelectMany(m => m.UserRole);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(Guid key)
        {
            return db.User.Count(e => e.Id == key) > 0;
        }
    }
}
