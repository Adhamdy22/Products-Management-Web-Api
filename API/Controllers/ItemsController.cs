using Managers.Categories;
using Managers.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text;
using ViewModel.Categories;
using ViewModel.Items;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private ItemsManager itemsManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ItemsController(ItemsManager _maneger, IWebHostEnvironment webHostEnvironment)
        {
            itemsManager = _maneger;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Index()
        {
            var items = itemsManager.GetAll().Select(item => new Item
            {
                Id = item.Id,
                Name = item.Name,
                CategoryId = item.CategoryId,
                Price = item.Price,
                //Category=item.Category,
                //CreatedBy = item.CreatedBy,
                ImagePath= item.ImagePath,
            }).ToList();
          return new JsonResult(items);
        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult GetById(int id) {
            var item = itemsManager.GetById(id);
            if (item == null)
            {
                return NotFound("Item Not Found");
            }
            return new JsonResult(item);
        }


        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddAsync([FromForm] AddItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newitem = new Item
                {
                    Name = viewModel.Name,
                    Price=viewModel.Price,
                    CategoryId=viewModel.CategoryId,
                    //Items = viewModel.Items
                    // Add other properties if needed
                };
                string uniqueFileName = null;
                if (viewModel.ItemImage != null)
                {
                    // Set the folder path for saving the image
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/items/");

                    // Generate a unique file name to avoid overwriting existing files
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.ItemImage.FileName;

                    // Combine folder path and unique file name to form the file path
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file on the server
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.ItemImage.CopyToAsync(fileStream);
                    }

                    // Save the relative image path to the database
                    newitem.ImagePath = "/images/items/" + uniqueFileName;
                }
                var res = itemsManager.Add(newitem);
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

        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromForm] ItemViewModel item)
        {
            if (ModelState.IsValid)
            {
                var existitem = itemsManager.GetById(item.Id);

                if (existitem == null)
                {
                    return NotFound("Item Not Found");
                }

                // Check if a new image is uploaded
                if (item.ImagePath != null && item.ImagePath.Length > 0)
                {
                    // Generate a unique file name for the new image
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(item.ImagePath.FileName);
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Ensure the folder exists
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Full path to save the file
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the new image file to the specified path
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        item.ImagePath.CopyTo(fileStream);
                    }

                    // Optionally delete the old image if a new one is uploaded
                    if (!string.IsNullOrEmpty(existitem.ImagePath))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existitem.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Update the item with the new image path
                    existitem.ImagePath = "/images/items/" + uniqueFileName;  // Update path relative to wwwroot
                }

                // Update the name only if it's not null
                if (!string.IsNullOrWhiteSpace(item.Name))
                {
                    existitem.Name = item.Name;
                }

                // Update the price only if it's provided
                if (item.Price!=null)
                {
                    existitem.Price = (decimal)item.Price;
                }

                // Update the CategoryId only if it's provided
                if (item.CategoryId!=null)
                {
                    existitem.CategoryId = (int)item.CategoryId;
                }
                // map other properties as needed
                var res = itemsManager.Update(existitem);
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
                var existItem = itemsManager.GetById(id);
                if (existItem == null)
                {
                    return NotFound("Item Not Found");
                }

                // Delete the existing item
                var res = itemsManager.Delete(existItem);

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


        [HttpGet]
        [Route("Search")]
        public IActionResult Search(string? name,int ?CategoryID)
        {
            if (ModelState.IsValid)
            {
                var result = itemsManager.Search(c => c.Name.Contains(name) && c.CategoryId==CategoryID).ToList();

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
        public IActionResult GetSortedItems(string columnName = "Id", bool isAscending = true)
        {
            if (ModelState.IsValid)
            {


                var items = itemsManager.GetAll();

                // Apply sorting using the BaseManager's Sort method
                items = itemsManager.Sort(items, columnName, isAscending);

                var result = items.Select(item => new ItemViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    CategoryId = item.CategoryId
                }).ToList();

                return Ok(result);
            }

            return BadRequest("Invalid Model State");
        }

    }
}
