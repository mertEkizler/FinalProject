using Business.Abstract;
using Castle.Core.Logging;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger _logger;

        public ProductsController(
            IProductService productService,
            ILogger logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            const string methodName = nameof(GetAll); 

            Thread.Sleep(500);

            _logger.Trace($"[{methodName}] Getting products.");
            var result = _productService.GetAll();

            if (result.Success)
            {
                _logger.Trace($"[{methodName}] Returning results...");
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            const string methodName = nameof(GetById);

            _logger.Trace($"[{methodName}] Getting product with product ID: '{id}'.");
            var result = _productService.GetById(id);

            if (result.Success)
            {
                _logger.Trace($"[{methodName}] Returning result...");
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbycategory")]
        public IActionResult GetByCategory(int categoryId)
        {
            const string methodName = nameof(GetByCategory);

            _logger.Trace($"[{methodName}] Getting products with category ID: '{categoryId}'.");
            var result = _productService.GetAllByCategoryId(categoryId);

            if (result.Success)
            {
                _logger.Trace($"[{methodName}] Returning results...");
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            const string methodName = nameof(Add);

            _logger.Trace($"[{methodName}] Adding product {{@product}}.");
            var result = _productService.Add(product);

            if (result.Success)
            {
                _logger.Trace($"[{methodName}] Returning result...");
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}