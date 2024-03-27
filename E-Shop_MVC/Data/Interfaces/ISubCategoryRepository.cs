using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Shop_MVC.Models.Data.Interfaces
{
    public interface ISubCategoryRepository
    {
        IEnumerable<SubProductCategory> GetAll();
        SubProductCategory GetById(int id);
        IEnumerable<SubProductCategory> GetByCategoryId(int id);
        SubProductCategory GetByProductId(int id);
        int Count();
        void Update(SubProductCategory updatedSubCategory);
        void Add(SubProductCategory newSubCategory);
        IEnumerable<SubProductCategory> GetSearchResult(string q);
    }
}
