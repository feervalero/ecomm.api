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
    builder.EntitySet<OrderDetail>("OrderDetails");
    builder.EntitySet<Order>("Order"); 
    builder.EntitySet<Product>("Product"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class OrderDetailsController : ODataController
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: odata/OrderDetails
        [EnableQuery]
        public IQueryable<OrderDetail> GetOrderDetails()
        {
            return db.OrderDetail;
        }

        // GET: odata/OrderDetails(5)
        [EnableQuery]
        public SingleResult<OrderDetail> GetOrderDetail([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.OrderDetail.Where(orderDetail => orderDetail.Id == key));
        }

        // PUT: odata/OrderDetails(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<OrderDetail> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            OrderDetail orderDetail = await db.OrderDetail.FindAsync(key);
            if (orderDetail == null)
            {
                return NotFound();
            }

            patch.Put(orderDetail);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(orderDetail);
        }

        // POST: odata/OrderDetails
        public async Task<IHttpActionResult> Post(OrderDetail orderDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrderDetail.Add(orderDetail);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderDetailExists(orderDetail.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(orderDetail);
        }

        // PATCH: odata/OrderDetails(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<OrderDetail> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            OrderDetail orderDetail = await db.OrderDetail.FindAsync(key);
            if (orderDetail == null)
            {
                return NotFound();
            }

            patch.Patch(orderDetail);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(orderDetail);
        }

        // DELETE: odata/OrderDetails(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            OrderDetail orderDetail = await db.OrderDetail.FindAsync(key);
            if (orderDetail == null)
            {
                return NotFound();
            }

            db.OrderDetail.Remove(orderDetail);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/OrderDetails(5)/Order
        [EnableQuery]
        public SingleResult<Order> GetOrder([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.OrderDetail.Where(m => m.Id == key).Select(m => m.Order));
        }

        // GET: odata/OrderDetails(5)/Product
        [EnableQuery]
        public SingleResult<Product> GetProduct([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.OrderDetail.Where(m => m.Id == key).Select(m => m.Product));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderDetailExists(Guid key)
        {
            return db.OrderDetail.Count(e => e.Id == key) > 0;
        }
    }
}
