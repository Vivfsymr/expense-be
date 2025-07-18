using Microsoft.AspNetCore.Mvc;
using ExpenseBe.Core.Models;
using ExpenseBe.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseBe.API.DTOs;

namespace ExpenseBe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Category>>>> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(new ApiResponse<IEnumerable<Category>>
                {
                Success = true,
                Message = "Categories fetched successfully",
                Data = categories
            });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<IEnumerable<Category>>
                {
                    Success = false,
                    Message = e.Message,
                    Data = new List<Category>()
                });
            }
        }
    }
} 