using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductTask.Dto;
using ProductTask.Model;
using ProductTask.Repository;

namespace ProductTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _productRepo;

        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet("GetProducts")]
        public List<ProductBr> GetProductsBr()
        {
            var productBrs = _productRepo.GetProducts();
            if (productBrs == null)
            {
                throw new Exception("Doesn't exist any products");
            }
            else { return productBrs; }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var item = await _productRepo.GetProductByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                Name = item.Name,
                Description = item.Description,
                price = item.price,
                CategoryId = item.CategoryId
            };

            return Ok(productDto);
        }
        [HttpGet("productById/{id}")]
        public ProductBr GetProductbyId(int id)
        {
            var item = _productRepo.GetProductById(id);
            if (item == null)
            {
                throw new Exception("this product not found");
            }

            var productDto = new ProductBr
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                price = item.price,
                CategoryId = item.CategoryId
            };
            if (productDto == null)
            {
                throw new Exception("error");
            }
            else
            {
                return productDto;
            }
        }
        [HttpGet("productByCategoryId/{id}")]
        public ActionResult<List<ProductBr>> GetProductByCategoryId(int id)
        {
            var items = _productRepo.GetProductsbyCategoryId(id);

            if (items == null || !items.Any())
            {
                return NotFound("No products found for this category.");
            }

            var productDtos = items.Select(item => new ProductBr
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                price = item.price,
                CategoryId = item.CategoryId
            }).ToList();

            return Ok(productDtos);
        }


        [HttpGet("Getproductbyname{Name}")]
        public List<ProductBr> SearchName(string Name)
        {
            List<ProductBr> products = _productRepo.GetpProductsbyName(Name);

            if (products == null)
            {
                throw new Exception("research");
            }

            return products;
        }


        [HttpGet("GetImageProduct/{id}")]
        public IActionResult GetImageItem(int id)
        {
            var url = _productRepo.GetImageUrlbyId(id);

            if (url == null)
            {
                throw new Exception("Not Found Url");
            }
            return File(url, "image/jpeg");

        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto product)
        {
            if (product.ImageURL == null || product.ImageURL.Length == 0)
            {
                return BadRequest("Product image is required.");
            }


            using var memoryStream = new MemoryStream();
            await product.ImageURL.CopyToAsync(memoryStream);

            var productEntity = new Product
            {
                Name = product.Name,
                price = product.price,
                Description = product.Description,
                ImageURL = memoryStream.ToArray(),
                CategoryId = product.CategoryId,
            };
            if (productEntity.price > 0)
            {
                _productRepo.AddItem(productEntity);
            }
            if (_productRepo.SaveChanges())
            {
                return Ok(new { message = "product added successfully" });
            }

            throw new Exception("Failed to add product");


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDto updateItemDto)
        {
            var existingItem = await _productRepo.GetProductByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }


            if (updateItemDto.ImageURL != null && updateItemDto.ImageURL.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await updateItemDto.ImageURL.CopyToAsync(memoryStream);
                existingItem.ImageURL = memoryStream.ToArray();
            }
            else
            {
                return BadRequest("Product Image is required!");
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.price = updateItemDto.price;
            existingItem.Description = updateItemDto.Description;
            existingItem.CategoryId = updateItemDto.CategoryId;


            await _productRepo.UpdateItemAsync(existingItem).ConfigureAwait(false);

            if (await _productRepo.SaveChangesAsync())
            {
                return Ok(new { message = "Product updated successfully" });
            }

            throw new Exception("Failed to update product");
        }



        [HttpDelete("DeleteProductbyId{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var item = await _productRepo.GetProductByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            await _productRepo.DeleteItemAsync(id);
            if (await _productRepo.SaveChangesAsync())
            {
                return Ok(new { message = "product deleted successfully" });
            }
            else
            {
                return NoContent();

            }
        }


        [HttpGet("GetProductsSortedByPrice")]
        public IActionResult GetProductsSortedByPrice(string sortOrder = "asc")
        {
            var products = _productRepo.GetProductsSortedByPrice(sortOrder);
            return Ok(products);
        }


        [HttpGet("GetProductsSortedByTitleandPrice")]
        public IActionResult GetProductsSortedByNameandPrice(string name, string sortOrder = "asc")
        {
            var products = _productRepo.GetProductsSortedByPriceAndTitle(name, sortOrder);
            return Ok(products);
        }
        [HttpGet("GetProductsSortedByCategoryIdandPrice")]
        public IActionResult GetProductsSortedByategoryIdandPrice(int CategoryId, string sortOrder = "asc")
        {
            var products = _productRepo.GetProductsSortedByPriceAndCategoryId(CategoryId, sortOrder);
            return Ok(products);
        }

        [HttpGet("GetProductsSortedByCategoryIdandPriceandName")]
        public IActionResult GetProductsSortedByategoryIdandPriceandName(string name, int CategoryId, string sortOrder = "asc")
        {
            var products = _productRepo.GetProductsSortedByPriceAndCategoryIdAndName(name, CategoryId, sortOrder);
            return Ok(products);
        }
    }
}
