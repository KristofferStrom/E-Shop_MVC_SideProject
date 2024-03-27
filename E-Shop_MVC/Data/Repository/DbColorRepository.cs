using E_Shop_MVC.Models.Data.Interfaces;

namespace E_Shop_MVC.Models.Data.Repository
{
    public class DbColorRepository : IColorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DbColorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ProductColor> GetAll()
        {
            return _dbContext.Colors.ToList();
        }

        public ProductColor GetById(int id)
        {
            return _dbContext.Colors.FirstOrDefault(dbCol => dbCol.Id == id);
        }
    }
}
