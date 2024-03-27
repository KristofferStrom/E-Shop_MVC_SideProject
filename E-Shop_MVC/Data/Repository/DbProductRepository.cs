using E_Shop_MVC.Models.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Shop_MVC.Models.Data.Repository
{
    public class DbProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IReviewRepository _reviewRepository;

        public DbProductRepository(ApplicationDbContext dbContext, IReviewRepository reviewRepository)
        {
            _dbContext = dbContext;
            _reviewRepository = reviewRepository;
        }

        public int CountByCategoryId(int id)
        {
            return _dbContext.Products.Count(dbProd => dbProd.SubCategory.Category.Id == id);
        }

        public bool IsTopRated(int id, int amount)
        {
            try
            {
                return _dbContext.Products.Select(p => new ProductItem
                {
                    Id = p.Id,
                    Rate = _reviewRepository.GetRateByProductId(p.Id)
                }).ToList().OrderByDescending(prod => prod.Rate).Take(amount).Any(p => p.Id == id);
            }
            catch { }

            return false;
           
        }

        public IEnumerable<Product> GetAll()
        {
            return _dbContext.Products
                .Include(dbProd => dbProd.Company)
                .Include(dbProd => dbProd.Color)
                .Include(dbProd => dbProd.SubCategory).ToList();
        }

        public Product GetById(int id)
        {
            return _dbContext.Products.Include(p => p.Company).Include(p => p.Color).Include(dbProd => dbProd.SubCategory).FirstOrDefault(dbProd => dbProd.Id == id);
        }

        public int CountByCompanyId(int id)
        {
            return _dbContext.Products.Count(dbProd => dbProd.Company.Id == id);
        }

        public IEnumerable<Product> GetByCategoryId(int id)
        {
            return _dbContext.Products.Include(dbProd => dbProd.Company).Where(dbProd => dbProd.SubCategory.Category.Id == id).ToList();
        }

        public IEnumerable<Product> GetBySubCategoryId(int id)
        {
            return _dbContext.Products.Include(dbProd => dbProd.Company).Where(dbProd => dbProd.SubCategory.Id == id).ToList();
        }

        public int CountBySubCategoryId(int id)
        {
            return _dbContext.Products.Count(dbProd => dbProd.SubCategory.Id == id);
        }

        public bool InStockById(int id)
        {
            return _dbContext.Products.Where(dbProd => dbProd.Id == id).Any(dbProd => dbProd.InStock > 0);
        }

        public IEnumerable<Product> GetRelatedProductsById(int id)
        {

            return _dbContext.Products.Include(p => p.Company)
                                        .Where(dbProd => dbProd.SubCategory.Id ==
                                        _dbContext.SubCategories.FirstOrDefault(dbSub => dbSub.Id ==
                                        _dbContext.Products.FirstOrDefault(prod => prod.Id == id)
                                        .SubCategory.Id).Id)
                                        .Where(dbProd => dbProd.Id != id).ToList();
        }

        public void Add(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }

        public int Count()
        {
            return _dbContext.Products.Count();
        }

        public void Update(Product updatedProduct)
        {
            var product = _dbContext.Products.FirstOrDefault(dbProd => dbProd.Id == updatedProduct.Id);
            if (product != null)
            {
                product.Id = updatedProduct.Id;
                product.Title = updatedProduct.Title;
                product.Price = updatedProduct.Price;
                product.Company = updatedProduct.Company;
                product.SubCategory = updatedProduct.SubCategory;
                product.Color = updatedProduct.Color;
                product.InStock = updatedProduct.InStock;
                product.ShortDescription = updatedProduct.ShortDescription;
                product.LongDescription = updatedProduct.LongDescription;
                product.Warranty = updatedProduct.Warranty;

                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<Product> GetSearchResult(string q)
        {
            return _dbContext.Products.Include(dbProd => dbProd.Company).Include(dbProd => dbProd.SubCategory).Where(dbProd => q == null || dbProd.Title.Contains(q)).ToList();
        }

        public IEnumerable<Product> GetByCompanyId(int id)
        {
            return _dbContext.Products.Include(dbProd => dbProd.Company).Include(dbProd => dbProd.SubCategory).Where(dbProd => dbProd.Company.Id == id).ToList();
        }

        public IEnumerable<Product> GetHomeSearchResult(string q)
        {
            return _dbContext.Products.Include(dbProd => dbProd.Company).Include(dbProd => dbProd.SubCategory).Where(dbProd => q == null || dbProd.Title.Contains(q) || dbProd.LongDescription.Contains(q)).ToList();
        }

        public class ProductItem
        {
            public int Id { get; set; }
            public double Rate { get; set; }
        }
    }
}
