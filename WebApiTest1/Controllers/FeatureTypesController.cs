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
    builder.EntitySet<FeatureType>("FeatureTypes");
    builder.EntitySet<ProductFeature>("ProductFeature"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class FeatureTypesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/FeatureTypes
        [EnableQuery]
        public IQueryable<FeatureType> GetFeatureTypes()
        {
            return db.FeatureType;
        }

        // GET: odata/FeatureTypes(5)
        [EnableQuery]
        public SingleResult<FeatureType> GetFeatureType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FeatureType.Where(featureType => featureType.Id == key));
        }

        // PUT: odata/FeatureTypes(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<FeatureType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FeatureType featureType = await db.FeatureType.FindAsync(key);
            if (featureType == null)
            {
                return NotFound();
            }

            patch.Put(featureType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(featureType);
        }

        // POST: odata/FeatureTypes
        public async Task<IHttpActionResult> Post(FeatureType featureType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FeatureType.Add(featureType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FeatureTypeExists(featureType.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(featureType);
        }

        // PATCH: odata/FeatureTypes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<FeatureType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FeatureType featureType = await db.FeatureType.FindAsync(key);
            if (featureType == null)
            {
                return NotFound();
            }

            patch.Patch(featureType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(featureType);
        }

        // DELETE: odata/FeatureTypes(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            FeatureType featureType = await db.FeatureType.FindAsync(key);
            if (featureType == null)
            {
                return NotFound();
            }

            db.FeatureType.Remove(featureType);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/FeatureTypes(5)/ProductFeature
        [EnableQuery]
        public IQueryable<ProductFeature> GetProductFeature([FromODataUri] Guid key)
        {
            return db.FeatureType.Where(m => m.Id == key).SelectMany(m => m.ProductFeature);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeatureTypeExists(Guid key)
        {
            return db.FeatureType.Count(e => e.Id == key) > 0;
        }
    }
}
