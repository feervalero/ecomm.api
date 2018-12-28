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
    builder.EntitySet<PromotionType>("PromotionTypes");
    builder.EntitySet<Promotion>("Promotion"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PromotionTypesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/PromotionTypes
        [EnableQuery]
        public IQueryable<PromotionType> GetPromotionTypes()
        {
            return db.PromotionType;
        }

        // GET: odata/PromotionTypes(5)
        [EnableQuery]
        public SingleResult<PromotionType> GetPromotionType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.PromotionType.Where(promotionType => promotionType.Id == key));
        }

        // PUT: odata/PromotionTypes(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<PromotionType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PromotionType promotionType = await db.PromotionType.FindAsync(key);
            if (promotionType == null)
            {
                return NotFound();
            }

            patch.Put(promotionType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(promotionType);
        }

        // POST: odata/PromotionTypes
        public async Task<IHttpActionResult> Post(PromotionType promotionType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PromotionType.Add(promotionType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PromotionTypeExists(promotionType.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(promotionType);
        }

        // PATCH: odata/PromotionTypes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<PromotionType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PromotionType promotionType = await db.PromotionType.FindAsync(key);
            if (promotionType == null)
            {
                return NotFound();
            }

            patch.Patch(promotionType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(promotionType);
        }

        // DELETE: odata/PromotionTypes(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            PromotionType promotionType = await db.PromotionType.FindAsync(key);
            if (promotionType == null)
            {
                return NotFound();
            }

            db.PromotionType.Remove(promotionType);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/PromotionTypes(5)/Promotion
        [EnableQuery]
        public IQueryable<Promotion> GetPromotion([FromODataUri] Guid key)
        {
            return db.PromotionType.Where(m => m.Id == key).SelectMany(m => m.Promotion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PromotionTypeExists(Guid key)
        {
            return db.PromotionType.Count(e => e.Id == key) > 0;
        }
    }
}
