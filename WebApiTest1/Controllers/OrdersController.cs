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
    builder.EntitySet<Order>("Orders");
    builder.EntitySet<OrderDetail>("OrderDetail"); 
    builder.EntitySet<Payment>("Payment"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class OrdersController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/Orders
        [EnableQuery]
        public IQueryable<Order> GetOrders()
        {
            return db.Order;
        }

        // GET: odata/Orders(5)
        [EnableQuery]
        public SingleResult<Order> GetOrder([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Order.Where(order => order.Id == key));
        }

        // PUT: odata/Orders(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Order> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Order order = await db.Order.FindAsync(key);
            if (order == null)
            {
                return NotFound();
            }

            patch.Put(order);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(order);
        }

        // POST: odata/Orders
        public async Task<IHttpActionResult> Post(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Order.Add(order);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(order);
        }

        // PATCH: odata/Orders(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Order> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Order order = await db.Order.FindAsync(key);
            if (order == null)
            {
                return NotFound();
            }

            patch.Patch(order);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(order);
        }

        // DELETE: odata/Orders(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Order order = await db.Order.FindAsync(key);
            if (order == null)
            {
                return NotFound();
            }

            db.Order.Remove(order);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Orders(5)/OrderDetail
        [EnableQuery]
        public IQueryable<OrderDetail> GetOrderDetail([FromODataUri] Guid key)
        {
            return db.Order.Where(m => m.Id == key).SelectMany(m => m.OrderDetail);
        }

        // GET: odata/Orders(5)/Payment
        [EnableQuery]
        public IQueryable<Payment> GetPayment([FromODataUri] Guid key)
        {
            return db.Order.Where(m => m.Id == key).SelectMany(m => m.Payment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(Guid key)
        {
            return db.Order.Count(e => e.Id == key) > 0;
        }
    }
}
