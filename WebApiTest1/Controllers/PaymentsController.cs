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
    builder.EntitySet<Payment>("Payments");
    builder.EntitySet<Order>("Order"); 
    builder.EntitySet<PaymentMethod>("PaymentMethod"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PaymentsController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/Payments
        [EnableQuery]
        public IQueryable<Payment> GetPayments()
        {
            return db.Payment;
        }

        // GET: odata/Payments(5)
        [EnableQuery]
        public SingleResult<Payment> GetPayment([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Payment.Where(payment => payment.Id == key));
        }

        // PUT: odata/Payments(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Payment> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Payment payment = await db.Payment.FindAsync(key);
            if (payment == null)
            {
                return NotFound();
            }

            patch.Put(payment);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(payment);
        }

        // POST: odata/Payments
        public async Task<IHttpActionResult> Post(Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Payment.Add(payment);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PaymentExists(payment.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(payment);
        }

        // PATCH: odata/Payments(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Payment> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Payment payment = await db.Payment.FindAsync(key);
            if (payment == null)
            {
                return NotFound();
            }

            patch.Patch(payment);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(payment);
        }

        // DELETE: odata/Payments(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Payment payment = await db.Payment.FindAsync(key);
            if (payment == null)
            {
                return NotFound();
            }

            db.Payment.Remove(payment);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Payments(5)/Order
        [EnableQuery]
        public SingleResult<Order> GetOrder([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Payment.Where(m => m.Id == key).Select(m => m.Order));
        }

        // GET: odata/Payments(5)/PaymentMethod
        [EnableQuery]
        public SingleResult<PaymentMethod> GetPaymentMethod([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Payment.Where(m => m.Id == key).Select(m => m.PaymentMethod));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaymentExists(Guid key)
        {
            return db.Payment.Count(e => e.Id == key) > 0;
        }
    }
}
