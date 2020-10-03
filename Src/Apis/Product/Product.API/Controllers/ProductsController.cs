using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Product.API.Services;

namespace Product.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;
        private readonly IProductServices _productServices;

        public ProductsController(ILogger<ProductsController> logger, IProductServices productServices)
        {
            _logger = logger;
            _productServices = productServices;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            try
            {
                _logger.LogInformation("GetAsync() method has been requested");

                var result = await _productServices.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error in method GetAsync(){ex.Message}");
                return StatusCode(500, "Unable to fetch product list");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            try
            {
                var result = await _productServices.GetSpecificAsync(id);

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unable to fetch product list");
            }
        }
    }
}
