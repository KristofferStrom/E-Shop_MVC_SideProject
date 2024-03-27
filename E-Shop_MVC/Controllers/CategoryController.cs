using E_Shop_MVC.Models.Data;
using E_Shop_MVC.Models.Data.Interfaces;
using E_Shop_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Shop_MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly int _categoriesPerPage = 10;
        private readonly int _topRatedAmount = 6;

        public CategoryController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ISubCategoryRepository subCategoryRepository,
            IReviewRepository reviewRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _reviewRepository = reviewRepository;
        }


        [Authorize]
        public IActionResult Index(string q, bool isDesc, string col, int pageNr = 1)
        {
            var viewModel = new CategoryIndexViewModel();

            var categorySearchResult = _categoryRepository.GetSearchResult(q).ToList();
            var sortedCategories = IndexGetSortedCategoryList(categorySearchResult, isDesc, col, ref viewModel);
            viewModel.Categories = IndexGetCategoriesByPageNr(pageNr, sortedCategories);

            viewModel.TotalCategoryAmount = categorySearchResult.Count;
            viewModel.CategoryAmountPerPage = _categoriesPerPage;
            viewModel.TotalNumberOfPages = (int)Math.Ceiling(viewModel.TotalCategoryAmount / viewModel.CategoryAmountPerPage);
            viewModel.SelectedPageNumber = pageNr;
            viewModel.q = q;

            return View(viewModel);
        }


        [Authorize]
        public IActionResult Edit(int id)
        {
            var viewModel = new CategoryEditViewModel();

            var category = _categoryRepository.GetById(id);
            viewModel.Id = category.Id;
            viewModel.Title = category.Title;

            return View(viewModel);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var updatedCategory = CreateCategory(viewModel);
                _categoryRepository.Update(updatedCategory);

                return RedirectToAction("Index");
            }

            var category = _categoryRepository.GetById(viewModel.Id);
            viewModel.Id = category.Id;
            viewModel.Title = category.Title;

            return View(viewModel);
        }
        public IActionResult DisplayProducts(int catId, int subCatId)
        {
            var viewModel = new CategoryDisplayProductsViewModel();

            viewModel.Category = DisplayProductsGetCategoryById(catId);
            viewModel.TopRatedProducts = DisplayProductsGetTopRatedProducts();

            if (subCatId == 0)
            {
                viewModel.Products = DisplayProductsGetProductsByCategoryId(catId);
                viewModel.Companies = DisplayProductsGetCompaniesByCategoryId(catId);
                viewModel.SubCategories = DisplayProductsGetSubCategoriesByCategoryId(catId);
            }
            else
            {
                viewModel.Products = DisplayProductsGetProductsBySubCategoryId(subCatId);
                viewModel.Companies = DisplayProductsGetCompaniesBySubCategoryId(subCatId);
                viewModel.SubCategory = DisplayProductsGetSubCategoryById(subCatId);
                viewModel.SubCategories = DisplayProductsGetSubCategoriesByCategoryId(catId, subCatId);
            }

            return View(viewModel);
        }


        [Authorize]
        public IActionResult New()
        {
            var viewModel = new CategoryNewViewModel();

            return View(viewModel);
        }


        [Authorize]
        [HttpPost]
        public IActionResult New(CategoryNewViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newCategory = CreateCategory(viewModel);
                _categoryRepository.Add(newCategory);

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
        public IActionResult _SelectSubCategory(int id)
        {
            var viewModel = new CategorySelectSubCategoryViewModel();

            viewModel.Products = _SelectSubCategoryGetProductsBySubCategoryId(id);

            return View(viewModel);
        }

        public IActionResult _ShowTree(int id)
        {
            var viewModel = new CategoryShowTreeViewModel();

            viewModel.Category = _ShowTreeGetCategoryBySubCategoryId(id);
            viewModel.SubCategory = _ShowTreeGetSubCategoryById(id);

            return View(viewModel);
        }

        private List<CategoryIndexViewModel.CategoryItem> IndexGetCategoriesByPageNr(int pageNr, IEnumerable<ProductCategory> sortedCategories)
        {
            return sortedCategories.Skip((pageNr - 1) * _categoriesPerPage).Take(_categoriesPerPage).Select(cat =>
                new CategoryIndexViewModel.CategoryItem
                {
                    Id = cat.Id,
                    Title = cat.Title

                }).ToList();
        }
        private IEnumerable<ProductCategory> IndexGetSortedCategoryList(IEnumerable<ProductCategory> categorySearchResult, bool isDesc, string col, ref CategoryIndexViewModel viewModel)
        {
            viewModel.IsDescTitle = false;
            viewModel.IsDescId = false;
            viewModel.SelectedColumn = col;
            viewModel.IsDesc = isDesc;

            if (isDesc && col == "title")
            {
                categorySearchResult = categorySearchResult.OrderByDescending(cat => cat.Title);

            }
            else if (!isDesc && col == "title")
            {
                categorySearchResult = categorySearchResult.OrderBy(cat => cat.Title);
                viewModel.IsDescTitle = true;
            }
            else if (isDesc && col == "id")
            {
                categorySearchResult = categorySearchResult.OrderByDescending(cat => cat.Id);
            }
            else if (!isDesc && col == "id")
            {
                categorySearchResult = categorySearchResult.OrderBy(cat => cat.Id);
                viewModel.IsDescId = true;
            }

            return categorySearchResult;
        }
        private CategoryShowTreeViewModel.SubCategoryItem _ShowTreeGetSubCategoryById(int id)
        {
            var subCat = _subCategoryRepository.GetById(id);
            return new CategoryShowTreeViewModel.SubCategoryItem
            {
                NumberOfProducts = _productRepository.CountBySubCategoryId(subCat.Id),
                Title = subCat.Title
            };
        }
        private CategoryShowTreeViewModel.CategoryItem _ShowTreeGetCategoryBySubCategoryId(int id)
        {
            var category = _categoryRepository.GetBySubCategoryId(id);
            return new CategoryShowTreeViewModel.CategoryItem
            {
                Id = category.Id,
                Title = category.Title,
                NumberOfProducts = _productRepository.CountByCategoryId(category.Id)
            };
        }
        private List<CategorySelectSubCategoryViewModel.ProductItem> _SelectSubCategoryGetProductsBySubCategoryId(int id)
        {
            return _productRepository.GetBySubCategoryId(id).Select(prod => new CategorySelectSubCategoryViewModel.ProductItem
            {
                Id = prod.Id,
                CompanyId = prod.Company.Id,
                Price = prod.Price,
                CompanyTitle = prod.Company.Title,
                Rate = (int)_reviewRepository.GetRateByProductId(prod.Id),
                Title = prod.Title,
                IsTopRated = _productRepository.IsTopRated(prod.Id, _topRatedAmount),
                ImgTitle = prod.imgTitle

            }).ToList();
        }
        private List<CategoryDisplayProductsViewModel.SubCategoryItem> DisplayProductsGetSubCategoriesByCategoryId(int categoryId, int subCatId)
        {
            return _subCategoryRepository.GetByCategoryId(categoryId).Select(subCat =>
                new CategoryDisplayProductsViewModel.SubCategoryItem
                {
                    Id = subCat.Id,
                    Title = subCat.Title,
                    Active = subCat.Id == subCatId ? "active" : ""


                }).ToList();
        }
        private CategoryDisplayProductsViewModel.SubCategoryItem DisplayProductsGetSubCategoryById(int id)
        {
            var subCategory = _subCategoryRepository.GetById(id);
            return new CategoryDisplayProductsViewModel.SubCategoryItem
            {
                Id = subCategory.Id,
                Title = subCategory.Title,
                NumberOfProducts = _productRepository.CountBySubCategoryId(subCategory.Id)
            };

        }
        private List<CategoryDisplayProductsViewModel.CompanyItem> DisplayProductsGetCompaniesBySubCategoryId(int id)
        {
            return _productRepository.GetBySubCategoryId(id).Select(
                    p => new CategoryDisplayProductsViewModel.CompanyItem
                    {
                        Id = p.Company.Id,
                        Title = p.Company.Title,
                        NumberOfProducts = _productRepository.CountByCompanyId(p.Company.Id)
                    }).GroupBy(c => c.Id).Select(group => group.First()).ToList();
        }
        private List<CategoryDisplayProductsViewModel.ProductItem> DisplayProductsGetProductsBySubCategoryId(int id)
        {
            return _productRepository.GetBySubCategoryId(id).Select(p =>
                new CategoryDisplayProductsViewModel.ProductItem
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    CompanyId = p.Company.Id,
                    CompanyTitle = p.Company.Title,
                    Rate = (int)_reviewRepository.GetRateByProductId(p.Id),
                    ImgTitle = p.imgTitle,
                    IsTopRated = _productRepository.IsTopRated(p.Id, _topRatedAmount)
                }).ToList();
        }
        private List<CategoryDisplayProductsViewModel.SubCategoryItem> DisplayProductsGetSubCategoriesByCategoryId(int id)
        {
            return _subCategoryRepository.GetByCategoryId(id).Select(subCat => new CategoryDisplayProductsViewModel.SubCategoryItem
            {
                Id = subCat.Id,
                Title = subCat.Title
            }).ToList();
        }
        private List<CategoryDisplayProductsViewModel.CompanyItem> DisplayProductsGetCompaniesByCategoryId(int id)
        {
            return _productRepository.GetByCategoryId(id).Select(prod => new CategoryDisplayProductsViewModel.CompanyItem
            {
                Id = prod.Company.Id,
                Title = prod.Company.Title,
                NumberOfProducts = _productRepository.CountByCompanyId(prod.Company.Id)
            }).GroupBy(c => c.Id).Select(group => group.First()).ToList();

        }
        private List<CategoryDisplayProductsViewModel.ProductItem> DisplayProductsGetProductsByCategoryId(int id)
        {
            return _productRepository.GetByCategoryId(id).Select(p =>
                new CategoryDisplayProductsViewModel.ProductItem
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    Rate = (int)_reviewRepository.GetRateByProductId(p.Id),
                    CompanyTitle = p.Company.Title,
                    CompanyId = p.Company.Id,
                    ImgTitle = p.imgTitle,
                    IsTopRated = _productRepository.IsTopRated(p.Id, _topRatedAmount)

                }).ToList();
        }
        private List<CategoryDisplayProductsViewModel.ProductItem> DisplayProductsGetTopRatedProducts()
        {
            return _productRepository.GetAll().Select(p => new CategoryDisplayProductsViewModel.ProductItem
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                CompanyId = p.Company.Id,
                CompanyTitle = p.Company.Title,
                ImgTitle = p.imgTitle,
                Rate = (int)_reviewRepository.GetRateByProductId(p.Id)
            }).OrderByDescending(prod => prod.Rate).Take(_topRatedAmount).ToList();
        }
        private CategoryDisplayProductsViewModel.CategoryItem DisplayProductsGetCategoryById(int id)
        {
            var category = _categoryRepository.GetById(id);
            return new CategoryDisplayProductsViewModel.CategoryItem
            {
                Id = category.Id,
                Title = category.Title,
                NumberOfProducts = _productRepository.CountByCategoryId(category.Id)
            };

        }
        private ProductCategory CreateCategory(CategoryEditViewModel viewModel)
        {
            return new ProductCategory
            {
                Id = viewModel.Id,
                Title = viewModel.Title
            };
        }
        private ProductCategory CreateCategory(CategoryNewViewModel viewModel)
        {
            return new ProductCategory()
            {
                Title = viewModel.Title
            };
        }
    }
}
