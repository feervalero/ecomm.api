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
    builder.EntitySet<PaymentMethod>("PaymentMethods");
    builder.EntitySet<Payment>("Payment"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PaymentMethodsController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/PaymentMethods
        [EnableQuery]
        public IQueryable<PaymentMethod> GetPaymentMethods()
        {
            return db.PaymentMethod;
        }

        // GET: odata/PaymentMethods(5)
        [EnableQuery]
        public SingleResult<PaymentMethod> GetPaymentMethod([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.PaymentMethod.Where(paymentMethod => paymentMethod.Id == key));
        }

        // PUT: odata/PaymentMethods(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<PaymentMethod> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PaymentMethod paymentMethod = await db.PaymentMethod.FindAsync(key);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            patch.Put(paymentMethod);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentMethodExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(paymentMethod);
        }

        // POST: odata/PaymentMethods
        public async Task<IHttpActionResult> Post(PaymentMethod paymentMethod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PaymentMethod.Add(paymentMethod);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PaymentMethodExists(paymentMethod.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(paymentMethod);
        }

        // PATCH: odata/PaymentMethods(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<PaymentMethod> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PaymentMethod paymentMethod = await db.PaymentMethod.FindAsync(key);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            patch.Patch(paymentMethod);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentMethodExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(paymentMethod);
        }

        // DELETE: odata/PaymentMethods(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            PaymentMethod paymentMethod = await db.PaymentMethod.FindAsync(key);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            db.PaymentMethod.Remove(paymentMethod);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/PaymentMethods(5)/Payment
        [EnableQuery]
        public IQueryable<Payment> GetPayment([FromODataUri] Guid key)
        {
            return db.PaymentMethod.Where(m => m.Id == key).SelectMany(m => m.Payment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaymentMethodExists(Guid key)
        {
            return db.PaymentMethod.Count(e => e.Id == key) > 0;
        }
    }
}
