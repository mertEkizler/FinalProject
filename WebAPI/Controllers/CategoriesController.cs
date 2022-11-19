using Business.Abstract;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger _logger;

        public CategoriesController(
            ICategoryService categoryService,
            ILogger logger)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            const string methodName = nameof(GetAll);

            _logger.Trace($"[{methodName}] Getting categories.");
            var result = _categoryService.GetAll();

            if (result.Success)
            {
                _logger.Trace($"[{methodName}] Returning results...");
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}