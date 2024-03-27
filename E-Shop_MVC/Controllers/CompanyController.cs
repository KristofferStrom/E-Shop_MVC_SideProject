using E_Shop_MVC.Models.Data;
using E_Shop_MVC.Models.Data.Interfaces;
using E_Shop_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Shop_MVC.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IProductRepository _productRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly int _companiesPerPage = 10;
        private readonly int _topRatedAmount = 6;
        private readonly int _topCategoryAmount = 5;
        private readonly int _topCompanyAmount = 5;

        public CompanyController(ICompanyRepository companyRepository, IProductRepository productRepository, IReviewRepository reviewRepository, ICategoryRepository categoryRepository)
        {
            _companyRepository = companyRepository;
            _productRepository = productRepository;
            _reviewRepository = reviewRepository;
            _categoryRepository = categoryRepository;
        }


        [Authorize]
        public IActionResult Index(string q, bool isDesc, string col, int pageNr = 1)
        {
            var viewModel = new CompanyIndexViewModel();

            var companySearchResult = _companyRepository.GetSearchResult(q).ToList();
            var sortedCompanies = IndexGetSortedCompanyList(companySearchResult, isDesc, col, ref viewModel);

            viewModel.Companies = IndexGetCompaniesByPageNr(pageNr, sortedCompanies);
            viewModel.TotalCompanyAmount = companySearchResult.Count;
            viewModel.CompanyAmountPerPage = _companiesPerPage;
            viewModel.TotalNumberOfPages = (int)Math.Ceiling(viewModel.TotalCompanyAmount / viewModel.CompanyAmountPerPage);
            viewModel.SelectedPageNumber = pageNr;
            viewModel.q = q;

            return View(viewModel);
        }


        [Authorize]
        public IActionResult Edit(int id)
        {
            var viewModel = new CompanyEditViewModel();

            var companyToEdit = _companyRepository.GetById(id);

            viewModel.Id = companyToEdit.Id;
            viewModel.Title = companyToEdit.Title;

            return View(viewModel);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Edit(CompanyEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var updatedCompany = CreateCompany(viewModel);
                _companyRepository.Update(updatedCompany);

                return RedirectToAction("Index");
            }

            var companyToEdit = _companyRepository.GetById(viewModel.Id);

            viewModel.Id = companyToEdit.Id;
            viewModel.Title = companyToEdit.Title;

            return View(viewModel);
        }


        [Authorize]
        public IActionResult New()
        {
            var viewModel = new CompanyNewViewModel();

            return View(viewModel);
        }


        [Authorize]
        [HttpPost]
        public IActionResult New(CompanyNewViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newCompany = CreateCompany(viewModel);
                _companyRepository.Add(newCompany);

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }


        public IActionResult DisplayProducts(int id)
        {
            var viewModel = new CompanyDisplayProductsViewModel();

            viewModel.Products = DisplayProductsGetProductsByCompanyId(id);
            viewModel.TopCategories = DisplayProductsGetTopCategories();
            viewModel.AllCategories = DisplayProductsGetAllCategories();
            viewModel.TopCompanies = DisplayProductsGetTopCompanies();
            viewModel.AllCompanies = DisplayProductsGetAllCompanies();
            viewModel.TopRatedProducts = DisplayProductsGetTopRatedProducts();
            viewModel.CompanyName = _companyRepository.GetById(id).Title;

            return View(viewModel);
        }


        private List<CompanyDisplayProductsViewModel.ProductItem> DisplayProductsGetTopRatedProducts()
        {
            return _productRepository.GetAll().Select(p => new CompanyDisplayProductsViewModel.ProductItem
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                Rate = (int)_reviewRepository.GetRateByProductId(p.Id),
                CompanyId = p.Company.Id,
                CompanyTitle = p.Company.Title,
                ImgTitle = p.imgTitle
            }).OrderByDescending(prod => prod.Rate).Take(_topRatedAmount).ToList();
        }
        private List<CompanyDisplayProductsViewModel.CompanyItem> DisplayProductsGetAllCompanies()
        {
            return _companyRepository.GetAll().Select(dbCom => new CompanyDisplayProductsViewModel.CompanyItem
            {
                Id = dbCom.Id,
                Title = dbCom.Title,
                NumberOfProducts = _productRepository.CountByCompanyId(dbCom.Id)
            }).ToList();
        }
        private List<CompanyDisplayProductsViewModel.CompanyItem> DisplayProductsGetTopCompanies()
        {
            return _companyRepository.GetAll().Select(com => new CompanyDisplayProductsViewModel.CompanyItem
            {
                Id = com.Id,
                Title = com.Title,
                NumberOfProducts = _productRepository.CountByCompanyId(com.Id)
            }).OrderByDescending(com => com.NumberOfProducts).Take(_topCompanyAmount).ToList();
        }
        private List<CompanyDisplayProductsViewModel.CategoryItem> DisplayProductsGetAllCategories()
        {
            return _categoryRepository.GetAll().Select(dbCat => new CompanyDisplayProductsViewModel.CategoryItem
            {
                Id = dbCat.Id,
                Title = dbCat.Title,
                NumberOfProducts = _productRepository.CountByCategoryId(dbCat.Id)
            }).ToList();
        }
        private List<CompanyDisplayProductsViewModel.CategoryItem> DisplayProductsGetTopCategories()
        {
            return _categoryRepository.GetAll().Select(cat => new CompanyDisplayProductsViewModel.CategoryItem
            {
                Id = cat.Id,
                Title = cat.Title,
                NumberOfProducts = _productRepository.CountByCategoryId(cat.Id)
            }).OrderByDescending(cat => cat.NumberOfProducts).Take(_topCategoryAmount).ToList();
        }
        private List<CompanyDisplayProductsViewModel.ProductItem> DisplayProductsGetProductsByCompanyId(int id)
        {
            return _productRepository.GetByCompanyId(id).Select(prod =>
               new CompanyDisplayProductsViewModel.ProductItem
               {
                   Id = prod.Id,
                   Title = prod.Title,
                   CompanyId = prod.Company.Id,
                   CompanyTitle = prod.Company.Title,
                   ImgTitle = prod.imgTitle,
                   IsTopRated = _productRepository.IsTopRated(prod.Id, _topRatedAmount),
                   Price = prod.Price,
                   Rate = (int)_reviewRepository.GetRateByProductId(prod.Id)
               }).ToList();
        }
        private List<CompanyIndexViewModel.CompanyItem> IndexGetCompaniesByPageNr(int pageNr, IEnumerable<Company> sortedCompanies)
        {
            return sortedCompanies.Skip((pageNr - 1) * _companiesPerPage).Take(_companiesPerPage).Select(com =>
                new CompanyIndexViewModel.CompanyItem
                {
                    Id = com.Id,
                    Title = com.Title

                }).ToList();
        }
        private List<Company> IndexGetSortedCompanyList(IEnumerable<Company> companySearchResult, bool isDesc, string col, ref CompanyIndexViewModel viewModel)
        {
            viewModel.IsDescTitle = false;
            viewModel.IsDescId = false;
            viewModel.SelectedColumn = col;
            viewModel.IsDesc = isDesc;
            if (isDesc && col == "title")
            {
                companySearchResult = companySearchResult.OrderByDescending(com => com.Title);

            }
            else if (!isDesc && col == "title")
            {
                companySearchResult = companySearchResult.OrderBy(com => com.Title);
                viewModel.IsDescTitle = true;
            }
            else if (isDesc && col == "id")
            {
                companySearchResult = companySearchResult.OrderByDescending(com => com.Id);
            }
            else if (!isDesc && col == "id")
            {
                companySearchResult = companySearchResult.OrderBy(com => com.Id);
                viewModel.IsDescId = true;
            }

            return companySearchResult.ToList();
        }
        private Company CreateCompany(CompanyEditViewModel viewModel)
        {
            return new Company
            {
                Id = viewModel.Id,
                Title = viewModel.Title
            };
        }
        private Company CreateCompany(CompanyNewViewModel viewModel)
        {
            return new Company
            {
                Title = viewModel.Title
            };
        }
    }
}
