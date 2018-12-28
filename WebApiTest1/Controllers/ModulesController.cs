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
    builder.EntitySet<Module>("Modules");
    builder.EntitySet<Product>("Product"); 
    builder.EntitySet<Resource>("Resource"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ModulesController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/Modules
        [EnableQuery]
        public IQueryable<Module> GetModules()
        {
            return db.Module;
        }

        // GET: odata/Modules(5)
        [EnableQuery]
        public SingleResult<Module> GetModule([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Module.Where(module => module.Id == key));
        }

        // PUT: odata/Modules(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Module> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Module module = await db.Module.FindAsync(key);
            if (module == null)
            {
                return NotFound();
            }

            patch.Put(module);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(module);
        }

        // POST: odata/Modules
        public async Task<IHttpActionResult> Post(Module module)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Module.Add(module);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ModuleExists(module.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(module);
        }

        // PATCH: odata/Modules(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Module> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Module module = await db.Module.FindAsync(key);
            if (module == null)
            {
                return NotFound();
            }

            patch.Patch(module);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(module);
        }

        // DELETE: odata/Modules(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Module module = await db.Module.FindAsync(key);
            if (module == null)
            {
                return NotFound();
            }

            db.Module.Remove(module);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Modules(5)/Product
        [EnableQuery]
        public IQueryable<Product> GetProduct([FromODataUri] Guid key)
        {
            return db.Module.Where(m => m.Id == key).SelectMany(m => m.Product);
        }

        // GET: odata/Modules(5)/Resource
        [EnableQuery]
        public IQueryable<Resource> GetResource([FromODataUri] Guid key)
        {
            return db.Module.Where(m => m.Id == key).SelectMany(m => m.Resource);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModuleExists(Guid key)
        {
            return db.Module.Count(e => e.Id == key) > 0;
        }
    }
}
