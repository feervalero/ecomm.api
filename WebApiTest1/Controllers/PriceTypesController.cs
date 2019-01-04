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
    builder.EntitySet<PriceType>("PriceTypes");
    builder.EntitySet<Price>("Price"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PriceTypesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/PriceTypes
        [EnableQuery]
        public IQueryable<PriceType> GetPriceTypes()
        {
            return db.PriceType;
        }

        // GET: odata/PriceTypes(5)
        [EnableQuery]
        public SingleResult<PriceType> GetPriceType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.PriceType.Where(priceType => priceType.Id == key));
        }

        // PUT: odata/PriceTypes(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<PriceType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PriceType priceType = await db.PriceType.FindAsync(key);
            if (priceType == null)
            {
                return NotFound();
            }

            patch.Put(priceType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(priceType);
        }

        // POST: odata/PriceTypes
        public async Task<IHttpActionResult> Post(PriceType priceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PriceType.Add(priceType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PriceTypeExists(priceType.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(priceType);
        }

        // PATCH: odata/PriceTypes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<PriceType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PriceType priceType = await db.PriceType.FindAsync(key);
            if (priceType == null)
            {
                return NotFound();
            }

            patch.Patch(priceType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(priceType);
        }

        // DELETE: odata/PriceTypes(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            PriceType priceType = await db.PriceType.FindAsync(key);
            if (priceType == null)
            {
                return NotFound();
            }

            db.PriceType.Remove(priceType);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/PriceTypes(5)/Price
        [EnableQuery]
        public IQueryable<Price> GetPrice([FromODataUri] Guid key)
        {
            return db.PriceType.Where(m => m.Id == key).SelectMany(m => m.Price);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PriceTypeExists(Guid key)
        {
            return db.PriceType.Count(e => e.Id == key) > 0;
        }
    }
}
