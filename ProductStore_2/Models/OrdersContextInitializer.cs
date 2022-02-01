using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace ProductStore_2.Models
{
    public class OrdersContextInitializer : DropCreateDatabaseIfModelChanges<OrdersContext>
    
    {
        private OrdersContext context;

        protected override void Seed(OrdersContext context)
        {
            this.context = context;

            var products = new List<Product>()
            {
                new Product() { Name = "Tomato soup", Price = 1.39M, ActualCost = .99M },
                new Product() { Name = "Hammer", Price = 16.99M, ActualCost = .99M },
                new Product() { Name = "Yo yo", Price = 6.99M, ActualCost = .99M }
            };

            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();

            var order = new Order() { Customer = "Bob" };
            context.Orders.Add(order);

            AddToOrderDetails(new OrderDetail() { Product = products[0], Quantity = 2, Order = order });
            AddToOrderDetails(new OrderDetail() { Product = products[1], Quantity = 4, Order = order });
                
            //var od = new List<OrderDetail>()
            //{
            //    new OrderDetail() { Product = products[0], Quantity = 2, Order = order},
            //    new OrderDetail() { Product = products[1], Quantity = 4, Order = order}
            //};

            //od.ForEach(o => context.OrderDetails.Add(o));

            context.SaveChanges();
        }

        private void AddToOrderDetails(OrderDetail orderDetail)
        {
            context.OrderDetails.Add(orderDetail);
        }
    }
}