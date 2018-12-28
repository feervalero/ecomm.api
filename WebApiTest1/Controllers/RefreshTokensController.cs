﻿using System;
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
    builder.EntitySet<RefreshToken>("RefreshTokens");
    builder.EntitySet<Audience>("Audience"); 
    builder.EntitySet<User>("User"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class RefreshTokensController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/RefreshTokens
        [EnableQuery]
        public IQueryable<RefreshToken> GetRefreshTokens()
        {
            return db.RefreshToken;
        }

        // GET: odata/RefreshTokens(5)
        [EnableQuery]
        public SingleResult<RefreshToken> GetRefreshToken([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RefreshToken.Where(refreshToken => refreshToken.Id == key));
        }

        // PUT: odata/RefreshTokens(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<RefreshToken> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RefreshToken refreshToken = await db.RefreshToken.FindAsync(key);
            if (refreshToken == null)
            {
                return NotFound();
            }

            patch.Put(refreshToken);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RefreshTokenExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(refreshToken);
        }

        // POST: odata/RefreshTokens
        public async Task<IHttpActionResult> Post(RefreshToken refreshToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RefreshToken.Add(refreshToken);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RefreshTokenExists(refreshToken.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(refreshToken);
        }

        // PATCH: odata/RefreshTokens(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<RefreshToken> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RefreshToken refreshToken = await db.RefreshToken.FindAsync(key);
            if (refreshToken == null)
            {
                return NotFound();
            }

            patch.Patch(refreshToken);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RefreshTokenExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(refreshToken);
        }

        // DELETE: odata/RefreshTokens(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            RefreshToken refreshToken = await db.RefreshToken.FindAsync(key);
            if (refreshToken == null)
            {
                return NotFound();
            }

            db.RefreshToken.Remove(refreshToken);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/RefreshTokens(5)/Audience
        [EnableQuery]
        public SingleResult<Audience> GetAudience([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RefreshToken.Where(m => m.Id == key).Select(m => m.Audience));
        }

        // GET: odata/RefreshTokens(5)/User
        [EnableQuery]
        public SingleResult<User> GetUser([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RefreshToken.Where(m => m.Id == key).Select(m => m.User));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RefreshTokenExists(Guid key)
        {
            return db.RefreshToken.Count(e => e.Id == key) > 0;
        }
    }
}
