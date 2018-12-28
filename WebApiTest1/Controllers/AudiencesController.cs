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
    builder.EntitySet<Audience>("Audiences");
    builder.EntitySet<RefreshToken>("RefreshToken"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class AudiencesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/Audiences
        [EnableQuery]
        public IQueryable<Audience> GetAudiences()
        {
            return db.Audience;
        }

        // GET: odata/Audiences(5)
        [EnableQuery]
        public SingleResult<Audience> GetAudience([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Audience.Where(audience => audience.Id == key));
        }

        // PUT: odata/Audiences(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Audience> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Audience audience = await db.Audience.FindAsync(key);
            if (audience == null)
            {
                return NotFound();
            }

            patch.Put(audience);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AudienceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(audience);
        }

        // POST: odata/Audiences
        public async Task<IHttpActionResult> Post(Audience audience)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Audience.Add(audience);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AudienceExists(audience.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(audience);
        }

        // PATCH: odata/Audiences(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Audience> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Audience audience = await db.Audience.FindAsync(key);
            if (audience == null)
            {
                return NotFound();
            }

            patch.Patch(audience);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AudienceExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(audience);
        }

        // DELETE: odata/Audiences(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Audience audience = await db.Audience.FindAsync(key);
            if (audience == null)
            {
                return NotFound();
            }

            db.Audience.Remove(audience);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Audiences(5)/RefreshToken
        [EnableQuery]
        public IQueryable<RefreshToken> GetRefreshToken([FromODataUri] Guid key)
        {
            return db.Audience.Where(m => m.Id == key).SelectMany(m => m.RefreshToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AudienceExists(Guid key)
        {
            return db.Audience.Count(e => e.Id == key) > 0;
        }
    }
}
