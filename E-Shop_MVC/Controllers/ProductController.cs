using E_Shop_MVC.Models.Data;
using E_Shop_MVC.Models.Data.Interfaces;
using E_Shop_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace E_Shop_MVC.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductRepository _productRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly int _topRatedAmount = 3;
        private readonly int _productsPerPage = 7;
        private readonly string _defaultImgTitle = "default.png";


        public ProductController(
            IProductRepository productRepository,
            IReviewRepository reviewRepository,
            ICompanyRepository companyRepository,
            ISubCategoryRepository subCategoryRepository,
            ICategoryRepository categoryRepository,
            IColorRepository colorRepository,
            IWebHostEnvironment hostingEnvironment
        )
        {
            _productRepository = productRepository;
            _reviewRepository = reviewRepository;
            _companyRepository = companyRepository;
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
            _colorRepository = colorRepository;
            _hostingEnvironment = hostingEnvironment;
        }


        [Authorize]
        public IActionResult Index(string q, bool isDesc, string col, int pageNr = 1)
        {
            var viewModel = new ProductIndexViewModel();

            var productSearchResult = _productRepository.GetSearchResult(q).ToList();
            var sortedProducts = GetSortedProductList(productSearchResult, isDesc, col, ref viewModel);

            viewModel.Products = GetProductsByPageNr(pageNr, sortedProducts);
            viewModel.TotalProductsAmount = productSearchResult.Count();
            viewModel.ProductAmountPerPage = _productsPerPage;
            viewModel.TotalNumberOfPages = (int)Math.Ceiling(viewModel.TotalProductsAmount / viewModel.ProductAmountPerPage);
            viewModel.SelectedPageNumber = pageNr;
            viewModel.q = q;

            return View(viewModel);
        }


        [Authorize]
        public IActionResult New()
        {
            var viewModel = new ProductNewViewModel();

            viewModel.Companies = GetCompaniesListItems();
            viewModel.SubCategories = GetSubCategoriesListItems();
            viewModel.Colors = GetColorsListItems();

            return View(viewModel);
        }


        [Authorize]
        [HttpPost]
        public IActionResult New(ProductNewViewModel viewModel)
        {
            if (viewModel.Price < 0)
                ModelState.AddModelError("Price", "Priset får inte vara negativt");
            else if (viewModel.Price > 999999)
                ModelState.AddModelError("Price", "Priset får max vara 999999 kr ");

            if (ModelState.IsValid)
            {
                var fileName = _defaultImgTitle;

                if (viewModel.Image != null)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath + "\\img");
                    fileName = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName;
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    viewModel.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                var newProduct = CreateProduct(viewModel, fileName);
                _productRepository.Add(newProduct);

                return RedirectToAction("Details", new { prodId = newProduct.Id });
            }

            viewModel.Companies = GetCompaniesListItems();
            viewModel.SubCategories = GetSubCategoriesListItems();
            viewModel.Colors = GetColorsListItems();

            return View(viewModel);
        }


        [Authorize]
        public IActionResult Edit(int prodId)
        {
            var viewModel = new ProductEditViewModel();

            viewModel = EditMapSelectedProduct(prodId, viewModel);
            viewModel.Companies = EditGetCompaniesListItems();
            viewModel.SubCategories = EditGetSubCategoriesListItems();
            viewModel.Colors = EditGetColorsListItems();

            return View(viewModel);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Edit(ProductEditViewModel viewModel)
        {
            if (viewModel.Price < 0)
                ModelState.AddModelError("Price", "Priset får inte vara negativt");
            else if (viewModel.Price > 999999)
                ModelState.AddModelError("Price", "Priset får max vara 999999 kr ");

            if (ModelState.IsValid)
            {
                var updatedProduct = CreateProduct(viewModel);
                _productRepository.Update(updatedProduct);

                return RedirectToAction("Details", new { prodId = updatedProduct.Id });
            }

            viewModel = EditMapSelectedProduct(viewModel.Id, viewModel);
            viewModel.Companies = EditGetCompaniesListItems();
            viewModel.SubCategories = EditGetSubCategoriesListItems();
            viewModel.Colors = EditGetColorsListItems();

            return View(viewModel);
        }


        public IActionResult Details(int prodId)
        {
            var viewModel = new ProductDetailsViewModel();

            viewModel.Product = DetailsGetProductById(prodId);
            viewModel.SubCategory = DetailsGetSubCategoryByProductId(prodId);
            viewModel.Category = DetailsGetCategoryByProductId(prodId);
            viewModel.Reviews = DetailsGetReviewsByProductId(prodId);
            viewModel.RelatedProducts = DetailsGetRelatedProductsById(prodId);
            viewModel.ProductId = prodId;
            viewModel.ReviewCategoryId = _categoryRepository.GetByProductId(prodId).Id;
            viewModel.ReviewSubCategoryId = _subCategoryRepository.GetByProductId(prodId).Id;
            viewModel.ReviewAmount = _reviewRepository.CountByProductId(prodId);

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Details(ProductDetailsViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                var newReview = CreateReview(viewModel, id);
                _reviewRepository.Add(newReview);

                return RedirectToAction("Index", "Review", new { prodId = id, catId = viewModel.ReviewCategoryId, subCatId = viewModel.ReviewSubCategoryId });
            }

            viewModel.Product = DetailsGetProductById(id);
            viewModel.SubCategory = DetailsGetSubCategoryByProductId(id);
            viewModel.Category = DetailsGetCategoryByProductId(id);
            viewModel.Reviews = DetailsGetReviewsByProductId(id);
            viewModel.RelatedProducts = DetailsGetRelatedProductsById(id);
            viewModel.ProductId = id;
            viewModel.ReviewCategoryId = _categoryRepository.GetByProductId(id).Id;
            viewModel.ReviewSubCategoryId = _subCategoryRepository.GetByProductId(id).Id;

            return View(viewModel);
        }




        private List<ProductIndexViewModel.ProductItem> GetProductsByPageNr(int pageNr, IEnumerable<Product> sortedProducts)
        {
            return sortedProducts.Skip((pageNr - 1) * _productsPerPage).Take(_productsPerPage).Select(prod =>
                new ProductIndexViewModel.ProductItem
                {
                    Id = prod.Id,
                    Title = prod.Title,
                    Price = prod.Price,
                    CompanyTitle = prod.Company.Title,
                    ImgTitle = prod.imgTitle,
                    SubCategoryTitle = prod.SubCategory.Title
                }).ToList();
        }
        private IEnumerable<Product> GetSortedProductList(IEnumerable<Product> productSearchResult, bool isDesc, string col, ref ProductIndexViewModel viewModel)
        {
            viewModel.IsDescCompany = false;
            viewModel.IsDescPrice = false;
            viewModel.IsDescSubCategory = false;
            viewModel.IsDescTitle = false;
            viewModel.IsDescId = false;
            viewModel.SelectedColumn = col;
            viewModel.IsDesc = isDesc;

            if (isDesc && col == "title")
            {
                productSearchResult = productSearchResult.OrderByDescending(prod => prod.Title);

            }
            else if (!isDesc && col == "title")
            {
                productSearchResult = productSearchResult.OrderBy(prod => prod.Title);
                viewModel.IsDescTitle = true;
            }
            else if (isDesc && col == "company")
            {
                productSearchResult = productSearchResult.OrderByDescending(prod => prod.Company.Title);
            }
            else if (!isDesc && col == "company")
            {
                productSearchResult = productSearchResult.OrderBy(prod => prod.Company.Title);
                viewModel.IsDescCompany = true;
            }
            else if (isDesc && col == "id")
            {
                productSearchResult = productSearchResult.OrderByDescending(prod => prod.Id);
            }
            else if (!isDesc && col == "id")
            {
                productSearchResult = productSearchResult.OrderBy(prod => prod.Id);
                viewModel.IsDescId = true;
            }
            else if (isDesc && col == "subCategory")
            {
                productSearchResult = productSearchResult.OrderByDescending(prod => prod.SubCategory.Title);
            }
            else if (!isDesc && col == "subCategory")
            {
                productSearchResult = productSearchResult.OrderBy(prod => prod.SubCategory.Title);
                viewModel.IsDescSubCategory = true;
            }
            else if (isDesc && col == "price")
            {
                productSearchResult = productSearchResult.OrderByDescending(prod => prod.Price);
            }
            else if (!isDesc && col == "price")
            {
                productSearchResult = productSearchResult.OrderBy(prod => prod.Price);
                viewModel.IsDescPrice = true;
            }

            return productSearchResult;
        }
        private ProductEditViewModel EditMapSelectedProduct(int prodId, ProductEditViewModel viewModel)
        {
            var product = _productRepository.GetById(prodId);
            viewModel.Id = product.Id;
            viewModel.Title = product.Title;
            viewModel.ShortDescription = product.ShortDescription;
            viewModel.InStock = product.InStock;
            viewModel.WarrantyYears = product.Warranty;
            viewModel.Price = product.Price;
            viewModel.LongDescription = product.LongDescription;
            viewModel.SelectedCompanyId = product.Company.Id;
            viewModel.SelectedSubCategoryId = product.SubCategory.Id;
            viewModel.SelectedColorId = product.Color.Id;
            viewModel.ImgTitle = product.imgTitle;
            return viewModel;
        }
        private List<SelectListItem> EditGetColorsListItems()
        {
            var colors = new List<SelectListItem>();
            colors.AddRange(_colorRepository.GetAll().Select(col => new SelectListItem
            {
                Text = col.Title,
                Value = col.Id.ToString()
            }));
            return colors;
        }
        private List<SelectListItem> EditGetSubCategoriesListItems()
        {
            var subCategories = new List<SelectListItem>();
            subCategories.AddRange(_subCategoryRepository.GetAll().Select(subCat => new SelectListItem
            {
                Text = subCat.Title,
                Value = subCat.Id.ToString()
            }));
            return subCategories;
        }
        private List<SelectListItem> EditGetCompaniesListItems()
        {
            var companies = new List<SelectListItem>();
            companies.AddRange(_companyRepository.GetAll().Select(com => new SelectListItem
            {
                Text = com.Title,
                Value = com.Id.ToString()
            }));
            return companies;
        }
        private Product CreateProduct(ProductNewViewModel viewModel, string fileName)
        {
            return new Product
            {
                Title = viewModel.Title,
                Color = _colorRepository.GetById(viewModel.SelectedColorId),
                Company = _companyRepository.GetById(viewModel.SelectedCompanyId),
                Price = viewModel.Price,
                InStock = viewModel.InStock,
                ShortDescription = viewModel.ShortDescription,
                LongDescription = viewModel.LongDescription,
                SubCategory = _subCategoryRepository.GetById(viewModel.SelectedSubCategoryId),
                Warranty = viewModel.WarrantyYears,
                imgTitle = fileName
            };
        }
        private List<SelectListItem> GetColorsListItems()
        {
            var colors = new List<SelectListItem>();
            colors.Add(new SelectListItem { Value = "0", Text = "...Välj Färg..." });
            colors.AddRange(_colorRepository.GetAll().Select(col => new SelectListItem
            {
                Text = col.Title,
                Value = col.Id.ToString()
            }));
            return colors;

        }
        private List<SelectListItem> GetSubCategoriesListItems()
        {
            var subCategories = new List<SelectListItem>();
            subCategories.Add(new SelectListItem { Value = "0", Text = "...Välj Underkategori..." });
            subCategories.AddRange(_subCategoryRepository.GetAll().Select(subCat => new SelectListItem
            {
                Text = subCat.Title,
                Value = subCat.Id.ToString()
            }));
            return subCategories;
        }
        private List<SelectListItem> GetCompaniesListItems()
        {
            var companies = new List<SelectListItem>();
            companies.Add(new SelectListItem { Value = "0", Text = "...Välj Varumärke..." });
            companies.AddRange(_companyRepository.GetAll().Select(com => new SelectListItem
            {
                Text = com.Title,
                Value = com.Id.ToString()
            }));
            return companies;

        }
        private List<ProductDetailsViewModel.ProductItem> DetailsGetRelatedProductsById(int prodId)
        {
            return _productRepository.GetRelatedProductsById(prodId).Select(prod =>
                new ProductDetailsViewModel.ProductItem
                {
                    Id = prod.Id,
                    Title = prod.Title,
                    Price = prod.Price,
                    AvgRate = (int)_reviewRepository.GetRateByProductId(prod.Id),
                    CompanyId = prod.Company.Id,
                    CompanyTitle = prod.Company.Title,
                    ImgTitle = prod.imgTitle,
                    IsTopRated = _productRepository.IsTopRated(prod.Id, _topRatedAmount)
                }).Take(4).ToList();
        }
        private List<ProductDetailsViewModel.ReviewItem> DetailsGetReviewsByProductId(int prodId)
        {
            return _reviewRepository.GetByProductId(prodId).Select(rev => new ProductDetailsViewModel.ReviewItem
            {
                Rate = rev.Rate,
                CustomerName = rev.CustomerName,
                Date = rev.Date,
                Review = rev.ReviewText
            }).OrderByDescending(rev => rev.Date).Take(2).ToList();
        }
        private ProductDetailsViewModel.CategoryItem DetailsGetCategoryByProductId(int prodId)
        {
            var category = _categoryRepository.GetByProductId(prodId);
            return new ProductDetailsViewModel.CategoryItem
            {
                Id = category.Id,
                Title = category.Title,
                NumberOfProducts = _productRepository.CountByCategoryId(category.Id)
            };
        }
        private ProductDetailsViewModel.SubCategoryItem DetailsGetSubCategoryByProductId(int prodId)
        {
            var subCategory = _subCategoryRepository.GetByProductId(prodId);
            return new ProductDetailsViewModel.SubCategoryItem
            {
                Id = subCategory.Id,
                Title = subCategory.Title,
                NumberOfProducts = _productRepository.CountBySubCategoryId(subCategory.Id)
            };
        }
        private ProductDetailsViewModel.ProductDetailsItem DetailsGetProductById(int prodId)
        {
            var product = _productRepository.GetById(prodId);
            return new ProductDetailsViewModel.ProductDetailsItem
            {
                Id = product.Id,
                Title = product.Title,
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                Price = product.Price,
                CompanyId = product.Company.Id,
                CompanyTitle = product.Company.Title,
                AvgRate = (int)_reviewRepository.GetRateByProductId(product.Id),
                Color = product.Color.Title,
                ImgTitle = product.imgTitle,
                NumberOfReviews = _reviewRepository.CountByProductId(product.Id),
                InStock = _productRepository.InStockById(product.Id),
                WarrantyYears = product.Warranty
            };
        }
        private Product CreateProduct(ProductEditViewModel viewModel)
        {
            return new Product
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Company = _companyRepository.GetById(viewModel.SelectedCompanyId),
                SubCategory = _subCategoryRepository.GetById(viewModel.SelectedSubCategoryId),
                Price = viewModel.Price,
                Warranty = viewModel.WarrantyYears,
                Color = _colorRepository.GetById(viewModel.SelectedColorId),
                InStock = viewModel.InStock,
                ShortDescription = viewModel.ShortDescription,
                LongDescription = viewModel.LongDescription
            };
        }
        private ProductReview CreateReview(ProductDetailsViewModel viewModel, int id)
        {
            return new ProductReview
            {
                CustomerName = viewModel.Review.CustomerName,
                Rate = viewModel.Review.Rate,
                ReviewText = viewModel.Review.ReviewText,
                Product = _productRepository.GetById(id),
                Date = DateTime.Now
            };
        }

    }
}
