
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
        public IEnumerable<Product> GetAllProducts()
        {
            var list = new List<Product>();
            lock (typeof(DataContainer))
            {
                foreach (var kvp in DataContainer.Products)
                {
                    list.Add(kvp.Value);
                }
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
            bool wasProductFound = false;

            lock (typeof(DataContainer))
            {
                wasProductFound = DataContainer.Products.TryGetValue(id, out product);
            }
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

            bool didAlreadyExist = true;
            int newProductCount = 0;
            lock (typeof(DataContainer))
            {
                if (!DataContainer.Products.ContainsKey(newProduct.Id))
                {
                    didAlreadyExist = false;
                    newProduct.Id = DataContainer.Products.Count + 1;
                    DataContainer.Products.Add(newProduct.Id, newProduct);
                    newProductCount = DataContainer.Products.Count;
                }
            }
            if(!didAlreadyExist)
            {
                return Ok(newProductCount);
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
            int newProductCount = 0;
            bool wasProductFound = false;
            //var list = new List<Product>();
            lock (typeof(DataContainer))
            { 
                foreach (var kvp in DataContainer.Products)
                {
                    if (kvp.Value.Name == productName)
                    {
                        wasProductFound = true;
                        IdToDelete = kvp.Value.Id;
                        DataContainer.Products.Remove(IdToDelete);
                        newProductCount = DataContainer.Products.Count;
                        break;
                    }
                }
            }
            if (wasProductFound)
            {
                return Ok(newProductCount);
            }
            else
            {
                return NotFound();
            }
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
