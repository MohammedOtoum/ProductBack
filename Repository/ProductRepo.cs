using ProductTask.Data;
using ProductTask.Dto;
using ProductTask.Model;

namespace ProductTask.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly DataEF _dataEF;

        public ProductRepo(IConfiguration configuration)
        {
            _dataEF = new DataEF(configuration);
        }

        public bool SaveChanges()
        {
            return _dataEF.SaveChanges() > 0;
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _dataEF.SaveChangesAsync() > 0;
        }

        public bool RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _dataEF.Remove(entityToRemove);
                return true;
            }
            return false;
        }

        public List<ProductBr> GetProducts()
        {
            List<ProductBr> products = _dataEF.products.Select(x => new ProductBr()
            {
                Id = x.Id,
                Name = x.Name,
                price = x.price,
                Description = x.Description,
                CategoryId = x.CategoryId,

            }).ToList();

            return products;
        }

        public byte[] GetImageUrlbyId(int id)
        {
            var url = _dataEF.products.Where(p => p.Id == id).Select(p => p.ImageURL).FirstOrDefault();
            if (url == null)
            {
                throw new Exception("The Image doesn't exist");
            }
            return url;
        }

        public List<ProductBr> GetpProductsbyName(string name)
        {
            var products = _dataEF.products
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .Select(p => new ProductBr()
                {
                    Id = p.Id,
                    Name = p.Name,
                    price = p.price,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                }).ToList();

            //if (products == null || products.Count == 0)
            //{
            //    //throw new Exception("No matching products found");
            //}

            return products;
        }

        public ProductBr GetProductById(int id)
        {
            ProductBr? product = _dataEF.products.Where(p => p.Id == id).Select(p => new ProductBr()
            {
                Id = p.Id,
                Name = p.Name,
                price = p.price,
                Description = p.Description,
                CategoryId = p.CategoryId,
            }).FirstOrDefault();
            return product;
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _dataEF.products.FindAsync(id);
        }



        public bool AddItem<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _dataEF.Add(entityToAdd);
                return true;
            }
            return false;
        }


        public async Task UpdateItemAsync(Product item)
        {
            _dataEF.products.Update(item);
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _dataEF.products.FindAsync(id);
            if (item != null)
            {
                _dataEF.products.Remove(item);
                //await _dataEF.SaveChangesAsync();
            }
        }


        public List<ProductBr> GetProductsSortedByPrice(string sortOrder = "asc")
        {
            IQueryable<Product> ProductQuery = _dataEF.products;

            if (sortOrder.ToLower() == "desc")
            {
                ProductQuery = ProductQuery.OrderByDescending(c => c.price);
            }
            else
            {
                ProductQuery = ProductQuery.OrderBy(c => c.price);
            }

            List<ProductBr> products = ProductQuery
                .Select(x => new ProductBr()
                {
                    Id = x.Id,
                    Name = x.Name,
                    price = x.price,
                    Description = x.Description,
                    CategoryId = x.CategoryId,
                })
                .ToList();
            return products;
        }

        public List<ProductBr> GetProductsbyCategoryId(int categoryId)
        {
            List<ProductBr>? products = _dataEF.products.Where(p => p.CategoryId == categoryId).Select(p => new ProductBr()
            {
                Id = p.Id,
                Name = p.Name,
                price = p.price,
                Description = p.Description,
                CategoryId = p.CategoryId,
            }).ToList();
            return products;
        }

        public List<ProductBr> GetProductsSortedByPriceAndTitle(string name, string sortOrder = "asc")
        {

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Title cannot be null or empty.", nameof(name));

            IQueryable<Product> productsQuery = _dataEF.products.Where(x => x.Name.ToLower().Contains(name.ToLower()));

            productsQuery = sortOrder?.ToLower() switch
            {
                "desc" => productsQuery.OrderByDescending(c => c.price),
                "asc" or null => productsQuery.OrderBy(c => c.price),
                _ => throw new ArgumentException(
                    "Invalid sort order. Use 'asc' or 'desc'.",
                    nameof(sortOrder)
                ),
            };

            return productsQuery
                .Select(x => new ProductBr
                {
                    Id = x.Id,
                    Name = x.Name,
                    price = x.price,
                    Description = x.Description,
                    CategoryId = x.CategoryId
                })
                .ToList();
        }
        public List<ProductBr> GetProductsSortedByPriceAndCategoryId(int CategoryId, string sortOrder = "asc")
        {



            IQueryable<Product> productsQuery = _dataEF.products.Where(x => x.CategoryId == CategoryId);

            productsQuery = sortOrder?.ToLower() switch
            {
                "desc" => productsQuery.OrderByDescending(c => c.price),
                "asc" or null => productsQuery.OrderBy(c => c.price),
                _ => throw new ArgumentException(
                    "Invalid sort order. Use 'asc' or 'desc'.",
                    nameof(sortOrder)
                ),
            };

            return productsQuery
                .Select(x => new ProductBr
                {
                    Id = x.Id,
                    Name = x.Name,
                    price = x.price,
                    Description = x.Description,
                    CategoryId = x.CategoryId
                })
                .ToList();
        }

        public List<ProductBr> GetProductsSortedByPriceAndCategoryIdAndName(string name, int CategoryId, string sortOrder = "asc")
        {



            IQueryable<Product> productsQuery = _dataEF.products.Where(x => x.CategoryId == CategoryId && x.Name.ToLower().Contains(name.ToLower()));

            productsQuery = sortOrder?.ToLower() switch
            {
                "desc" => productsQuery.OrderByDescending(c => c.price),
                "asc" or null => productsQuery.OrderBy(c => c.price),
                _ => throw new ArgumentException(
                    "Invalid sort order. Use 'asc' or 'desc'.",
                    nameof(sortOrder)
                ),
            };

            return productsQuery
                .Select(x => new ProductBr
                {
                    Id = x.Id,
                    Name = x.Name,
                    price = x.price,
                    Description = x.Description,
                    CategoryId = x.CategoryId
                })
                .ToList();
        }
    }
}
