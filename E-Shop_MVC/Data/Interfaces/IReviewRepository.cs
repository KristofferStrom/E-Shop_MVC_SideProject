using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Shop_MVC.Models.Data.Interfaces
{
    public interface IReviewRepository
    {
        IEnumerable<ProductReview> GetByProductId(int id);
        int CountByProductId(int id);
        void Add(ProductReview review);
        double GetRateByProductId(int id);
    }
}
