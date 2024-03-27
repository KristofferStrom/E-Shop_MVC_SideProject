using E_Shop_MVC.Models.Data.Interfaces;

namespace E_Shop_MVC.Models.Data.Repository
{
    public class DbCompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DbCompanyRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Company newCompany)
        {
            _dbContext.Companies.Add(newCompany);
            _dbContext.SaveChanges();
        }

        public int Count()
        {
            return _dbContext.Companies.Count();
        }

        public IEnumerable<Company> GetAll()
        {
            return _dbContext.Companies.ToList();
        }

        public Company GetById(int id)
        {
            return _dbContext.Companies.FirstOrDefault(dbCom => dbCom.Id == id);
        }

        public IEnumerable<Company> GetSearchResult(string q)
        {
            return _dbContext.Companies.Where(dbCom => q == null || dbCom.Title.Contains(q)).ToList();
        }

        public void Update(Company updatedCompany)
        {
            var company = _dbContext.Companies.FirstOrDefault(dbCom => dbCom.Id == updatedCompany.Id);
            if (company != null)
            {
                company.Id = updatedCompany.Id;
                company.Title = updatedCompany.Title;

                _dbContext.SaveChanges();
            }
        }
    }
}
