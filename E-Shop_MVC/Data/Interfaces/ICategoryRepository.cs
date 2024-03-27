using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Shop_MVC.Models.Data.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<ProductCategory> GetAll();
        ProductCategory GetById(int id);
        ProductCategory GetByProductId(int id);
        ProductCategory GetBySubCategoryId(int id);
        void Update(ProductCategory updatedCategory);
        void Add(ProductCategory newCategory);
        IEnumerable<ProductCategory> GetSearchResult(string q);
        int Count();
    }
}
