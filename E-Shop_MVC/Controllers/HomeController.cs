using E_Shop_MVC.Models.Data.Interfaces;
using E_Shop_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Shop_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly int _topRatedAmount = 6;
        private readonly int _productsPerPage = 12;
        private readonly int _topCategoryAmount = 3;
        private readonly int _topCompanyAmount = 3;


        public HomeController(
            IProductRepository productRepository,
            ICompanyRepository companyRepository,
            ICategoryRepository categoryRepository,
            IReviewRepository reviewRepository)
        {
            _productRepository = productRepository;
            _companyRepository = companyRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
        }


        public IActionResult Index(int? selectedSortBy, int? selectedAmountPerPage, string q, int pageNr = 1)
        {
            var viewModel = new HomeIndexViewModel();
            List<HomeIndexViewModel.ProductItem> products;

            if (selectedSortBy != null && selectedAmountPerPage != null)
            {
                products = IndexGetSortedProducts(selectedSortBy, q);
                viewModel.SelectedSortBy = selectedSortBy.Value;
                viewModel.SelectedIncludeProductAmount = selectedAmountPerPage.Value;
            }
            else
            {
                products = IndexGetProducts(q);
                viewModel.SelectedIncludeProductAmount = 1;
            }


            var amountPerPage = (int)IndexGetProductsPerPageAmount(selectedAmountPerPage);

            viewModel.q = q;
            viewModel.Products = IndexGetProductsByPageNr(products, pageNr, amountPerPage);
            viewModel.ProductsAmount = _productRepository.GetHomeSearchResult(q).Count();
            viewModel.ProductAmountPerPage = IndexGetProductsPerPageAmount(selectedAmountPerPage);
            viewModel.TotalNumberOfPages = (int)Math.Ceiling(viewModel.ProductsAmount / viewModel.ProductAmountPerPage);
            viewModel.SelectedPageNumber = pageNr;
            viewModel.TopCategories = IndexGetTopCategories();
            viewModel.AllCategories = IndexGetAllCategories();
            viewModel.TopCompanies = IndexGetTopCompanies();
            viewModel.AllCompanies = IndexGetAllCompanies();
            viewModel.TopRatedProducts = IndexGetTopRatedProducts();
            viewModel.SortBy = IndexGetSortByListItems();
            viewModel.IncludeProductAmount = IndexGetIncludeProductAmountListItems();

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Index(HomeIndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", new { selectedSortBy = viewModel.SelectedSortBy, selectedAmountPerPage = viewModel.SelectedIncludeProductAmount, q = viewModel.q });
            }

            viewModel.Products = IndexGetProducts(viewModel.q);
            viewModel.TopCategories = IndexGetTopCategories();
            viewModel.AllCategories = IndexGetAllCategories();
            viewModel.TopCompanies = IndexGetTopCompanies();
            viewModel.AllCompanies = IndexGetAllCompanies();
            viewModel.TopRatedProducts = IndexGetTopRatedProducts();
            viewModel.ProductsAmount = _productRepository.Count();
            viewModel.ProductAmountPerPage = _productsPerPage;
            viewModel.TotalNumberOfPages = (int)Math.Ceiling(viewModel.ProductsAmount / viewModel.ProductAmountPerPage);
            viewModel.SelectedPageNumber = 1;
            viewModel.SortBy = IndexGetSortByListItems();
            viewModel.IncludeProductAmount = IndexGetIncludeProductAmountListItems();

            return View(viewModel);
        }


        private List<HomeIndexViewModel.ProductItem> IndexGetProductsByPageNr(List<HomeIndexViewModel.ProductItem> products, int pageNr, int amountPerPage)
        {
            return products.Skip((pageNr - 1) * amountPerPage).Take(amountPerPage).ToList();
        }
        private double IndexGetProductsPerPageAmount(int? selectedAmountPerPage)
        {
            if (selectedAmountPerPage == 0)
                return 4;
            if (selectedAmountPerPage == 1)
                return 8;
            if (selectedAmountPerPage == 2)
                return 12;

            return 8;
        }
        private List<HomeIndexViewModel.ProductItem> IndexGetSortedProducts(int? selectedSortBy, string q)
        {
            var products = _productRepository.GetHomeSearchResult(q).Select(prod => new HomeIndexViewModel.ProductItem
            {
                Id = prod.Id,
                Title = prod.Title,
                Price = prod.Price,
                CompanyId = prod.Company.Id,
                CompanyTitle = prod.Company.Title,
                Rate = (int)_reviewRepository.GetRateByProductId(prod.Id),
                ImgTitle = prod.imgTitle,
                IsTopRated = _productRepository.IsTopRated(prod.Id, _topRatedAmount)

            });
            switch (selectedSortBy)
            {
                case 0:
                    products = products.OrderByDescending(prod => prod.Rate);
                    break;
                case 1:
                    products = products.OrderByDescending(prod => prod.Price);
                    break;
                case 2:
                    products = products.OrderBy(prod => prod.Price);
                    break;
            }



            return products.ToList();
        }
        private List<SelectListItem> IndexGetIncludeProductAmountListItems()
        {
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "0", Text = "4" });
            selectListItems.Add(new SelectListItem { Value = "1", Text = "8" });
            selectListItems.Add(new SelectListItem { Value = "2", Text = "12" });
            return selectListItems;
        }
        private List<SelectListItem> IndexGetSortByListItems()
        {
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "0", Text = "Högst betyg" });
            selectListItems.Add(new SelectListItem { Value = "1", Text = "Pris, fallande" });
            selectListItems.Add(new SelectListItem { Value = "2", Text = "Pris, stigande" });

            return selectListItems;
        }
        private List<HomeIndexViewModel.CompanyItem> IndexGetTopCompanies()
        {
            return _companyRepository.GetAll().Select(com => new HomeIndexViewModel.CompanyItem
            {
                Id = com.Id,
                Title = com.Title,
                NumberOfProducts = _productRepository.CountByCompanyId(com.Id)
            }).OrderByDescending(com => com.NumberOfProducts).Take(_topCompanyAmount).ToList();
        }
        private List<HomeIndexViewModel.CategoryItem> IndexGetTopCategories()
        {
            return _categoryRepository.GetAll().Select(cat => new HomeIndexViewModel.CategoryItem
            {
                Id = cat.Id,
                Title = cat.Title,
                NumberOfProducts = _productRepository.CountByCategoryId(cat.Id)
            }).OrderByDescending(cat => cat.NumberOfProducts).Take(_topCategoryAmount).ToList();
        }
        private List<HomeIndexViewModel.ProductItem> IndexGetTopRatedProducts()
        {
            return _productRepository.GetAll().Select(p => new HomeIndexViewModel.ProductItem
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
        private List<HomeIndexViewModel.CompanyItem> IndexGetAllCompanies()
        {
            return _companyRepository.GetAll().Select(dbCom => new HomeIndexViewModel.CompanyItem
            {
                Id = dbCom.Id,
                Title = dbCom.Title,
                NumberOfProducts = _productRepository.CountByCompanyId(dbCom.Id)
            }).ToList();
        }
        private List<HomeIndexViewModel.CategoryItem> IndexGetAllCategories()
        {
            return _categoryRepository.GetAll().Select(dbCat => new HomeIndexViewModel.CategoryItem
            {
                Id = dbCat.Id,
                Title = dbCat.Title,
                NumberOfProducts = _productRepository.CountByCategoryId(dbCat.Id)
            }).ToList();
        }
        private List<HomeIndexViewModel.ProductItem> IndexGetProducts(string q)
        {
            return _productRepository.GetHomeSearchResult(q).Select(dbProd => new HomeIndexViewModel.ProductItem
            {
                Id = dbProd.Id,
                Title = dbProd.Title,
                Price = dbProd.Price,
                CompanyId = dbProd.Company.Id,
                CompanyTitle = dbProd.Company.Title,
                Rate = (int)_reviewRepository.GetRateByProductId(dbProd.Id),
                ImgTitle = dbProd.imgTitle!,
                IsTopRated = _productRepository.IsTopRated(dbProd.Id, _topRatedAmount)

            }).OrderByDescending(prod => prod.Rate).ToList();
        }
    }
}
