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
    builder.EntitySet<ProductFeature>("ProductFeatures");
    builder.EntitySet<FeatureType>("FeatureType"); 
    builder.EntitySet<Product>("Product"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ProductFeaturesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/ProductFeatures
        [EnableQuery]
        public IQueryable<ProductFeature> GetProductFeatures()
        {
            return db.ProductFeature;
        }

        // GET: odata/ProductFeatures(5)
        [EnableQuery]
        public SingleResult<ProductFeature> GetProductFeature([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.ProductFeature.Where(productFeature => productFeature.Id == key));
        }

        // PUT: odata/ProductFeatures(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<ProductFeature> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductFeature productFeature = await db.ProductFeature.FindAsync(key);
            if (productFeature == null)
            {
                return NotFound();
            }

            patch.Put(productFeature);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductFeatureExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productFeature);
        }

        // POST: odata/ProductFeatures
        public async Task<IHttpActionResult> Post(ProductFeature productFeature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProductFeature.Add(productFeature);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductFeatureExists(productFeature.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(productFeature);
        }

        // PATCH: odata/ProductFeatures(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<ProductFeature> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductFeature productFeature = await db.ProductFeature.FindAsync(key);
            if (productFeature == null)
            {
                return NotFound();
            }

            patch.Patch(productFeature);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductFeatureExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(productFeature);
        }

        // DELETE: odata/ProductFeatures(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            ProductFeature productFeature = await db.ProductFeature.FindAsync(key);
            if (productFeature == null)
            {
                return NotFound();
            }

            db.ProductFeature.Remove(productFeature);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ProductFeatures(5)/FeatureType
        [EnableQuery]
        public SingleResult<FeatureType> GetFeatureType([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.ProductFeature.Where(m => m.Id == key).Select(m => m.FeatureType));
        }

        // GET: odata/ProductFeatures(5)/Product
        [EnableQuery]
        public IQueryable<Product> GetProduct([FromODataUri] Guid key)
        {
            return db.ProductFeature.Where(m => m.Id == key).SelectMany(m => m.Product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductFeatureExists(Guid key)
        {
            return db.ProductFeature.Count(e => e.Id == key) > 0;
        }
    }
}
