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
    builder.EntitySet<Promotion>("Promotions");
    builder.EntitySet<PromotionType>("PromotionType"); 
    builder.EntitySet<ResourceType>("ResourceType"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PromotionsController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/Promotions
        [EnableQuery]
        public IQueryable<Promotion> GetPromotions()
        {
            return db.Promotion;
        }

        // GET: odata/Promotions(5)
        [EnableQuery]
        public SingleResult<Promotion> GetPromotion([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Promotion.Where(promotion => promotion.Id == key));
        }

        // PUT: odata/Promotions(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Promotion> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Promotion promotion = await db.Promotion.FindAsync(key);
            if (promotion == null)
            {
                return NotFound();
            }

            patch.Put(promotion);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(promotion);
        }

        // POST: odata/Promotions
        public async Task<IHttpActionResult> Post(Promotion promotion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Promotion.Add(promotion);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PromotionExists(promotion.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(promotion);
        }

        // PATCH: odata/Promotions(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Promotion> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Promotion promotion = await db.Promotion.FindAsync(key);
            if (promotion == null)
            {
                return NotFound();
            }

            patch.Patch(promotion);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(promotion);
        }

        // DELETE: odata/Promotions(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Promotion promotion = await db.Promotion.FindAsync(key);
            if (promotion == null)
            {
                return NotFound();
            }

            db.Promotion.Remove(promotion);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Promotions(5)/PromotionType
        [EnableQuery]
        public SingleResult<PromotionType> GetPromotionType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Promotion.Where(m => m.Id == key).Select(m => m.PromotionType));
        }

        // GET: odata/Promotions(5)/ResourceType
        [EnableQuery]
        public SingleResult<ResourceType> GetResourceType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Promotion.Where(m => m.Id == key).Select(m => m.ResourceType));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PromotionExists(Guid key)
        {
            return db.Promotion.Count(e => e.Id == key) > 0;
        }
    }
}
