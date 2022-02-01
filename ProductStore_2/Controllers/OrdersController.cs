using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProductStore_2.Models;

namespace ProductStore_2.Controllers
{

    public class OrdersController : ApiController
    {
        private OrdersContext db = new OrdersContext();

        // GET: api/Orders
        public IEnumerable<Order> GetOrders()
        {
            return db.Orders.Where(o => o.Customer == User.Identity.Name);
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public OrderDTO GetOrder(int id)
        {
            //When we query the database for the order, we use the DbQuery.Include method to fetch the related OrderDetail and Product entities.
            Order order = db.Orders.Include("OrderDetails.Product")
                .First(o => o.Id == id);
                //.First(o => o.Id == id && o.Customer == User.Identity.Name);

            if (order == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            // The return value is an OrderDTO instance, instead of an Order.
            return new OrderDTO()
            {
                Details = from d in order.OrderDetails
                          select new OrderDTO.Detail()
                          {
                              ProductID = d.Product.Id,
                              Product = d.Product.Name,
                              Price = d.Product.Price,
                              Quantity = d.Quantity
                          }
            };
        }

        // PUT: api/Orders/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutOrder(int id, Order order)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != order.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(order).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public HttpResponseMessage PostOrder(OrderDTO dto)
        {
            if (ModelState.IsValid)
            {
                var order = new Order()
                {
                    //Customer = User.Identity.Name,
                    Customer = "Francisco",
                    OrderDetails = (from item in dto.Details
                                    select new OrderDetail()
                                    { ProductId = item.ProductID, Quantity = item.Quantity }).ToList()
                };

                db.Orders.Add(order);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, order);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = order.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE: api/Orders/5
        //[ResponseType(typeof(Order))]
        //public IHttpActionResult DeleteOrder(int id)
        //{
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Orders.Remove(order);
        //    db.SaveChanges();

        //    return Ok(order);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}