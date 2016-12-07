using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiNoMvc.Models;

namespace WebApiNoMvc.Classes
{
    public static class DataContainer
    {
        public static Dictionary<int, Product> Products = new Dictionary<int, Product>();

        static DataContainer()
        {
            Products.Add(1, new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 });
            Products.Add(2, new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M });
            Products.Add(3, new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M });
        }
    }
}