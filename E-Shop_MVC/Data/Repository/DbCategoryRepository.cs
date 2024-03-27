using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Shop_MVC.Models.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Shop_MVC.Models.Data.Repository
{
    public class DbCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DbCategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(ProductCategory newCategory)
        {
            _dbContext.Categories.Add(newCategory);
            _dbContext.SaveChanges();
        }

        public int Count()
        {
            return _dbContext.Categories.Count();
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            return _dbContext.Categories.ToList();
        }

        public ProductCategory GetById(int id)
        {
            return _dbContext.Categories.FirstOrDefault(dbCat => dbCat.Id == id);
        }

        public ProductCategory GetByProductId(int id)
        {
            return _dbContext.Categories.FirstOrDefault(dbCat =>
                dbCat.Id == _dbContext.Products.FirstOrDefault(dbProd => dbProd.Id == id).SubCategory.Category.Id);
        }

        public ProductCategory GetBySubCategoryId(int id)
        {
            return _dbContext.Categories.FirstOrDefault(dbCat =>
                dbCat.Id == _dbContext.SubCategories.FirstOrDefault(dbSubCat => dbSubCat.Id == id).Category.Id);
        }

        public IEnumerable<ProductCategory> GetSearchResult(string q)
        {
            return _dbContext.Categories.Where(dbCat => q == null || dbCat.Title.Contains(q)).ToList();
        }

        public void Update(ProductCategory updatedCategory)
        {
            var category = _dbContext.Categories.FirstOrDefault(dbCat => dbCat.Id == updatedCategory.Id);
            if (category != null)
            {
                category.Id = updatedCategory.Id;
                category.Title = updatedCategory.Title;
            }

            _dbContext.SaveChanges();
        }
    }
}
