using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.OData;
using AngularWebApi.Models;

namespace AngularWebApi.Controllers
{
    [EnableCors("http://localhost:43693", "*", "*")]
    public class ProductsController : ApiController
    {
        // GET: api/Products
        [EnableQuery]
        [ResponseType(typeof(Product))]
        public IHttpActionResult Get()
        {
            try
            {

                ProductRepository productRepository = new ProductRepository();
                return Ok(productRepository.Retrieve().AsQueryable());

            }
            catch (Exception exception)
            {

                return InternalServerError(exception);
            }
        }


        // GET: api/Products/5
        [Authorize]
        [ResponseType(typeof(Product))]
        public IHttpActionResult Get(int id)
        {
            Product product;
            ProductRepository productRepository = new ProductRepository();

            if (id > 0)
            {
                var products = productRepository.Retrieve();
                product = products.FirstOrDefault(p => p.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }
            }
            else
            {
                product = productRepository.Create();
            }

            return Ok(product);
        }

        // POST: api/Products
        public IHttpActionResult Post([FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productRepository = new Models.ProductRepository();
            var newProduct = productRepository.Save(product);
            if (newProduct == null)
            {
                return Conflict();
            }

            return Created<Product>(Request.RequestUri + newProduct.ProductId.ToString(), newProduct);
        }

        // PUT: api/Products/5
        public IHttpActionResult Put(int id, [FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productRepository = new Models.ProductRepository();
            var updateProduct = productRepository.Save(id, product);

            if (updateProduct == null)
            {
                return NotFound();
            }

            return Ok();
        }

        // DELETE: api/Products/5
        public void Delete(int id)
        {
        }
    }
}
