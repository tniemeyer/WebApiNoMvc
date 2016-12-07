
using WebApiNoMvc.Models;
using WebApiNoMvc.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiNoMvc.Controllers
{
    public class ProductsController : ApiController
    {
        //Product[] products = new Product[]
        //List<Product> products = new List<Product>
        //{
        //    new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
        //    new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
        //    new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        //};

        public IEnumerable<Product> GetAllProducts()
        {
            var list = new List<Product>();
            foreach (var kvp in DataContainer.Products)
            {
                list.Add(kvp.Value);
            }

            return list;
        }

        public IHttpActionResult GetProduct(int id, string token)
        {
            if(!ValidateToken(token))
            {
                return Unauthorized();
            }

            Product product;
            var wasProductFound = DataContainer.Products.TryGetValue(id, out product);
            if (!wasProductFound)
            {
                return NotFound();
                //return this.StatusCode(HttpStatusCode.Forbidden);
            }
            return Ok(product);
        }

        public IHttpActionResult PostProduct(Product newProduct, string token)
        {
            if (!ValidateToken(token))
            {
                return Unauthorized();
            }

            if (!DataContainer.Products.ContainsKey(newProduct.Id))
            {
                newProduct.Id = DataContainer.Products.Count + 1;
                DataContainer.Products.Add(newProduct.Id, newProduct);
                return Ok(DataContainer.Products.Count);
            }
            else
            {
                return this.Conflict();
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteProduct([FromUri]string productName, [FromUri]string token)
        {
            if (!ValidateToken(token))
            {
                return Unauthorized();
            }

            int IdToDelete;
            var list = new List<Product>();
            foreach (var kvp in DataContainer.Products)
            {
                if(kvp.Value.Name == productName)
                {
                    IdToDelete = kvp.Value.Id;
                    DataContainer.Products.Remove(IdToDelete);

                    return Ok(DataContainer.Products.Count);
                }
            }
            return NotFound();
        }

        private bool ValidateToken(string token)
        {
            foreach(var kvp in AuthenticationContainer.activeTokens)
            {
                if(kvp.Value.Value == token)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
