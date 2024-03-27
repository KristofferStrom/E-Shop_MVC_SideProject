using E_Shop_MVC.Models.Data.Interfaces;

namespace E_Shop_MVC.Models.Data.Repository
{
    public class DbReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DbReviewRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(ProductReview review)
        {
            _dbContext.Reviews.Add(review);
            _dbContext.SaveChanges();
        }

        public int CountByProductId(int id)
        {
            return _dbContext.Reviews.Count(dbRev => dbRev.Product.Id == id);
        }

        public IEnumerable<ProductReview> GetByProductId(int id)
        {
            return _dbContext.Reviews.Where(dbRev => dbRev.Product.Id == id).ToList();
        }

        public double GetRateByProductId(int id)
        {
            try
            {
                var isReviewed = _dbContext.Reviews.Any(dbRev => dbRev.Product.Id == id);

                if (isReviewed)
                    return _dbContext.Reviews.Where(dbRev => dbRev.Product.Id == id).Average(dbRev => dbRev.Rate);
            }
            catch { }
            

            return 0;
        }
    }
}
