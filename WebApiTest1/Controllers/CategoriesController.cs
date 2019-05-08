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
using WebApiTest1.Models;

namespace WebApiTest1.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApiTest1.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Category>("Categories");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CategoriesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/Categories
        [EnableQuery]
        public IQueryable<Category> GetCategories()
        {
            return db.Category;
        }

        // GET: odata/Categories(5)
        [EnableQuery]
        public SingleResult<Category> GetCategory([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Category.Where(category => category.Id == key));
        }

        // PUT: odata/Categories(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Category> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = await db.Category.FindAsync(key);
            if (category == null)
            {
                return NotFound();
            }

            patch.Put(category);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(category);
        }

        // POST: odata/Categories
        public async Task<IHttpActionResult> Post(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Category.Add(category);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CategoryExists(category.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(category);
        }

        // PATCH: odata/Categories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Category> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = await db.Category.FindAsync(key);
            if (category == null)
            {
                return NotFound();
            }

            patch.Patch(category);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(category);
        }

        // DELETE: odata/Categories(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Category category = await db.Category.FindAsync(key);
            if (category == null)
            {
                return NotFound();
            }

            db.Category.Remove(category);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(Guid key)
        {
            return db.Category.Count(e => e.Id == key) > 0;
        }
    }
}
