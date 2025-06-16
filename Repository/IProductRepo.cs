using ProductTask.Dto;
using ProductTask.Model;

namespace ProductTask.Repository
{
    public interface IProductRepo
    {
        public bool SaveChanges();
        public Task<bool> SaveChangesAsync();
        public bool RemoveEntity<T>(T _product);
        public List<ProductBr> GetProducts();
        public ProductBr GetProductById(int id);
        public Task<Product> GetProductByIdAsync(int id);
        public byte[] GetImageUrlbyId(int id);
        public List<ProductBr> GetpProductsbyName(string name);
        public bool AddItem<T>(T entityToAdd);
        public Task UpdateItemAsync(Product item);
        public Task DeleteItemAsync(int id);
        public List<ProductBr> GetProductsbyCategoryId(int categoryId);
        public List<ProductBr> GetProductsSortedByPrice(string sortOrder = "asc");
        public List<ProductBr> GetProductsSortedByPriceAndTitle(string name, string sortOrder = "asc");
        public List<ProductBr> GetProductsSortedByPriceAndCategoryId(int CategoryId, string sortOrder = "asc");
        public List<ProductBr> GetProductsSortedByPriceAndCategoryIdAndName(string name, int CategoryId, string sortOrder = "asc");



    }
}
