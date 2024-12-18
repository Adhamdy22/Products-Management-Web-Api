using Managers.Categories;
using Managers.Items;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using ViewModel.Categories;
using ViewModel.Items;
using ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [Authorize]
    public class CategoryController : ControllerBase
    {
        private CategoryManager categoryManager;
        public CategoryController(CategoryManager _maneger)
        {
            categoryManager = _maneger;
        }

        [HttpGet]
        
        [Route("GetAll")]
        
        public IActionResult Index()
        {
            var categories = categoryManager.GetAll().Select(category => new Category
            {
                Id = category.Id,
                Name = category.Name,
                Items = category.Items.Select(item => new Item
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price= item.Price,
                    CategoryId = item.CategoryId,
                    CreatedDate = item.CreatedDate,
                    ImagePath=item.ImagePath,
                }).ToList()


            }).ToList();
            return new JsonResult(categories);
        }

        [HttpGet]
        [Route("Getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            var FilteredCategory = categoryManager.GetById(id);
            if (FilteredCategory == null)
            {
                return NotFound("Category Not Found");
            }
            return new JsonResult(FilteredCategory);
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromForm]AddCategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newCategory = new Category
                {
                    Name = viewModel.Name,
                    //Items = viewModel.Items
                    // Add other properties if needed
                };
                var res = categoryManager.Add(newCategory);
                if (res)
                {
                    return Ok("Added Successfully");
                }
                else
                {

                    return BadRequest("Sorry Failed To Add");
                }

            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                //foreach (var item in ModelState.V)
                //{
                //    stringBuilder.Append(item.Description);
                //    stringBuilder.Append(", ");
                //}
                return new JsonResult(stringBuilder.ToString());
            }

        }


        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                // Check if the id is valid
                if (id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                // Get the category by id
                var existItem = categoryManager.GetById(id);
                if (existItem == null)
                {
                    return NotFound("Category Not Found");
                }

                // Delete the existing item
                var res = categoryManager.Delete(existItem);

                if (res)
                {
                    return Ok("Deleted Successfully");
                }
                else
                {
                    return BadRequest("Sorry, Failed to Delete");
                }
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                //foreach (var item in ModelState.V)
                //{
                //    stringBuilder.Append(item.Description);
                //    stringBuilder.Append(", ");
                //}
                return new JsonResult(stringBuilder.ToString());
            }

        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromForm] CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                var existitem = categoryManager.GetById(category.Id);

                if (existitem == null)
                {
                    return NotFound("Category Not Found");
                }
                existitem.Name = category.Name;
                // map other properties as needed
                var res = categoryManager.Update(existitem);
                if (res)
                {
                    return Ok("Updated Successfully");
                }
                else
                {
                    return BadRequest("Sorry, Failed to Update");
                }

            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                //foreach (var item in ModelState.V)
                //{
                //    stringBuilder.Append(item.Description);
                //    stringBuilder.Append(", ");
                //}
                return new JsonResult(stringBuilder.ToString());
            }
        }

        [HttpGet]
        [Route("Search")]
        public IActionResult SearchByName(string name)
        {
            if (ModelState.IsValid)
            {
                var result = categoryManager.Search(c => c.Name.Contains(name)).ToList();

                if (result == null)
                {
                    return NotFound("Category Not Found");
                }
                else
                {
                    return Ok(result);
                }
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                //foreach (var item in ModelState.V)
                //{
                //    stringBuilder.Append(item.Description);
                //    stringBuilder.Append(", ");
                //}
                return new JsonResult(stringBuilder.ToString());
            }
        }


        [HttpGet]
        [Route("Sort")]
        public IActionResult GetSortedCategories(string columnname="Id",bool IsAscending = true) {
            if (ModelState.IsValid)
            {
                var categories = categoryManager.GetAll();

                categories=categoryManager.Sort(categories,columnname,IsAscending);

                var result = categories.Select(item => new CategoryViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    
                }).ToList();

                return Ok(result);
            }

            return BadRequest("Invalid Model State");
        }
    }
}

