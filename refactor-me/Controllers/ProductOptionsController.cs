using System;
using System.Net;
using System.Web.Http;
using Refactor_me.Models;

namespace Refactor_me.Controllers
{
    [RoutePrefix("products/{productId}/options")]
    public class ProductOptionsController : ApiController
    {
        [Route]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        [Route("{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }

        [Route("{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);
            if (option.IsNew)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return option;
        }

        [Route]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            return new ProductOptions(productId);
        }

        [Route("{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption updatedOption)
        {
            var orig = new ProductOption(id) { Name = updatedOption.Name, Description = updatedOption.Description };

            if (!orig.IsNew)
            {
                orig.Save();
            }
        }
    }
}
