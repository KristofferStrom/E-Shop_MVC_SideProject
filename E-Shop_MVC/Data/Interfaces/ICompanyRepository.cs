using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Shop_MVC.Models.Data.Interfaces
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAll();
        Company GetById(int id);
        int Count();
        void Update(Company updatedCompany);
        void Add(Company newCompany);
        IEnumerable<Company> GetSearchResult(string q);
    }
}
