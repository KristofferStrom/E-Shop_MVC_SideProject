using E_Shop_MVC.Models.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Shop_MVC.Models.Data.Repository
{
    public class DbSubCategoryRepository : ISubCategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DbSubCategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(SubProductCategory newSubCategory)
        {
            _dbContext.SubCategories.Add(newSubCategory);
            _dbContext.SaveChanges();
        }

        public int Count()
        {
            return _dbContext.SubCategories.Count();
        }

        public IEnumerable<SubProductCategory> GetAll()
        {
            return _dbContext.SubCategories.Include(dbSubCat => dbSubCat.Category).ToList();
        }

        public IEnumerable<SubProductCategory> GetByCategoryId(int id)
        {
            return _dbContext.SubCategories.Where(dbSubCat => dbSubCat.Category.Id == id).ToList();
        }

        public SubProductCategory GetById(int id)
        {
            return _dbContext.SubCategories.Include(dbSubCat => dbSubCat.Category)
                .FirstOrDefault(dbSubCat => dbSubCat.Id == id);
        }

        public SubProductCategory GetByProductId(int id)
        {
            return _dbContext.SubCategories.FirstOrDefault(dbSub =>
                dbSub.Id == _dbContext.Products.FirstOrDefault(dbProd => dbProd.Id == id).SubCategory.Id);
        }

        public IEnumerable<SubProductCategory> GetSearchResult(string q)
        {
            return _dbContext.SubCategories.Include(dbSubCat => dbSubCat.Category)
                .Where(dbSubCat => q == null || dbSubCat.Title.Contains(q)).ToList();
        }

        public void Update(SubProductCategory updatedSubCategory)
        {
            var subCategory = _dbContext.SubCategories.FirstOrDefault(dbSubCat => dbSubCat.Id == updatedSubCategory.Id);
            if (subCategory != null)
            {
                subCategory.Id = updatedSubCategory.Id;
                subCategory.Title = updatedSubCategory.Title;
                subCategory.Category = updatedSubCategory.Category;

                _dbContext.SaveChanges();
            }
        }
    }
}
