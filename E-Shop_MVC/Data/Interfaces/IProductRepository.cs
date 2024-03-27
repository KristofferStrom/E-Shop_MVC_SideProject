using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Shop_MVC.Models.Data.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Add(Product product);
        int CountByCategoryId(int id);
        int CountByCompanyId(int id);
        int CountBySubCategoryId(int id);
        bool IsTopRated(int id, int amount);
        IEnumerable<Product> GetByCategoryId(int id);
        IEnumerable<Product> GetBySubCategoryId(int id);
        bool InStockById(int id);
        IEnumerable<Product> GetRelatedProductsById(int id);
        int Count();
        void Update(Product updatedProduct);
        IEnumerable<Product> GetSearchResult(string q);
        IEnumerable<Product> GetHomeSearchResult(string q);
        IEnumerable<Product> GetByCompanyId(int id);
    }
}
