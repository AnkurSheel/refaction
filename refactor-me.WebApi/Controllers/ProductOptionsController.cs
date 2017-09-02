﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Refactor_me.Models;
using Refactor_me.Services.Interfaces;

namespace Refactor_me.WebApi.Controllers
{
    [RoutePrefix("products/{productId}/options")]
    public class ProductOptionsController : ApiController
    {
        private readonly IProductOptionsService _productOptionsService;

        public ProductOptionsController(IProductOptionsService productOptionsService)
        {
            _productOptionsService = productOptionsService;
        }

        [Route]
        [HttpPost]
        public void Create(Guid productId, ProductOption newOption)
        {
            _productOptionsService.AddNewOption(productId, newOption);
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            _productOptionsService.RemoveOption(id);
        }

        [Route("{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = _productOptionsService.GetOption(id);
            if (option == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return option;
        }

        [Route]
        [HttpGet]
        public IEnumerable<ProductOption> GetOptions(Guid productId)
        {
            return _productOptionsService.GetOptions(productId);
        }

        [Route("{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption updatedOption)
        {
            _productOptionsService.UpdateOption(id, updatedOption);
        }
    }
}
